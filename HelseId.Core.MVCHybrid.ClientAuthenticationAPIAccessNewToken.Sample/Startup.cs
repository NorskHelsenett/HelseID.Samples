using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample
{
    public class Startup
    {

        public readonly string _scopes;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
    }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Settings from appsettings.json
            Settings settings = new Settings();
            Configuration.GetSection("Settings").Bind(settings);
            // Create singleton from instance
            services.AddSingleton<Settings>(settings);

            services.AddHttpClient();

            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(settings.Authority, () => factory.CreateClient());
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);
            MvcOptions opt = new MvcOptions();
            opt.EnableEndpointRouting = false;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();


            //settings for HelseId utvikling-enviroment are set in appsettings.json
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = settings.DefaultChallengeScheme;

            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    
                    options.SignInScheme = settings.SignInScheme;                   
                    options.RequireHttpsMetadata = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    
                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                    options.ResponseType = settings.ResponseType;
                    options.Scope.Add(settings.Scope);

                    


                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

        }
    }
}
