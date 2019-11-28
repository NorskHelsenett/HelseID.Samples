using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HelseId.Core.MVCHybrid.API.Sample
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


            services.AddControllers();

            services.AddCors();
            services.AddDistributedMemoryCache();

            services.AddAuthentication("token")
                .AddIdentityServerAuthentication("token", options =>
                {
                   
                    
                    options.RequireHttpsMetadata = false;

                    //Configuration for HelseID - Utvikling enviroment
                    options.Authority = settings.Authority;
                    options.ApiName = settings.ApiName;
                    options.ApiSecret = settings.ApiSecret;
             

                    options.JwtBearerEvents = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                    {
                        OnTokenValidated = e =>
                        {
                            var jwt = e.SecurityToken as JwtSecurityToken;
                            var type = jwt.Header.Typ;
                        
                            //TODO: Verify scopes
                           

                            return Task.CompletedTask;
                        }
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

            app.UseCors(policy =>
            {
                policy.WithOrigins(
                    
                   "http://localhost:5003");

                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.WithExposedHeaders("WWW-Authenticate");
            });

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
