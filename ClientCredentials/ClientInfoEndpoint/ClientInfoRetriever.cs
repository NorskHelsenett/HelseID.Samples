using System.Text.Json;
using HelseId.Samples.Common.Interfaces;
using HelseId.Samples.Common.Interfaces.Endpoints;
using IdentityModel.Client;

namespace HelseId.Samples.ClientCredentials.ClientInfoEndpoint;

/// <summary>
/// Can be used for debugging purposes
/// </summary>
public class ClientInfoRetriever : IClientInfoRetriever
{
    private readonly IHelseIdEndpointsDiscoverer _helseIdEndpointsDiscoverer;

    public ClientInfoRetriever(IHelseIdEndpointsDiscoverer helseIdEndpointsDiscoverer)
    {
        _helseIdEndpointsDiscoverer = helseIdEndpointsDiscoverer;
    }

    /// <summary>
    /// This method gets the URL for the clientinfo endpoint from the discovery document on
    /// the HelseID server. Then, it uses the returned access token to access the
    /// endpoint. The resulting information is printed out to the console.
    /// </summary>
    public async Task ConsumeClientInfoEndpoint(HttpClient httpClient, string accessToken) {

        var clientInfoUrl = await _helseIdEndpointsDiscoverer.GetClientInfoEndpointFromHelseId();

        var clientInfo = await GetClientInfoFromHelseId(httpClient, accessToken, clientInfoUrl);

        DisplayClientInfo(clientInfo);
    }

    /// <summary>
    /// We call HelseID to get information about our client
    /// </summary>
    private async Task<string> GetClientInfoFromHelseId(HttpClient httpClient, string accessToken, string clientInfoUrl)
    {
        // This extension from the IdentityModel library sets the token in the authorization header
        // value for the request: "Authorization: Bearer {token}"
        httpClient.SetBearerToken(accessToken);
        return await httpClient.GetStringAsync(clientInfoUrl);
    }

    private void DisplayClientInfo(string clientInfo)
    {
        Console.WriteLine("Description of the client from HelseID:");
        Console.WriteLine(FormatJsonText(clientInfo));
        Console.WriteLine("------");
    }

    static string FormatJsonText(string jsonString)
    {
        var memoryStream = new MemoryStream();
        using var document = JsonDocument.Parse(jsonString);
        using (var utf8JsonWriter = new Utf8JsonWriter(
                   memoryStream,
                   new JsonWriterOptions
                    {
                        Indented = true
                    }))
        document.WriteTo(utf8JsonWriter);
        return new System.Text.UTF8Encoding().GetString(memoryStream.ToArray());
    }
}
