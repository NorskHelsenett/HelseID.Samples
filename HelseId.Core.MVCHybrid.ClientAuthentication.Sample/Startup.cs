using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Core.MVCHybrid.ClientAuthentication.Sample
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
                 
                    options.SignInScheme = settings.SignInScheme;
                    options.RequireHttpsMetadata = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;


                    options.Authority = settings.Authority;
                    options.ClientId = settings.ClientId;
                    // Use the following line if using a shared secret for client authentication
                    //options.ClientSecret = settings.ClientSecret; 
                    options.ResponseType = settings.ResponseType;
                    options.Scope.Add(settings.Scope);

                    // Use the following event handler if using a Signed Jwt for client auhtentication
                    options.Events.OnAuthorizationCodeReceived = ctx =>
                    {
                        var rsa = RSA.Create();
                        rsa.FromXmlString(settings.ClientSecret);
                        var securityKey = new RsaSecurityKey(rsa);

                        ctx.TokenEndpointRequest.ClientAssertionType = OidcConstants.ClientAssertionTypes.JwtBearer;
                        ctx.TokenEndpointRequest.ClientAssertion = ClientAssertion.Generate(settings.ClientId, settings.Authority, securityKey);

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
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
          
        }
    }
}