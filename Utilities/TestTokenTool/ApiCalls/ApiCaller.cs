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

        var accessToken = tokenResponse.SuccessResponse.AccessTokenJwt;
        using var httpClient = new HttpClient();

        Console.WriteLine("Return from API:");
        if (parameters.UseDPoP)
        {
            if (tokenResponse.SuccessResponse.DPoPProof == null)
            {
                throw new Exception("No DPoP proof received");
            }
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5081/user-login-clients/greetings");
            requestMessage.SetDPoPToken(accessToken, tokenResponse.SuccessResponse.DPoPProof);

            var httpResponse = await httpClient.SendAsync(requestMessage);

            Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
        }
        else
        {
            httpClient.SetBearerToken(accessToken);

            var httpResponse = await httpClient.GetStringAsync("https://localhost:5081/machine-clients/greetings");
            Console.WriteLine(httpResponse);
        }
    }
}
