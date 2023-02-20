using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.TokenExpiration;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

public static class UserSessionDataExtensions
{
    public static bool AccessTokenHasExpired(this UserSessionData userSessionData, string accessTokenAudience, IExpirationTimeCalculator expirationTimeCalculator)
    {
        if (!userSessionData.AccessTokens.ContainsKey(accessTokenAudience))
        {
            return true;
        }
        return expirationTimeCalculator.ExpirationTimeHasPassed(userSessionData.AccessTokens[accessTokenAudience].AccessTokenExpiresAtUtc);
    }
    
    public static bool RefreshTokenHasExpired(this UserSessionData userSessionData, IExpirationTimeCalculator expirationTimeCalculator)
    {
        return expirationTimeCalculator.ExpirationTimeHasPassed(userSessionData.RefreshTokenExpiresAtUtc);
    }
}