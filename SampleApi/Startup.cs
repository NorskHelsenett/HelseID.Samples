using HelseId.SampleApi.Configuration;
using HelseId.SampleAPI.Controllers;
using HelseId.SampleApi.Interfaces;
using HelseId.Samples.Common.Configuration;

namespace HelseId.SampleAPI;

public  class Startup
{
    public const string TokenAuthenticationSchemeForAuthCode = "token_authentication_scheme_for_auth_code";
    public const string TokenAuthenticationSchemeForClientCredentials = "token_authentication_scheme_for_client_credentials";
    public const string TokenAuthenticationSchemeForResourceIndicator1 = "token_authentication_scheme_for_resource_indicator_1";
    public const string TokenAuthenticationSchemeForResourceIndicator2 = "token_authentication_scheme_for_resource_indicator_2";

    public const string AuthCodePolicy = "auth_code_policy";
    public const string ClientCredentialsPolicy = "client_credentials_policy";
    public const string AuthCodePolicyForResourceIndicator1 = "auth_code_policy_for_resource_indicator_1";
    public const string AuthCodePolicyForResourceIndicator2 = "auth_code_policy_for_resource_indicator_2";

    public WebApplication BuildWebApplication()
    {
        var webApplicationBuilder = WebApplication.CreateBuilder();

        CreateConfiguration(webApplicationBuilder);

        AddServices(webApplicationBuilder);

        SetUpKestrel(webApplicationBuilder);

        var webApplication = webApplicationBuilder.Build();

        ConfigureServices(webApplication);

        return webApplication;
    }

    private void CreateConfiguration(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var contentRootPath = webApplicationBuilder.Environment.ContentRootPath;
        var sharedConfigurationPath = Path.Combine(contentRootPath, "..", "Configuration");

    //    builder.Configuration.AddJsonFile(Path.Combine(sharedConfigurationPath, _settings.AppsettingsFile), optional: false, reloadOnChange: false);
        webApplicationBuilder.Configuration.AddJsonFile(Path.Combine(sharedConfigurationPath, "appsettings.General.json"), optional: false, reloadOnChange: false);
    }

    private void AddServices(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.Services.AddSingleton<IApiResponseCreator, ApiResponseCreator>();
        webApplicationBuilder.Services.AddMvc(option => option.EnableEndpointRouting = false);
        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();

        var stsOptions = webApplicationBuilder.Configuration.GetOptions<StsOptions>();

        // Adds the authentication scheme to be used for bearer tokens (auth code)
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationSchemeForAuthCode)
            .AddJwtBearer(TokenAuthenticationSchemeForAuthCode, options =>
            {
                options.Authority = stsOptions.StsUrl;
                options.Audience = ApiOptions.StandardApiAudience;

                // Validation parameters are in agreement with HelseIDs requirements:
                // https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs
                // These (and a few others) are all true by default
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RequireSignedTokens = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.RequireAudience = true;
            });

        // Adds the authentication scheme to be used for bearer tokens (client credentials)
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationSchemeForClientCredentials)
            .AddJwtBearer(TokenAuthenticationSchemeForClientCredentials, options =>
            {
                options.Authority = stsOptions.StsUrl;
                options.Audience = ApiOptions.StandardApiAudience;
            });

        // Adds the authentication scheme to be used for bearer tokens (resource indicator 1)
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationSchemeForResourceIndicator1)
            .AddJwtBearer(TokenAuthenticationSchemeForResourceIndicator1, options =>
            {
                options.Authority = stsOptions.StsUrl;
                options.Audience = ApiOptions.ResourceIndicators1Audience;
            });

        // Adds the authentication scheme to be used for bearer tokens (resource indicator 2)
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationSchemeForResourceIndicator2)
            .AddJwtBearer(TokenAuthenticationSchemeForResourceIndicator2, options =>
            {
                options.Authority = stsOptions.StsUrl;
                options.Audience = ApiOptions.ResourceIndicators2Audience;
            });

        webApplicationBuilder.Services.AddAuthorization(options =>
        {   
            // Add a policy for verifying scopes and claims for a logged on user
            options.AddPolicy(
                AuthCodePolicy,
                policy => policy.RequireClaim("scope", ApiOptions.AuthorizationCodeScope)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));

            options.AddPolicy(
                AuthCodePolicyForResourceIndicator1,
                policy => policy.RequireClaim("scope", ApiOptions.ResourceIndicators1Scope)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));

            options.AddPolicy(
                AuthCodePolicyForResourceIndicator2,
                policy => policy.RequireClaim("scope", ApiOptions.ResourceIndicators2Scope)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));

            // Add a policy for verifying scopes for client credentials
            options.AddPolicy(
                ClientCredentialsPolicy, 
                policy => policy.RequireClaim("scope", ApiOptions.ClientCredentialsScope));
        });        
    }

    private void SetUpKestrel(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.WebHost.UseKestrel(kestrelServerOptions =>
        {
            // Bind directly to a socket handle or Unix socket
            kestrelServerOptions.ListenLocalhost(ApiOptions.ApiPort, listenOptions => listenOptions.UseHttps());
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