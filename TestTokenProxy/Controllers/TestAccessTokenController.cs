using System.Dynamic;
using System.Text;
using System.Text.Json;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTokenProxy.Models;

namespace TestTokenProxy.Controllers;

[ApiController]
public class TestAccessTokenController : ControllerBase
{
    private const string ApiKeyConfig = "ApiKey";
    private const string AudienceConfig = "Audience";
    private const string TestTokenServiceEndpointConfig = "TestTokenServiceEndpoint";

    private enum TokenCreationParameter
    {
        None = 0,
        CreateTokenForClientCredentials = 1,
        CreateTokenWithUser = 2,
    }

    private readonly IConfiguration _configuration;
    public TestAccessTokenController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route(ConfigurationValues.TestTokenProxyResource)]
    public async Task<ActionResult<TestTokenResult>> TestToken([FromBody] TestTokenParameters testTokenParameters)
    {
        // This method calls the HelseID Test Token Service (TTT) and returns a token per the input parameters
        var tokenCreationParameter = GetTokenCreationParameter(testTokenParameters);

        if (tokenCreationParameter == TokenCreationParameter.None)
        {
            return new BadRequestResult();
        }

        var body = CreateBodyForUseAgainstTestTokenService(tokenCreationParameter, testTokenParameters.Uri);
        using var httpClient = CreateHttpClient();
        var httpResponse = await httpClient.PostAsync(
                _configuration[TestTokenServiceEndpointConfig],
                new StringContent(body, Encoding.UTF8, "application/json"));

        TokenResponse? tokenResponse;
        try
        {
            tokenResponse = await httpResponse.Content.ReadFromJsonAsync<TokenResponse>();
        }
        catch
        {
            return new StatusCodeResult(500);
        }

        if (!httpResponse.IsSuccessStatusCode && tokenResponse != null)
        {
            Console.WriteLine(httpResponse.ReasonPhrase);
            return new StatusCodeResult(500);
        }

        var result = new TestTokenResult
        {
            AccessToken = tokenResponse!.SuccessResponse.AccessTokenJwt,
            DPoPProof = tokenResponse!.SuccessResponse.DPoPProof!,
        };

        return new JsonResult(result);
    }

    private static TokenCreationParameter GetTokenCreationParameter(TestTokenParameters testTokenParameters)
    {
        // This returns a parameter for creating a token, based upon what controller method the URI is set against
        var controllerName = new Uri(testTokenParameters.Uri);
        switch (controllerName.AbsolutePath)
        {
            case "/" + ConfigurationValues.ResourceIndicatorsResource1:
            case "/" + ConfigurationValues.ResourceIndicatorsResource2:
            case "/" + ConfigurationValues.AuthCodeClientResource:
                return TokenCreationParameter.CreateTokenWithUser;
            case "/" + ConfigurationValues.SampleApiMachineClientResource:
                return TokenCreationParameter.CreateTokenForClientCredentials;
            default:
                return TokenCreationParameter.None;
        }
    }

    private static dynamic CreateBodyForUseAgainstTestTokenService(TokenCreationParameter parameter, string uri)
    {
        dynamic bodyObject = new ExpandoObject();
        bodyObject.generalClaimsParametersGeneration = 2; // 2: GenerateOnlyFromNonEmptyParameterValues
        // This sets up the DPoP proof:
        bodyObject.createDPoPTokenWithDPoPProof = true;
        bodyObject.dPoPProofParameters = new
        {
            htuClaimValue = uri,
            htmClaimValue = "GET",
        };

        switch (parameter)
        {
            case TokenCreationParameter.CreateTokenForClientCredentials:
                // Client Credentials token:
                bodyObject.generalClaimsParameters = new
                {
                    scope = new List<string> {ConfigurationValues.ClientCredentialsScopeForSampleApi},
                };
                break;
            case TokenCreationParameter.CreateTokenWithUser:
                // Authorization Code token:
                bodyObject.userClaimsParametersGeneration = 1; // 1: GenerateOnlyDefault
                bodyObject.generalClaimsParameters = new
                {
                    scope = new List<string> {ConfigurationValues.AuthorizationCodeScopeForSampleApi},
                };
                break;
        }

        return JsonSerializer.Serialize(bodyObject);
    }

    private HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Auth-Key", _configuration[ApiKeyConfig]);
        httpClient.DefaultRequestHeaders.Add("Audience", _configuration[AudienceConfig]);
        return httpClient;
    }
}


