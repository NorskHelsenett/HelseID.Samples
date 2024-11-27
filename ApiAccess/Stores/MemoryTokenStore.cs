using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.Stores;

// Obviously, this store only persists tokens while the server is running.
public class MemoryUserSessionDataStore : IUserSessionDataStore
{
    // Slight hack to make the token store available for all instances
    private static readonly Dictionary<string, UserSessionData> UserSessions = new();

    public Task UpsertUserSessionData(string sessionIdValue, UserSessionData userSessionData)
    {
        UserSessions[sessionIdValue] = userSessionData;
        return Task.CompletedTask;
    }

    public Task<UserSessionData> GetUserUserSessionData(string sessionIdValue)
    {
        if (!UserSessions.ContainsKey(sessionIdValue))
        {
            throw new SessionIdDoesNotExistException();
        }
        return Task.FromResult(UserSessions[sessionIdValue]);
    }

    public Task ClearUserSession(string sessionIdValue)
    {
        UserSessions.Remove(sessionIdValue);
        return Task.CompletedTask;
    }
}
