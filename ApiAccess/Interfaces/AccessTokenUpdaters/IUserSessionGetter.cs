using System.Security.Claims;
using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;

public interface IUserSessionGetter
{
    Task<UserSessionData> GetUserSessionData(ClaimsPrincipal loggedOnUser);
}