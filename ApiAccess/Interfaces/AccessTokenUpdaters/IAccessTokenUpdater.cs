using System.Security.Claims;
using HelseId.Samples.ApiAccess.Models;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;

public interface IAccessTokenUpdater
{
    Task<string> GetValidAccessToken(
        HttpClient httpClient,
        ClaimsPrincipal loggedOnUser,
        ApiIndicators apiIndicators);

    Task SetOrganizationAndDeleteTokens(ClaimsPrincipal loggedOnUser, Organization organization);
}