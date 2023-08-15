using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;

namespace HelseId.Samples.Common.ApiConsumers;

/// <summary>
/// This class consumes the Sample API and returns the response to the command line.
/// </summary>
public class ApiConsumer : IApiConsumer
{
    private readonly IDpopProofCreator _dpopProofCreator;
    public ApiConsumer(IDpopProofCreator dpopProofCreator)
    {
        _dpopProofCreator = dpopProofCreator;
    }
    
    public async Task<ApiResponse?> CallApiWithBearerToken(HttpClient httpClient, string apiUrl, string accessToken)
    {
        // This extension from the IdentityModel library sets the token in the authorization header
        // value for the request: "Authorization: Bearer {token}"
        httpClient.SetBearerToken(accessToken);

        return await CallApi(httpClient, apiUrl);
    }

    public async Task<ApiResponse?> CallApiWithDPoPToken(HttpClient httpClient, string apiUrl, string accessToken, string? dPoPNonce)
    {
        var dPopProof = _dpopProofCreator.CreateDpopProof(dPoPNonce, apiUrl, "GET"); 
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        requestMessage.SetDPoPToken(accessToken, dPopProof);
        
        var response = await httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        
        return JsonSerializer.Deserialize<ApiResponse>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
    }

    private static async Task<ApiResponse?> CallApi(HttpClient httpClient, string apiUrl)
    {
        var response = await httpClient.GetStringAsync(apiUrl);

        return JsonSerializer.Deserialize<ApiResponse>(
            response,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
    }
}