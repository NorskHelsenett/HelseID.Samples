using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HelseId.Core.BFF.Sample.Client.Infrastructure.AutomaticTokenManagement;
using HelseId.Core.BFF.Sample.Client.Middleware;
using HelseId.Core.BFF.Sample.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;
using HelseId.Core.BFF.Sample.WebCommon.Identity;
using HelseId.Core.BFF.Sample.WebCommon.Middleware;

namespace HelseId.Core.BFF.Sample.Client
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            this._configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri(_configuration["ApiUrl"]);
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            });

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
                    var hidConfig = _configuration.GetSection("HelseId");
                    var acrValues = hidConfig["AcrValues"];
                    var hasAcrValues = !string.IsNullOrWhiteSpace(acrValues);

                    options.Authority = hidConfig["Authority"];
                    options.RequireHttpsMetadata = true;
                    options.ClientId = hidConfig["ClientId"];
                    options.ClientSecret = hidConfig["ClientSecret"];
                    options.ResponseType = "code";
                    options.TokenValidationParameters.ValidAudience = hidConfig["ClientId"];

                    options.AccessDeniedPath = "/Forbidden";

                    var scopes = hidConfig["Scopes"].Split(' ');
                    options.Scope.Clear();
                    foreach (var scope in scopes)
                    {
                        options.Scope.Add(scope.Trim());
                    }

                    options.SaveTokens = true;

                    options.Events.OnRedirectToIdentityProvider = ctx =>
                    {
                        // API requests should get a 401 status instead of being redirected to login
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.Headers["Location"] = ctx.ProtocolMessage.CreateAuthenticationRequestUrl();
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            ctx.HandleResponse();
                        }

                        if (ctx.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication && hasAcrValues)
                        {
                            ctx.ProtocolMessage.AcrValues = acrValues;
                        }

                        return Task.CompletedTask;
                    };
                })
                .AddAutomaticTokenManagement(options =>
                {
                    options.RefreshBeforeExpiration = TimeSpan.FromMinutes(2);
                    options.RevokeRefreshTokenOnSignout = true;
                    options.Scheme = "HelseID";

                    options.CookieEvents.OnRedirectToAccessDenied = ctx =>
                    {
                        // API requests should get a 403 status instead of being redirected to access denied page
                        if (ctx.Request.Path.StartsWithSegments("/api"))
                        {
                            ctx.Response.Headers["Location"] = ctx.RedirectUri;
                            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                        }

                        return Task.CompletedTask;
                    };
                });

            var authenticatedHidUserPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(IdentityClaims.SecurityLevel, SecurityLevel.Level4)
                .Build();
            var apiScopePolicy = new AuthorizationPolicyBuilder()
                .Combine(authenticatedHidUserPolicy)
                .RequireScope(_configuration["ApiScope"])
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
                    endpoints.MapControllers();
                    endpoints.MapRazorPages();
                });

            app.UseProtectPaths(new ProtectPathsOptions("HidAuthenticated", "/Forbidden")
            {
                Exclusions = new List<PathString> {"/favicon.ico"}
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSpa(builder => { });
        }
    }
}