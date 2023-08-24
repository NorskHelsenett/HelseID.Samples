using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.SampleApiForTokenExchange.Controllers;

// Authentication is needed to access the API
[ApiController]
[Authorize(Policy=Startup.TokenExchangePolicy, AuthenticationSchemes = Startup.TokenAuthenticationScheme)]
public class TokenExchangeController : ControllerBase
{
    private readonly ITokenRequestBuilder _tokenRequestBuilder;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreatorForClientAssertion;
    private readonly IApiConsumer _apiConsumer;
    private readonly IDPoPProofCreator _dPoPProofCreator;
    private readonly HelseIdConfiguration _configuration;
    
    public TokenExchangeController(
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        IApiConsumer apiConsumer,
        IDPoPProofCreator dPoPProofCreator,
        HelseIdConfiguration configuration)
    {
        _tokenRequestBuilder = tokenRequestBuilder;
        _payloadClaimsCreatorForClientAssertion = payloadClaimsCreatorForClientAssertion;
        _apiConsumer = apiConsumer;
        _dPoPProofCreator = dPoPProofCreator;
        _configuration = configuration;
    }

    private string ApiUrl => _configuration.UseDPoP
        ? ConfigurationValues.SampleApiUrlForM2MWithDPoP
        : ConfigurationValues.SampleApiUrlForM2M;
    
    [HttpGet]
    [Route(ConfigurationValues.TokenExchangeResource)]
    public async Task<ActionResult<ApiResponse?>> Get()
    {
        return await CallApiWithTokenExchange();
    }
    
    //TODO: Cache actor access token (or at least show how to)
    private async Task<ApiResponse?> CallApiWithTokenExchange()
    {
        using var httpClient = new HttpClient();

        // 1: get the subject access token
        var subjectAccessToken = await HttpContext.GetTokenAsync("access_token");

        // 2: use token exchange to get an actor access token
        var actorAccessToken = await ExchangeTheSubjectTokenForAnActorAccessToken(httpClient, subjectAccessToken!); 

        // 3: consume the API
        return await CallApi(httpClient, actorAccessToken);
    }
    
    private async Task<ApiResponse?> CallApi(HttpClient httpClient, string actorAccessToken)
    {
        try
        {
            Console.WriteLine("Using the (exchanged) access token to call the sample API");
            if (_configuration.UseDPoP)
            {
                return await _apiConsumer.CallApiWithDPoPToken(httpClient, ApiUrl, actorAccessToken);
            }
            else
            {
                return await _apiConsumer.CallApiWithBearerToken(httpClient, ApiUrl, actorAccessToken);
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"The sample API did not accept the request at address '{ApiUrl}'. (Have you started the sample API application (in the 'SampleAPI' folder)?");
            Console.WriteLine($"Error message: '{e.Message}'");
            return new ApiResponse() {Greeting = e.Message};
        }
    }

    private async Task<string> ExchangeTheSubjectTokenForAnActorAccessToken(HttpClient httpClient, string subjectToken)
    {
        string? dPoPNonce = null;
        if (_configuration.UseDPoP)
        {
            dPoPNonce = await GetDPoPNonce(httpClient, subjectToken);
        }
        // The request to HelseID is created:
        var request = await _tokenRequestBuilder.CreateTokenExchangeTokenRequest(
            _payloadClaimsCreatorForClientAssertion,
            new TokenExchangeTokenRequestParameters
            {
                SubjectToken = subjectToken,
            },
            dPoPNonce);

        // We use the HTTP client to retrieve the response from HelseID:
        var tokenResponse = await httpClient.RequestTokenExchangeTokenAsync(request);

        if (tokenResponse.IsError || tokenResponse.AccessToken == null)
        {
            throw new Exception();
        }

        return tokenResponse.AccessToken;
    }

    private async Task<string?> GetDPoPNonce(HttpClient httpClient, string subjectToken)
    {
        var request = await _tokenRequestBuilder.CreateTokenExchangeTokenRequest(
            _payloadClaimsCreatorForClientAssertion,
            new TokenExchangeTokenRequestParameters
            {
                SubjectToken = subjectToken,
            },
            null);

        var tokenResponse = await httpClient.RequestTokenExchangeTokenAsync(request);

        if (!tokenResponse.IsError || string.IsNullOrEmpty(tokenResponse.DPoPNonce))
        {
            throw new Exception("Could not get a valid response from the Authorization server when requesting a DPoP token.");
        }

        return tokenResponse.DPoPNonce;
    }
}
