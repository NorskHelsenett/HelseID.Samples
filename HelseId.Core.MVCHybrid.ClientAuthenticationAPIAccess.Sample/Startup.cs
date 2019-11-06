using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace  HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccess.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(option => option.EnableEndpointRouting = false);
            MvcOptions opt = new MvcOptions();
            opt.EnableEndpointRouting = false;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";

            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";                                      
                    options.RequireHttpsMetadata = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    //Configuration for HelseID - Utvikling enviroment
                    options.Authority = "https://helseid-sts.utvikling.nhn.no/";
                    options.ClientId = "NewSample-MVCHybridClientAuthentication";
                    options.ClientSecret = "vbHwjXlKYILNpafLgOwgrviAn8R3XFkHXNnLSaryqe7I8Y03zSLtmg5FICkHZEHC";
                    options.ResponseType = "code";             

                    //Scopes
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("helseid://scopes/identity/pid");
                    options.Scope.Add("helseid://scopes/identity/pid_pseudonym");
                    options.Scope.Add("helseid://scopes/identity/assurance_level");
                    options.Scope.Add("helseid://scopes/identity/security_level");                    
                    options.Scope.Add("helseid://scopes/hpr/hpr_number");
                    options.Scope.Add("helseid://scopes/identity/network");

                    //Scope to access API  
                    options.Scope.Add("willy:newsampleapi/");


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
