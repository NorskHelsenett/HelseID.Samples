using System.Text.Json;
using HelseId.Library;
using HelseId.Library.Interfaces.JwtTokens;
using HelseId.Library.Models;
using HelseId.Samples.ClientCredentials.Interfaces;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.ClientCredentials.CallsToApi;

/// <summary>
/// This class consumes the Sample API and returns the response to the command line.
/// </summary>
public class ApiConsumer(IDPoPProofCreatorForApiRequests idPoPProofCreatorForApiRequests) : IApiConsumer
{
    public async Task<ApiResponse?> CallApiWithDPoPToken(string apiUrl, AccessTokenResponse accessTokenResponse)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Get, apiUrl);
        var dPopProof = await idPoPProofCreatorForApiRequests.CreateDPoPProofForApiRequest(HttpMethod.Get, apiUrl, accessTokenResponse);
        requestMessage.SetDPoPTokenAndProof(accessTokenResponse, dPopProof);

        try
        {
            using var httpClient = new HttpClient();
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
