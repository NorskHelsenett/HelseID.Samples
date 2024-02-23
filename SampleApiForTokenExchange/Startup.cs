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

public class Startup
{
    private readonly Settings _settings;
    private const int ApiPort = ConfigurationValues.SampleApiForTokenExchangePort;
    public const string TokenAuthenticationScheme = "token_authentication_scheme";
    public const string TokenExchangePolicy = "token_exchange_policy";

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
        webApplicationBuilder.Services.AddSingleton(_settings);
        var configuration = HelseIdSamplesConfiguration.TokenExchangeClient;
        // TODO: REMOVE
        configuration.NoUseOfDPoP = !_settings.UseDPoP;
        webApplicationBuilder.Services.AddSingleton<HelseIdConfiguration>(configuration);
        webApplicationBuilder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        webApplicationBuilder.Services.AddSingleton<IDiscoveryDocumentGetter>(new DiscoveryDocumentGetter(ConfigurationValues.StsUrl));
        webApplicationBuilder.Services.AddSingleton<IHelseIdEndpointsDiscoverer, HelseIdEndpointsDiscoverer>();
        webApplicationBuilder.Services.AddSingleton<IPayloadClaimsCreatorForClientAssertion, ClientAssertionPayloadClaimsCreator>();
        webApplicationBuilder.Services.AddSingleton<IJwtPayloadCreator, JwtPayloadCreator>();
        webApplicationBuilder.Services.AddSingleton<ISigningTokenCreator, SigningTokenCreator>();
        webApplicationBuilder.Services.AddSingleton<IDPoPProofCreator, DPoPProofCreator>();
        // Two builder classes are used for creating the token exchange request to HelseID
        //   * A ClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        webApplicationBuilder.Services.AddSingleton<IClientAssertionsBuilder, ClientAssertionsBuilder>();
        //   * A TokenRequestBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        webApplicationBuilder.Services.AddSingleton<ITokenRequestBuilder, TokenRequestBuilder>();

        // The controller also needs an API consumer:
        webApplicationBuilder.Services.AddSingleton<IApiConsumer, ApiConsumer>();

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
                // These (and a few others) are all set as true by default
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RequireSignedTokens = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.RequireAudience = true;
            });

        webApplicationBuilder.Services.AddAuthorization(options =>
        {
            // Add a policy for verifying scopes and claims for a logged on user for the token exchange endpoint
            options.AddPolicy(
                TokenExchangePolicy,
                policy => policy.RequireClaim("scope", _settings.TokenExchangeApiScope)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));
        });
    }

    private void SetUpKestrel(WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder.WebHost.UseKestrel(kestrelServerOptions =>
        {
            // Bind directly to a socket handle or Unix socket
            kestrelServerOptions.ListenLocalhost(ApiPort, listenOptions => listenOptions.UseHttps());
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
