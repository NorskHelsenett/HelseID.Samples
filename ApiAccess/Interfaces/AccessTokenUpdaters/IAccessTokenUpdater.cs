using System.Security.Claims;
using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;

public interface IAccessTokenUpdater
{
    Task<string> GetValidAccessToken(
        HttpClient httpClient,
        ClaimsPrincipal loggedOnUser,
        ApiIndicators apiIndicators);
}