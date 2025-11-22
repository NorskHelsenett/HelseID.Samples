using Duende.IdentityModel.Client;
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

        if (parameters.UseDPoP)
        {
            if (tokenResponse.SuccessResponse.DPoPProof == null)
            {
                throw new Exception("No DPoP proof received");
            }

            await CallApiWithDpopProof(tokenResponse, accessToken, httpClient);

            if (parameters.ReuseDPoPProof)
            {
                await CallApiWithDpopProof(tokenResponse, accessToken, httpClient);
            }
        }
        else
        {
            await CallApiWithBearerToken(httpClient, accessToken);
        }
    }

    private static async Task ReadToConsole(HttpResponseMessage httpResponse)
    {
        Console.WriteLine("-----");
        Console.WriteLine("Return from API:");
        Console.WriteLine($"Status code from http response: {httpResponse.StatusCode}");
        Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());
    }

    private static async Task CallApiWithDpopProof(TokenResponse tokenResponse, string accessToken, HttpClient httpClient)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5081/machine-clients/greetings");
        requestMessage.SetDPoPToken(accessToken, tokenResponse.SuccessResponse.DPoPProof!);
        var httpResponse = await httpClient.SendAsync(requestMessage);
        await ReadToConsole(httpResponse);
    }
    
    private static async Task CallApiWithBearerToken(HttpClient httpClient, string accessToken)
    {
        httpClient.SetBearerToken(accessToken);

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5081/machine-clients/greetings-without-dpop");
        var httpResponse = await httpClient.SendAsync(requestMessage);
        await ReadToConsole(httpResponse);
    }
}
