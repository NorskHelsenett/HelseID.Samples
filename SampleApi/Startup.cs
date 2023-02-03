using HelseId.SampleApi.Configuration;
using HelseId.SampleAPI.Controllers;
using HelseId.SampleApi.Interfaces;

namespace HelseId.SampleAPI;

public  class Startup
{
    private readonly Settings _settings;
    
    public const string TokenAuthenticationScheme = "token_authentication_scheme";
    public const string AuthCodePolicy = "auth_code_policy";
    public const string ClientCredentialsPolicy = "client_credentials_policy";

    public Startup(Settings settings)
    {
        _settings = settings;
    }

    public WebApplication BuildWebApplication()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();

        AddServices(webApplicationBuilder);

        SetUpKestrel(webApplicationBuilder);

        var webApplication = webApplicationBuilder.Build();

        ConfigureServices(webApplication);

        return webApplication;
    }

    private void AddServices(WebApplicationBuilder webApplicationBuilder)
    {
        // Create singleton from instance
        webApplicationBuilder.Services.AddSingleton(_settings);
        webApplicationBuilder.Services.AddSingleton<IApiResponseCreator, ApiResponseCreator>();
        
        webApplicationBuilder.Services.AddMvc(option => option.EnableEndpointRouting = false);
        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();

        // Adds the authentication scheme to be used for bearer tokens
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationScheme)
            .AddJwtBearer(TokenAuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.Authority = _settings.Authority;
                options.Audience = _settings.Audience;

                // Validation parameters are in agreement with HelseIDs requirements:
                // https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs
                // These (and a few others) are all true by default
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RequireSignedTokens = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.RequireAudience = true;
            });

        webApplicationBuilder.Services.AddAuthorization(options =>
        {   
            // Add a policy for verifying scopes and claims for a logged on user
            options.AddPolicy(
                AuthCodePolicy,
                policy => policy.RequireClaim("scope", _settings.AuthCodeApiScopeForSampleApi)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));

            // Add a policy for verifying scopes for client credentials
            options.AddPolicy(
                ClientCredentialsPolicy, 
                policy => policy.RequireClaim("scope", _settings.ClientCredentialsApiScopeForSampleApi));
        });        
    }

    private void SetUpKestrel(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.WebHost.UseKestrel(kestrelServerOptions =>
        {
            // Bind directly to a socket handle or Unix socket
            kestrelServerOptions.ListenLocalhost(_settings.ApiPort, listenOptions => listenOptions.UseHttps());
        });
    }

    private void ConfigureServices(WebApplication webApplication)
    {
        // Configure the HTTP request pipeline.
        webApplication.UseHttpsRedirection();
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.MapControllers();
    }
}