using System.Security.Claims;
using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

// Gets the stored data about a user from the user session data store by means of the ClaimsPrincipal
// object that is present in the HttpContext.
public class UserSessionGetter : IUserSessionGetter
{
    private readonly IUserSessionDataStore _userSessionDataStore;

    public UserSessionGetter(IUserSessionDataStore userSessionDataStore)
    {
        _userSessionDataStore = userSessionDataStore;
    }

    public async Task<UserSessionData> GetUserSessionData(ClaimsPrincipal loggedOnUser)
    {
        var sidClaim = loggedOnUser.Claims.FirstOrDefault(c => c.Type == "sid");

        if (sidClaim == null)
        {
            // We don't have a logged on user; this should not happen.
            throw new Exception("The logged on user has no session id claim");
        }

        return await _userSessionDataStore.GetUserUserSessionData(sidClaim.Value);
    }
}