using System.Security.Claims;
using HelseId.SampleApi.Configuration;
using HelseId.SampleAPI.Controllers;
using HelseId.SampleApi.Interfaces;
using HelseId.Samples.Common.ApiDPoPValidation;
using HelseID.Samples.Configuration;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HelseId.SampleAPI;

public  class Startup
{
    private readonly Settings _settings;

    public const string DPoPTokenAuthenticationScheme = "dpop_token_authentication_scheme";
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
        webApplicationBuilder.Services.AddSwaggerGen();

        // Create singleton from instance
        webApplicationBuilder.Services.AddSingleton(_settings);
        webApplicationBuilder.Services.AddSingleton<IApiResponseCreator, ApiResponseCreator>();

        webApplicationBuilder.Services.AddMvc(option => option.EnableEndpointRouting = false);
        webApplicationBuilder.Services.AddControllers();
        webApplicationBuilder.Services.AddEndpointsApiExplorer();

        webApplicationBuilder.Services.AddSingleton<IReplayCache, DummyReplayCache>();
        webApplicationBuilder.Services.AddSingleton<DPoPProofValidator>();

        webApplicationBuilder.Services
            .AddAuthentication(DPoPTokenAuthenticationScheme)
            // Adds the authentication scheme to be used for DPoP tokens:
            .AddJwtBearer(DPoPTokenAuthenticationScheme, options =>
            {
                options.RequireHttpsMetadata = true;
                options.Authority = _settings.Authority;
                options.Audience = _settings.Audience;

                // Validation parameters are in agreement with HelseIDs requirements:
                // https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/284229708/Guidelines+for+using+JSON+Web+Tokens+JWTs
                // The following parameters (and a few others) are all true by default, but set to true here for instructive purposes:
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

                        // Get the DPoP proof:
                        var request = tokenValidatedContext.HttpContext.Request;
                        if (!request.GetDPoPProof(out var dPopProof))
                        {
                            tokenValidatedContext.Fail("Missing DPoP proof");
                            return;
                        }

                        // Get the access token:
                        request.GetDPoPAccessToken(out var accessToken);

                        // Get the cnf claim from the access token:
                        var cnfClaimValue = tokenValidatedContext.Principal!.FindFirstValue(JwtClaimTypes.Confirmation);

                        var data = new DPoPProofValidationData(request, dPopProof!, accessToken!, cnfClaimValue);

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
            // Add a policy for verifying scopes and claims for a logged on user
            options.AddPolicy(
                AuthCodePolicy,
                policy => policy.RequireClaim("scope", _settings.AuthCodeApiScopeForSampleApi)
                    .RequireClaim("helseid://claims/identity/pid")
                    .RequireClaim("helseid://claims/identity/security_level", "4"));

            // Add a policy for verifying scopes for client credentials
            options.AddPolicy(
                ClientCredentialsPolicy,
                policy =>
                {
                    policy.RequireClaim("scope", _settings.ClientCredentialsApiScopeForSampleApi);
                });
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

    private static void ConfigureServices(WebApplication webApplication)
    {
        // Set up functionality for getting a bearer token (see extend-swagger.js for details):
        webApplication.UseStaticFiles();
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI(options =>
        {
            var testTokenProxyEndpointAddress = ConfigurationValues.TestTokenProxyUrl;
            // This injection of JavaScript code is needed in order to get the access token from the test token service proxy:
            options.UseRequestInterceptor($"(req) => {{ return setDPoPTokenInRequest(req, '{testTokenProxyEndpointAddress}'); }} ");
            options.InjectJavascript("extend-swagger.js");
        });

        // Configure the HTTP request pipeline.
        webApplication.UseHttpsRedirection();
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.MapControllers();
    }
}
