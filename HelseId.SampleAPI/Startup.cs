using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HelseId.SampleAPI
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

            services.AddHttpClient();

            services.AddMvc(option => option.EnableEndpointRouting = false);
            MvcOptions opt = new MvcOptions();
            opt.EnableEndpointRouting = false;

            // Adding Cross Origin Resource Sharing (CORS) to the container.
            // CORS allows a server to make cross-domain calls from specified domains, while rejecting others.
            services.AddCors();

            // Stores items in memory.
            services.AddDistributedMemoryCache();

            // Adds the authentication scheme to be used for bearer tokens with user information.
            services.AddAuthentication("token")
                    .AddJwtBearer("token", options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.Authority = settings.Authority;
                        options.Audience = settings.ApiName;

                        // Validation parameters in agreement with HelseIDs requirements (https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs)
                        options.TokenValidationParameters.ValidateLifetime = true;
                        options.TokenValidationParameters.ValidateIssuer = true;
                        options.TokenValidationParameters.ValidateAudience = true;
                        options.TokenValidationParameters.RequireSignedTokens = true;
                        options.TokenValidationParameters.RequireExpirationTime = true;
                        options.TokenValidationParameters.RequireAudience = true;
                    });


            // Adds the authentication scheme to be used for bearer tokens with NO user information.
            services.AddAuthentication("client_credentials_scheme")
                   .AddJwtBearer("client_credentials_scheme", options =>
                   {
                       options.RequireHttpsMetadata = true;
                       options.Authority = settings.Authority;
                       options.Audience = settings.ClientCredentialsApiName;

                        // Validation parameters in agreement with HelseIDs requirements (https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs)
                        options.TokenValidationParameters.ValidateLifetime = true;
                       options.TokenValidationParameters.ValidateIssuer = true;
                       options.TokenValidationParameters.ValidateAudience = true;
                       options.TokenValidationParameters.RequireSignedTokens = true;
                       options.TokenValidationParameters.RequireExpirationTime = true;
                       options.TokenValidationParameters.RequireAudience = true;
                   });

            services.AddAuthorization(options =>
            {   
                // Verify scopes for user
                options.AddPolicy("auth_code_policy", policy => policy
                .RequireClaim("scope", settings.AuthCodeApiScope)
                .RequireClaim("helseid://claims/identity/pid")
                .RequireClaim("helseid://claims/identity/security_level", "4"));

                // Verify scopes for client credentials
                options.AddPolicy("client_credentials_policy", policy => policy
                    .RequireClaim("scope", settings.ClientCredentialsApiScope));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adds a CORS middleware with a given policy to the pipline, to allow cross domain requests
            app.UseCors(policy =>
            {
                policy.WithOrigins("http://localhost:5003"); // Allow requests from the given domain
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate"); // Adds the specified headers which need to be exposed to the client
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvcWithDefaultRoute();
        }
    }
}
