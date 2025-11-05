using HelseId.Library.Models;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.ClientCredentials.Interfaces;

public interface IApiConsumer
{ 
    Task<ApiResponse?> CallApiWithDPoPToken(string apiUrl, AccessTokenResponse accessToken);
}