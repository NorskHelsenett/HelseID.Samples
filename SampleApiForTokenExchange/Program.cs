using HelseId.SampleApiForTokenExchange.Stores;
using HelseId.Samples.Common.ApiConsumers;
using HelseId.Samples.Common.ClientAssertions;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Endpoints;
using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.Common.PayloadClaimsCreators;
using HelseId.Samples.Common.TokenExpiration;
using HelseId.Samples.Common.TokenRequests;
using HelseID.Samples.Configuration;

namespace HelseId.SampleApiForTokenExchange;

public static class Program
{
    private const int ApiPort = ConfigurationValues.SampleApiForTokenExchangePort;

    public const string TokenAuthenticationScheme = "token_authentication_scheme";
    public const string TokenExchangePolicy = "token_exchange_policy";

    public static void Main(string[] args)
    {
        Console.Title = "Sample API";
        BuildWebApplication(args).Run();
    }

    private static WebApplication BuildWebApplication(string[] args)
    {
        var webApplicationBuilder = WebApplication.CreateBuilder(args);

        AddFilesToConfiguration(webApplicationBuilder);

        AddServices(webApplicationBuilder);

        SetUpKestrel(webApplicationBuilder);

        var webApplication = webApplicationBuilder.Build();

        ConfigureServices(webApplication);

        return webApplication;
    }
    
    private static void AddFilesToConfiguration(WebApplicationBuilder builder)
    {
        var contentRootPath = builder.Environment.ContentRootPath;
        var sharedConfigurationPath = Path.Combine(contentRootPath, "..", "Configuration");

        builder.Configuration.AddJsonFile(Path.Combine(sharedConfigurationPath, "appsettings.SampleApiForTokenExchangeClient.json"), optional: false, reloadOnChange: false);
        builder.Configuration.AddJsonFile(Path.Combine(sharedConfigurationPath, "appsettings.General.json"), optional: false, reloadOnChange: false);
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        var settings = new Settings();
        builder.Services.AddSingleton(settings);
        builder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        builder.Services.AddSingleton<IDiscoveryDocumentGetter, DiscoveryDocumentGetter>();
        builder.Services.AddSingleton<IHelseIdEndpointsDiscoverer, HelseIdEndpointsDiscoverer>();
        builder.Services.AddSingleton<IPayloadClaimsCreatorForClientAssertion, ClientAssertionPayloadClaimsCreator>();
        builder.Services.AddSingleton<IJwtPayloadCreator, JwtPayloadCreator>();
        builder.Services.AddSingleton<ISigningCredentialsStore, SigningCredentialsStore>();
        builder.Services.AddSingleton<ISigningJwtTokenCreator, SigningJwtTokenCreator>();
        // Two builder classes are used for creating the token exchange request to HelseID
        //   * A ClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        builder.Services.AddSingleton<IClientAssertionsBuilder, ClientAssertionsBuilder>();
        //   * A TokenRequestBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        builder.Services.AddSingleton<ITokenRequestBuilder, TokenRequestBuilder>();

        // The controller also needs an API consumer:
        builder.Services.AddSingleton<IApiConsumer, ApiConsumer>();
        
        builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

       // Adds the authentication scheme to be used for bearer tokens
        builder.Services.AddAuthentication(TokenAuthenticationScheme)
            .AddJwtBearer(TokenAuthenticationScheme, options =>
            {
                var stsOptions = builder.Configuration.GetOptions<StsOptions>();
                var apiOptions = builder.Configuration.GetOptions<ApiOptions>();

                options.RequireHttpsMetadata = true;
                options.Authority = stsOptions.StsUrl;
                options.Audience = apiOptions.Audience;

                // Validation parameters are in agreement with HelseIDs requirements:
                // https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs
                // These (and a few others) are all set as true by default
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RequireSignedTokens = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.RequireAudience = true;
            });

        builder.Services.AddAuthorization(options =>
        {   
            // Add a policy for verifying scopes and claims for a logged on user for the token exchange endpoint
            options.AddPolicy(
                TokenExchangePolicy,
                policy => policy.RequireClaim("scope", settings.TokenExchangeApiScope)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));
        });        
    }

    private static void SetUpKestrel(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.WebHost.UseKestrel(kestrelServerOptions =>
        {
            // Bind directly to a socket handle or Unix socket
            kestrelServerOptions.ListenLocalhost(ApiPort, listenOptions => listenOptions.UseHttps());
        });
    }

    private static void ConfigureServices(WebApplication webApplication)
    {
        // Configure the HTTP request pipeline.
        webApplication.UseHttpsRedirection();
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.MapControllers();
    }
}