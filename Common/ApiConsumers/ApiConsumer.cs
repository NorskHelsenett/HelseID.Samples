using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;

namespace HelseId.Samples.Common.ApiConsumers;

/// <summary>
/// This class consumes the Sample API and returns the response to the command line.
/// </summary>
public class ApiConsumer : IApiConsumer
{
    public async Task<ApiResponse?> CallApi(HttpClient httpClient, string apiUrl, string accessToken)
    {
        // This extension from the IdentityModel library sets the token in the authorization header
        // value for the request: "Authorization: Bearer {token}"
        httpClient.SetBearerToken(accessToken);

        var result = await httpClient.GetStringAsync(apiUrl);

        return JsonSerializer.Deserialize<ApiResponse>(
            result,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
    }
}