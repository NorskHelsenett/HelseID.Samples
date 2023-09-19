using System.Dynamic;
using System.Text;
using System.Text.Json;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TestTokenProxy.Models;

namespace TestTokenProxy.Controllers;

[ApiController]
public class TestAccessTokenController : ControllerBase
{
    private const string ApiKey = "6e46177a-5525-4655-b24a-e1ce502c49cd";
    private const string Audience = "nhn:helseid-public-samplecode";
    private const string TestTokenServiceEndpoint = "https://helseid-ttt.test.nhn.no/create-test-token-with-key";

    private enum TokenCreationParameter
    {
        None = 0,
        CreateTokenForClientCredentials = 1,
        CreateTokenWithUser = 2,
        CreateTokenWithDPoP = 3,
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("test-token")]
    public async Task<ActionResult<TestTokenResult>> TestToken([FromBody] TestTokenParameters testTokenParameters)
    {
        var tokenCreationParameter = GetTokenCreationParameter(testTokenParameters);

        if (tokenCreationParameter == TokenCreationParameter.None)
        {
            return new JsonResult("");
        }

        var body = CreateBody(tokenCreationParameter);
        using var httpClient = CreateHttpClient();
        var httpResponse = await httpClient
            .PostAsync(TestTokenServiceEndpoint,
                new StringContent(body, Encoding.UTF8, "application/json"));

        TokenResponse? tokenResponse = null;
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
/*
        if (createTokenWithDPoP)
        {
            if (tokenResponse!.SuccessResponse.DPoPProof == null)
            {
                Console.WriteLine("Missing DPoP proof");
                return new StatusCodeResult(500);
            }

            result.DPoPProof = tokenResponse!.SuccessResponse.DPoPProof;
        }
*/
        var result = new TestTokenResult
        {
            AccessToken = tokenResponse!.SuccessResponse.AccessTokenJwt
        };

        return new JsonResult(result);
    }

    private static HttpClient CreateHttpClient()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Auth-Key", ApiKey);
        httpClient.DefaultRequestHeaders.Add("Audience", Audience);
        return httpClient;
    }

    private static TokenCreationParameter GetTokenCreationParameter(TestTokenParameters testTokenParameters)
    {
        var controllerName = new Uri(testTokenParameters.Uri);
        switch (controllerName.AbsolutePath)
        {
            case "/" + ConfigurationValues.SampleApiMachineClientResource:
                return TokenCreationParameter.CreateTokenForClientCredentials;
            case "/" + ConfigurationValues.SampleApiMachineClientResourceForDPoP:
                return TokenCreationParameter.CreateTokenWithDPoP;
            case "/" + ConfigurationValues.AuthCodeClientResource:
            case "/" + ConfigurationValues.ResourceIndicatorsResource1:
            case "/" + ConfigurationValues.ResourceIndicatorsResource2:
                return TokenCreationParameter.CreateTokenWithUser;
            default:
                return TokenCreationParameter.None;
        }
    }

    private static dynamic CreateBody(TokenCreationParameter parameter)
    {
        dynamic bodyObject = new ExpandoObject();
        bodyObject.generalClaimsParametersGeneration = 2; // 2: GenerateOnlyFromNonEmptyParameterValues

        switch (parameter)
        {
            case TokenCreationParameter.CreateTokenForClientCredentials:
                bodyObject.generalClaimsParameters = new
                {
                    scope = new List<string> {"nhn:helseid-public-samplecode/client-credentials"},
                };
                break;
            case TokenCreationParameter.CreateTokenWithUser:
                bodyObject.userClaimsParametersGeneration = 1; // 1: GenerateOnlyDefault
                bodyObject.generalClaimsParameters = new
                {
                    scope = new List<string> {"nhn:helseid-public-samplecode/authorization-code"},
                };
                break;
        }

        return JsonSerializer.Serialize(bodyObject);
    }
}


