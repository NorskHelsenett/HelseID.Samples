using HelseId.APIAccess.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace HelseId.APIAccess
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

            // Settings from appsettings.json
            Settings settings = new Settings();
            Configuration.GetSection("Settings").Bind(settings);

            // Create singleton from instance
            services.AddSingleton<Settings>(settings);

            // Setup for Model-View-Controller (MVC) functionality
            services.AddMvc(option => option.EnableEndpointRouting = false);
            MvcOptions opt = new MvcOptions();
            opt.EnableEndpointRouting = false;

            // Disables default namespaces added by ASP.NET Core, and uses the exact claims created with the OpenID Server instead
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // The following section configures the authentication scheme to be used when authenticating a user
            // The scheme uses OpenID Connect ("oidc") and Cookies ("Cookies") as authentication mechanisms
            // If a cookie does not exist the OpenID Connect is challenged to authenticate the user
            // A Json Web Key (Jwk) pair is used as client secret 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc"; // OpenID connect is used as authentication mechanism when a user wants to access the requested resource
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // Configuration for the OpenID Connect handler (reads data from "appsettings.json")
                    options.SignInScheme = settings.SignInScheme;
                    options.RequireHttpsMetadata = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    options.ResponseType = settings.ResponseType;

                    options.Scope.Clear();
                    foreach (var scope in settings.Scope.Split(' '))
                    {
                        options.Scope.Add(scope);
                    };

                    // The following handler uses a Signed Jwt for client authentication
                    options.Events.OnAuthorizationCodeReceived = ctx =>
                    {
                        // Sets the client assertion as a Jwt bearer type
                        ctx.TokenEndpointRequest.ClientAssertionType = OidcConstants.ClientAssertionTypes.JwtBearer;
                        // Asserts a client by using the generated Jwt
                        ctx.TokenEndpointRequest.ClientAssertion = BuildClientAssertion.Generate(settings.ClientId, settings.Authority, "jwk.json");

                        return Task.CompletedTask;
                    };
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
            app.UseAuthentication(); // Registers the authentication middleware
            app.UseMvcWithDefaultRoute();
        }
    }
}
