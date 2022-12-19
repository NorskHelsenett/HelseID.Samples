using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.Stores;

// Obviously, this store only persists tokens while the server is running.
// You might want to use a different approach for a production-like environment.
public class MemoryUserSessionDataStore : IUserSessionDataStore
{
    // Slight hack to make the token store available for all instances
    private static readonly Dictionary<string, UserSessionData> _userSessions = new Dictionary<string, UserSessionData>();

    public Task UpsertUserSessionData(string sessionIdValue, UserSessionData userSessionData)
    {
        _userSessions[sessionIdValue] = userSessionData;
        return Task.CompletedTask;
    }

    public Task<UserSessionData> GetUserUserSessionData(string sessionIdValue)
    {
        if (!_userSessions.ContainsKey(sessionIdValue))
        {
            throw new SessionIdDoesNotExistException();
        }
        return Task.FromResult(_userSessions[sessionIdValue]);
    }

    public Task ClearUserSession(string sessionIdValue)
    {
        _userSessions.Remove(sessionIdValue);
        return Task.CompletedTask;
    }
}