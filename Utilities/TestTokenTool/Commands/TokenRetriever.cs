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

    /*
    private static async Task<string> GetAccessTokenForTtt(IConfiguration config)
    {
        if (AccessTokenOnFile(config) && GetAccessTokenFromFile(out var savedAccessToken))
        {
            return savedAccessToken;
        }

        var tokenResponse = await GetAccessTokenFromHelseId(config);

        if (ShouldSaveAccessToken(config))
        {
            SaveAccessTokenToFile(tokenResponse);
        }

        // Previously checked for null
        return tokenResponse.AccessToken!;
    }
    */
/*
    private static async Task<IdentityModel.Client.TokenResponse> GetAccessTokenFromHelseId(IConfiguration config)
    {
        using var tokenAccessClient = new HttpClient();
        var clientCredentialsGrantRequester = new ClientCredentialsGrantRequester(config);
        var tokenRequest = await clientCredentialsGrantRequester.CreateClientCredentialsTokenRequest();
        var tokenResponse = await HttpClientTokenRequestExtensions.RequestClientCredentialsTokenAsync(tokenAccessClient, tokenRequest);

        if (tokenResponse.AccessToken == null)
        {
            throw new Exception("Could not get an access token from HelseID.");
        }

        return tokenResponse;
    }
*/
/*
    private static void SaveAccessTokenToFile(IdentityModel.Client.TokenResponse tokenResponse)
    {
        File.WriteAllText(FileConstants.AccessTokenFileName, tokenResponse.AccessToken);
        File.WriteAllText(FileConstants.AccessTokenExpirationFileName,
            DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
    }

    private static bool GetAccessTokenFromFile(out string accessToken)
    {
        accessToken = "";
        try
        {
            var timeFromFileString = File.ReadAllText(FileConstants.AccessTokenExpirationFileName);
            var timeFromFile = DateTime.Parse(timeFromFileString).ToUniversalTime();
            if (DateTime.UtcNow < timeFromFile)
            {
                accessToken = File.ReadAllText(FileConstants.AccessTokenFileName);
                return true;
            }
        } catch (FormatException) {}

        return false;
    }
*/
/*
    private static bool AccessTokenOnFile(IConfiguration config)
    {
        return ShouldSaveAccessToken(config) &&
               File.Exists(FileConstants.AccessTokenFileName) &&
               File.Exists(FileConstants.AccessTokenExpirationFileName);
    }
*/
/*
    private static bool ShouldSaveAccessToken(IConfiguration config)
    {
        return config[ConfigurationConstants.AuthenticationSaveAccessToken] == "true";
    }
    */
}
