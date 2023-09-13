using System.Text;
using System.Text.Json;
using HelseId.Samples.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.SampleAPI.Controllers;

public class TokenResponse
{
  public bool IsError { get; set; }
  public SuccessResponse SuccessResponse { get; set; } = new();
  public ErrorResponse ErrorResponse { get; set; } = new();
}

public class ErrorResponse
{
  public string ErrorMessage { get; set; } = string.Empty;
}

public class SuccessResponse
{
  public string AccessTokenJwt { get; set; } = string.Empty;

  public string? DPoPProof { get; set; } = string.Empty;
}


[ApiController]
public class TestAccessTokenController : ControllerBase
{
    
    private const string ApiKey = "6e46177a-5525-4655-b24a-e1ce502c49cd";
    private const string Audience = "foo:bar:car";

    [AllowAnonymous]
    [HttpGet]
    [Route("test-token")]
    public async Task<ActionResult<ApiResponse>> TestToken()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Auth-Key", ApiKey);
        httpClient.DefaultRequestHeaders.Add("Audience", Audience);
        
        var body = """{"GeneralClaimsParametersGeneration":1,"GeneralClaimsParameters":{"Scope":["foo:bar:car/dilldall"],"ClientId":"my-client-id","OrgnrParent":"321456987","AuthenticationMethodsReferences":[],"ClientAuthenticationMethodsReferences":"","ClientName":"my-test-client"} }""";
        var httpResponse = await httpClient
            .PostAsync("https://helseid-int-ttt.test.nhn.no/create-test-token-with-key", 
                new StringContent(body, Encoding.UTF8, "application/json"));

        TokenResponse? result = null;
        try
        {
          result = await httpResponse.Content.ReadFromJsonAsync<TokenResponse>();
        }
        catch
        {
          return new StatusCodeResult(500);
        }

        if (!httpResponse.IsSuccessStatusCode && result != null)
        {
          Console.WriteLine(httpResponse.ReasonPhrase);
          return new StatusCodeResult(500);
        }

        return new JsonResult(result!.SuccessResponse.AccessTokenJwt);
    }    
}