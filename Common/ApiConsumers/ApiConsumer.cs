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
    private readonly IDPoPProofCreator _idPoPProofCreator;
    public ApiConsumer(IDPoPProofCreator idPoPProofCreator)
    {
        _idPoPProofCreator = idPoPProofCreator;
    }

    public async Task<ApiResponse?> CallApiWithDPoPToken(HttpClient httpClient, string apiUrl, string accessToken)
    {
        var dPopProof = _idPoPProofCreator.CreateDPoPProof(apiUrl, "GET", accessToken: accessToken);

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
