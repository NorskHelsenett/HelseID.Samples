using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.ApiConsumers;

public interface IApiConsumer
{
    Task<ApiResponse?> CallApiWithBearerToken(HttpClient httpClient, string apiUrl, string accessToken);
    
    Task<ApiResponse?> CallApiWithDPoPToken(HttpClient httpClient, string apiUrl, string accessToken);
}