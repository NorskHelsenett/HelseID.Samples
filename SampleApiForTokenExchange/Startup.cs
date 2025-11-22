using System.Security.Claims;
using Duende.IdentityModel;
using HelseId.Samples.Common.ApiConsumers;
using HelseId.Samples.Common.ApiDPoPValidation;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseId.SampleApiForTokenExchange;

public class Startup
{
    private readonly Settings _settings;
    private const int ApiPort = ConfigurationValues.SampleApiForTokenExchangePort;
    public const string TokenAuthenticationSchemeWithDPoP = "token_authentication_scheme_with_dpop";
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
        webApplicationBuilder.Services.AddSingleton<HelseIdConfiguration>(configuration);
        webApplicationBuilder.Services.AddSingleton<IDateTimeService, DateTimeService>();
        webApplicationBuilder.Services.AddSingleton<IDiscoveryDocumentGetter>(new DiscoveryDocumentGetter(ConfigurationValues.IssuerUri));
        webApplicationBuilder.Services.AddSingleton<IHelseIdEndpointsDiscoverer, HelseIdEndpointsDiscoverer>();
        webApplicationBuilder.Services.AddSingleton<IPayloadClaimsCreatorForClientAssertion, ClientAssertionPayloadClaimsCreator>();
        webApplicationBuilder.Services.AddSingleton<IJwtClaimsCreator, JwtClaimsCreator>();
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

        webApplicationBuilder.Services.AddSingleton<IReplayCache, SimpleReplayCache>();
        webApplicationBuilder.Services.AddSingleton<DPoPProofValidator>();

        // Adds the authentication scheme to be used for bearer tokens
        webApplicationBuilder.Services.AddAuthentication(TokenAuthenticationSchemeWithDPoP)
            .AddJwtBearer(TokenAuthenticationSchemeWithDPoP, options =>
            {
                // "Authority" is the address for the HelseID server
                options.Authority = _settings.Authority;
                // "Audience": the name for this API (as set in HelseID Selvbetjening)
                options.Audience = _settings.Audience;
                // The following parameters (and a few others) are all true by default, but set to true here for instructive purposes:
                options.RequireHttpsMetadata = true;

                // Validation parameters are in agreement with HelseIDs requirements:
                // https://utviklerportal.nhn.no/informasjonstjenester/helseid/protokoller-og-sikkerhetsprofil/sikkerhetsprofil/docs/vedlegg/validering_av_access_token_enmd/
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.RequireSignedTokens = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.RequireAudience = true;

                options.Events ??= new JwtBearerEvents();

                options.Events.OnMessageReceived = context =>
                {
                    // Per HelseID's security profile, an API endpoint can accept *either*
                    // a DPoP access token *or* a Bearer access token, but not both.

                    // This ensures that the received access token is a DPoP token:
                    if (context.Request.GetDPoPAccessToken(out var dPopToken))
                    {
                        context.Token = dPopToken;
                    }
                    else
                    {
                        // Do not accept a bearer token:
                        context.Fail("Expected a valid DPoP token");
                    }
                    return Task.CompletedTask;
                };

                options.Events.OnTokenValidated = async tokenValidatedContext =>
                {
                    try
                    {
                        // This functionality validates the DPoP proof
                        // https://www.ietf.org/archive/id/draft-ietf-oauth-dpop-16.html#name-checking-dpop-proofs

                        if (!GetTokenAndDpopProofFromRequest(tokenValidatedContext, out var data))
                        {
                            return;
                        }

                        var dPopProofValidator = tokenValidatedContext.HttpContext.RequestServices.GetRequiredService<DPoPProofValidator>();
                        var validationResult = await dPopProofValidator.Validate(data);
                        if (validationResult.IsError)
                        {
                            tokenValidatedContext.Fail(validationResult.ErrorDescription!);
                        }
                    }
                    catch (Exception)
                    {
                        tokenValidatedContext.Fail("Invalid token!");
                    }
                };
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

    private static bool GetTokenAndDpopProofFromRequest(
        TokenValidatedContext tokenValidatedContext,
        out DPoPProofValidationData data)
    {
        // TODO: invalidate a DPoP proof that is too long
        // Get the DPoP proof:
        var request = tokenValidatedContext.HttpContext.Request;
        if (!request.GetDPoPProof(out var dPopProof))
        {
            tokenValidatedContext.Fail("Missing DPoP proof");
            data = null!;
            return false;
        }

        // Get the access token:
        request.GetDPoPAccessToken(out var accessToken);

        // Get the cnf claim from the access token:
        var cnfClaimValue = tokenValidatedContext.Principal!.FindFirstValue(JwtClaimTypes.Confirmation);

        data = new DPoPProofValidationData(request, dPopProof!, accessToken!, cnfClaimValue);
        return true;
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
