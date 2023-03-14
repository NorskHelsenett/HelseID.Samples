using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

/// <summary>
/// This particular instance (which inherits AccessTokenUpdater) sets the contextual claim type(s) for
/// use against HelseID.
/// </summary>
public class AccessTokenUpdaterForContextualClaims : AccessTokenUpdater
{
    public AccessTokenUpdaterForContextualClaims(
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        IUserSessionDataStore userSessionDataStore,
        IExpirationTimeCalculator expirationTimeCalculator,
        IUserSessionGetter userSessionGetter) : base(tokenRequestBuilder, payloadClaimsCreatorForClientAssertion,
        userSessionDataStore, expirationTimeCalculator, userSessionGetter)
    {
    }

    protected override RefreshTokenRequestParameters CreateRefreshTokenRequestParameters(
        UserSessionData userSessionData,
        ApiIndicators apiIndicators)
    {
        var result = base.CreateRefreshTokenRequestParameters(userSessionData, apiIndicators);
        result.PayloadClaimParameters.ContextualClaimType = ConfigurationValues.TestContextClaim;
        return result;
    }
}