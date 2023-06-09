using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TestTokenTool.Authentication;
using TestTokenTool.Constants;
using TestTokenTool.RequestModel;
using TestTokenTool.ResponseModel;
using AuthorizationHeaderExtensions = IdentityModel.Client.AuthorizationHeaderExtensions;
using HttpClientTokenRequestExtensions = IdentityModel.Client.HttpClientTokenRequestExtensions;

namespace TestTokenTool.Commands;

internal static class TokenRetriever
{
    public static async Task<TokenResponse?> GetToken(IConfigurationBuilder configurationBuilder, TokenRequest tokenRequest)
    {
        IConfiguration config = configurationBuilder.Build();

        using var httpClient = await CreateHttpClient(config);
        
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

    private static async Task<HttpClient> CreateHttpClient(IConfiguration config)
    {
        var accessToken = await GetAccessTokenForTtt(config);
        var httpClient = new HttpClient();
        AuthorizationHeaderExtensions.SetBearerToken(httpClient, accessToken);
        return httpClient;
    }

    private static async Task<string> GetAccessTokenForTtt(IConfiguration config)
    {
        using var tokenAccessClient = new HttpClient();
        var clientCredentialsGrantRequester = new ClientCredentialsGrantRequester(config);
        var tokenRequest = await clientCredentialsGrantRequester.CreateClientCredentialsTokenRequest();
        var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(tokenAccessClient, tokenRequest);
        if (tokenResponse.AccessToken == null)
        {
            throw new Exception("Could not get an access token from HelseID.");
        }
        return tokenResponse.AccessToken;
    }
}