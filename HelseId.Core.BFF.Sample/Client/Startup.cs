using Duende.AccessTokenManagement;
using HelseId.Core.BFF.Sample.Client.Auth;
using HelseId.Core.BFF.Sample.Client.Middleware;
using HelseId.Core.BFF.Sample.Client.Services;
using HelseId.Core.BFF.Sample.Models.Model;
using HelseId.Core.BFF.Sample.WebCommon.Identity;
using HelseId.Core.BFF.Sample.WebCommon.Middleware;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelseId.Core.BFF.Sample.Client
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Client used to make calls to the API.
            // Access token is automatically included and refreshed with 'AddUserAccessTokenHandler'.
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]!);
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            }).AddUserAccessTokenHandler();

            if (_env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                services.AddHttpsRedirection(options =>
                {
                    // Always use port 443 in prod
                    options.HttpsPort = 443;
                });
            }

            services.AddScoped<ICurrentUser, CurrentHttpUser>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var hidConfigSection = _configuration.GetRequiredSection("HelseId");
            var hidOptions = hidConfigSection.Get<HelseIdAuthOptions>()!;
            services.AddOptions<HelseIdAuthOptions>()
                .Bind(hidConfigSection)
                .ValidateDataAnnotations();

            var apiScope = _configuration["ApiScope"]!;

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = "HelseID";
                })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                    options.AccessDeniedPath = "/Forbidden";

                    // NOTE: options.Events must be set in AddAutomaticTokenManagement.
                    // This is because it overrides the events set here.
                })
                .AddOpenIdConnect("HelseID", options =>
                {
                    var acrValues = hidOptions.AcrValues;
                    var hasAcrValues = !string.IsNullOrWhiteSpace(acrValues);

                    options.Authority = hidOptions.Authority;
                    options.RequireHttpsMetadata = true;
                    options.ClientId = hidOptions.ClientId;
                    options.ResponseType = OidcConstants.ResponseTypes.Code;
                    options.UsePkce = true;
                    options.TokenValidationParameters.ValidAudience = hidOptions.ClientId;
                    options.CallbackPath = "/signin-oidc";
                    options.SignedOutCallbackPath = "/signout-callback-oidc";
                    options.MapInboundClaims = false;

                    options.AccessDeniedPath = "/Forbidden";

                    var scopes = hidOptions.Scopes.Split(' ');
                    options.Scope.Clear();
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope.Trim());
                    }

                    options.Scope.Add(apiScope);

                    options.SaveTokens = true;

                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        if (context.ProtocolMessage.RequestType != OpenIdConnectRequestType.Authentication)
                        {
                            return Task.CompletedTask;
                        }

                        // API requests should get a 401 status instead of being redirected to login
                        if (context.Request.Path.StartsWithSegments("/api"))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.HandleResponse();
                        }

                        return Task.CompletedTask;
                    };
                });

            var dpopKey = CreateDPoPJwk();
            services.AddSingleton(dpopKey);

            // Automatic refresh of access token
            services.AddOpenIdConnectAccessTokenManagement(options =>
            {
                options.RefreshBeforeExpiration = TimeSpan.FromSeconds(10);

                options.DPoPJsonWebKey = dpopKey.Jwk;
            });

            // Workaround to use Client Assertion and Pushed Authorization Request during logon.
            services.ConfigureOptions<ConfigureOpenIdConnectOptionsForHelseId>();

            // Use client assertion for automatic refresh of tokens
            services.AddTransient<IClientAssertionService, ClientAssertionService>();

            // This handler is a workaround for a bug in Duende.AccessTokenManagement that affects logon.
            services.Replace(ServiceDescriptor.Transient<OpenIdConnectHandler, DPoPOpenIdConnectHandler>());

            var authenticatedHidUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(IdentityClaims.SecurityLevel, SecurityLevel.Level4)
                .Build();
            var apiScopePolicy = new AuthorizationPolicyBuilder()
                .Combine(authenticatedHidUserPolicy)
                .RequireClaim("scope", apiScope)
                .Build();

            services.AddAuthorization(config =>
            {
                config.AddPolicy("HidAuthenticated", authenticatedHidUserPolicy);
                config.AddPolicy("ApiScope", apiScopePolicy);
                config.DefaultPolicy = authenticatedHidUserPolicy;
            });

            services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(authenticatedHidUserPolicy)));

            var razorPagesBuilder = services.AddRazorPages()
                .AddRazorPagesOptions(
                    options =>
                    {
                        options.Conventions.AllowAnonymousToPage("/Forbidden");
                        options.Conventions.AllowAnonymousToPage("/NotFound");
                        options.Conventions.AllowAnonymousToPage("/ServerError");
                        options.Conventions.AllowAnonymousToPage("/StatusCode");
                    }
                );
#if DEBUG
            if (_env.IsDevelopment())
            {
                razorPagesBuilder.AddRazorRuntimeCompilation();
            }
#endif
        }

        private static DPoPJwk CreateDPoPJwk()
        {
            var rsaKey = new RsaSecurityKey(RSA.Create(2048));
            var jwk = JsonWebKeyConverter.ConvertFromSecurityKey(rsaKey);
            jwk.Alg = SecurityAlgorithms.RsaSsaPssSha256;
            string jwkJson = JsonSerializer.Serialize(jwk);

            var dpopKey = new DPoPJwk(jwkJson, jwk.Alg);
            return dpopKey;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiClient apiClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseWhen(
                    context => !context.Request.Path.StartsWithSegments("/api"),
                    builder => builder.UseRedirectOnException("/ServerError")
                );
                app.UseHsts();
                app.UseForwardedHeaders();
            }

            app.UseUnhandledExceptionLogger();
            app.UseSerilogRequestLogging();

            app.UseWhen(
                context => !context.Request.Path.StartsWithSegments("/api"),
                builder => builder.UseStatusCodePagesWithReExecute("/StatusCode", "?code={0}")
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });

            app.UseProtectPaths(new ProtectPathsOptions("HidAuthenticated", "/Forbidden")
            {
                Exclusions = new List<PathString> { "/favicon.ico" }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSpa(builder => { });
        }
    }
}