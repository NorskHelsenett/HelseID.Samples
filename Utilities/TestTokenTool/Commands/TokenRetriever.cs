using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TestTokenTool.Constants;
using TestTokenTool.RequestModel;
using TestTokenTool.ResponseModel;

namespace TestTokenTool.Commands;

internal static class TokenRetriever
{
    public static async Task<TokenResponse?> GetToken(IConfigurationBuilder configurationBuilder, TokenRequest tokenRequest)
    {
        IConfiguration config = configurationBuilder.Build();

        using var httpClient = CreateHttpClient(config);

        var body = JsonSerializer.Serialize(tokenRequest);

        var httpResponse = await httpClient
            .PostAsync(
                config[ConfigurationConstants.TttServiceEndpoint],
                new StringContent(body, Encoding.UTF8, "application/json"));

        TokenResponse? result = null;
        try
        {
            result = await httpResponse.Content.ReadFromJsonAsync<TokenResponse>();
        }
        catch
        {
            // Some responses may come without a token response
        }

        if (!httpResponse.IsSuccessStatusCode)
        {
            Console.WriteLine(httpResponse.ReasonPhrase);
        }

        return result;
    }

    private static HttpClient CreateHttpClient(IConfiguration config)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("X-Auth-Key", config[ConfigurationConstants.ApiKey]);
        return httpClient;
    }
}
