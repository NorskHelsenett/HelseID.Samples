using HelseId.ClienAuthentication;
using HelseId.ClientAuthentication.Models;
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

namespace HelseId.ClientAuthentication
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

            var settings = new Settings {
                Authority = "https://helseid-sts.test.nhn.no/",
                ClientId = "helseid-sample-client-authentication",
                ResponseType = "code",
                DefaultChallengeScheme = "oidc",
                Scope = "openid profile helseid://scopes/identity/pid helseid://scopes/identity/security_level",
                SignInScheme = "Cookies",
            };

            // Create singleton from instance
            services.AddSingleton(settings);

            // Setup for Model-View-Controller (MVC) functionality
            services.AddMvc(option => option.EnableEndpointRouting = false);
            MvcOptions opt = new MvcOptions();
            opt.EnableEndpointRouting = false;

            // Disables default namespaces added by ASP.NET Core, and uses the exact claims created with the OpenID Server instead
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // The following section configures the authentication scheme to be used when authenticating a user
            // The scheme uses OpenID Connect ("oidc") and Cookies ("Cookies") as authentication mechanisms
            // If a cookie does not exist the OpenID Connect is challenged to authenticate the user
            // A private key formatted as a Json Web Key (Jwk) is used as client secret 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc"; // OpenID connect is used as authentication mechanism when a user wants to access the requested resource
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    // Configuration for the OpenID Connect handler (reads data from HelseID utvikling-environment in "appsettings.json")
                    options.SignInScheme = settings.SignInScheme;
                    options.RequireHttpsMetadata = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    options.ResponseType = settings.ResponseType;

                    options.Scope.Clear();
                    foreach (var scope in settings.Scope.Split(' ')) {
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
