using IdentityModel.Client;
using Parameters = TestTokenTool.Configuration.Parameters;
using TokenResponse = TestTokenTool.ResponseModel.TokenResponse;

namespace TestTokenTool.ApiCalls;

public class ApiCaller : IApiCaller
{
    public async Task CallApi(TokenResponse? tokenResponse, Parameters parameters)
    {
        if (tokenResponse == null || tokenResponse.IsError || !parameters.CallApi)
        {
            return;
        }
        
        var jwtInput = tokenResponse.SuccessResponse.AccessTokenJwt;
        using var httpClient = new HttpClient();
        AuthorizationHeaderExtensions.SetBearerToken(httpClient, jwtInput);

        var httpResponse = await httpClient.GetStringAsync("https://localhost:5081/machine-clients/greetings");
        Console.WriteLine(httpResponse);
    }
}