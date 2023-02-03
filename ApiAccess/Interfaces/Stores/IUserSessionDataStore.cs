using HelseId.Samples.ApiAccess.Models;

namespace HelseId.Samples.ApiAccess.Interfaces.Stores;

public interface IUserSessionDataStore
{
    Task UpsertUserSessionData(string sessionIdValue, UserSessionData userSessionData);
    
    Task<UserSessionData> GetUserUserSessionData(string sessionIdValue);

    Task ClearUserSession(string sessionIdValue);
}