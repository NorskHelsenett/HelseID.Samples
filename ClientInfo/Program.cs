using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using HelseId.Samples.ClientCredentials;
using HelseID.Samples.Configuration;
using IdentityModel.Client;
using HelseID.Samples.Extensions;

namespace HelseId.Samples.ClientInfo;

/// <summary>
/// This sample expands upon the client credentials grant example in order to demonstrate how to
/// access the client info endpoint on the HelseID server
/// </summary>
static class Program
{
    static async Task Main() 
    {
        using var httpClient = new HttpClient();
        var clientInfoRetriever = new ClientInfoRetriever(
            HelseIdSamplesConfiguration.ConfigurationForClientInfoClient, httpClient);

        await clientInfoRetriever.GetAndDisplayClientInfoFromHelseId();
    }
}

/// <summary>
/// See the ConsumeClientinfoEndpoint method for the example of how to access the client info endpoint
/// </summary>
public class ClientInfoRetriever 
{
    private readonly HelseIdSamplesConfiguration _configuration;

    private readonly HttpClient _httpClient;

    public ClientInfoRetriever(HelseIdSamplesConfiguration configuration, HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task GetAndDisplayClientInfoFromHelseId()
    {
        var accessToken = await GetAccessTokenFromHelseId();
       
        await ConsumeClientinfoEndpoint(accessToken);
    }

    private async Task<string> GetAccessTokenFromHelseId()
    {
        // This method uses the general client credential grant in order to get an access token.
        // Two builder classes are used (see the ClientCredentials directory for details):
        //   * ClientCredentialsClientAssertionsBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        //   * ClientCredentialsClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        var clientCredentialsTokenRequestBuilder =
            new ClientCredentialsTokenRequestBuilder(
                new ClientCredentialsClientAssertionsBuilder(_configuration),
                _configuration);

        var request = await clientCredentialsTokenRequestBuilder.CreateClientCredentialsTokenRequest(_httpClient);

        var result = await _httpClient.RequestClientCredentialsTokenAsync(request);
        if (result.IsError)
        {
            await result.WriteErrorToConsole();
            throw new Exception("Error when trying to get the access token");
        }
        return result.AccessToken;
    }

    /// <summary>
    /// This method gets the URL for the clientinfo endpoint from the discovery document on
    /// the HelseID server. Then, it uses the returned access token to access the
    /// endpoint. The resulting information is printed out to the console. 
    /// </summary>
    private async Task ConsumeClientinfoEndpoint(string accessToken) {

        var clientInfoUrl = await GetClientInfoEndpointUrl();

        var clientInfo = await GetClientInfoFromHelseId(accessToken, clientInfoUrl);

        DisplayClientInfo(clientInfo);
    }

    private async Task<string> GetClientInfoEndpointUrl()
    {
        var clientInfoEndpointName = "clientinfo_endpoint";
        var disco = await _httpClient.GetDiscoveryDocumentAsync(_configuration.StsUrl);
        var clientInfoUrl = disco.TryGet(clientInfoEndpointName);
        if (string.IsNullOrEmpty(clientInfoUrl))
        {
            throw new Exception($"Unable to get URL of clientInfo. Looking for '{clientInfoEndpointName}'");
        }
        return clientInfoUrl;
    }

    private async Task<string> GetClientInfoFromHelseId(string accessToken, string clientInfoUrl) 
    {
        _httpClient.SetBearerToken(accessToken);
        return await _httpClient.GetStringAsync(clientInfoUrl);
    }

    private void DisplayClientInfo(string clientInfo)
    {
        Console.WriteLine("Client info from HelseID:");
        Console.WriteLine(clientInfo);
    }
}



