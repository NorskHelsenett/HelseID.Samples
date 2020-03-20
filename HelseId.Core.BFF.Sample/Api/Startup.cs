using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using HelseId.Core.BFF.Sample.Api.Authorization;
using HelseId.Core.BFF.Sample.Api.Identity;
using HelseId.Core.BFF.Sample.Api.Options;
using HelseId.Core.BFF.Sample.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using HelseId.Core.BFF.Sample.WebCommon.Middleware;

namespace HelseId.Core.BFF.Sample.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            _env = env;
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                services.AddHttpsRedirection(
                    options =>
                    {
                        // Always use port 443 in prod
                        options.HttpsPort = 443;
                    }
                );
            }

            services.AddOptions<HelseIdOptions>()
                .Bind(_configuration.GetSection("HelseId"))
                .ValidateDataAnnotations();

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentHttpUser>();
            services.AddScoped<IAccessTokenProvider, HttpContextAccessTokenProvider>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.Authority = _configuration["HelseId:Authority"];
                        options.Audience = _configuration["HelseId:ApiName"];
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.RefreshOnIssuerKeyNotFound = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            RequireSignedTokens = true,
                            RequireAudience = true,
                            RequireExpirationTime = true,
                            ValidateIssuer = true,
                            ValidateIssuerSigningKey = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                        };
                    }
                );

            services.AddAuthorization(
                config =>
                {
                    var authenticatedHidUserPolicy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    var apiAccessPolicy = new AuthorizationPolicyBuilder()
                        .Combine(authenticatedHidUserPolicy)
                        .RequireScope(_configuration["HelseId:ApiScope"])
                        .Build();

                    config.DefaultPolicy = apiAccessPolicy;

                    config.AddPolicy(Policies.HidAuthenticated, authenticatedHidUserPolicy);
                    config.AddPolicy(Policies.ApiAccess, apiAccessPolicy);
                }
            );

            services.AddControllers(config => { config.Filters.Add(new AuthorizeFilter(Policies.ApiAccess)); })
                .AddJsonOptions(
                    options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); }
                );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            ValidateOptions(provider);

            app.UseUnhandledExceptionLogger();
            app.UseSerilogRequestLogging();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseForwardedHeaders();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ValidateOptions(IServiceProvider provider)
        {
            ValidateOptions<HelseIdOptions>(provider);
        }

        private static void ValidateOptions<TOptions>(IServiceProvider provider)
        {
            // Runs validation when constructing options instance.
            var unused = provider.GetRequiredService<IOptionsMonitor<TOptions>>().CurrentValue;
        }
    }
}