using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

/// <summary>
/// This particular instance (which inherits AccessTokenUpdater) sets the organization number(s)
/// for the 'consumer' organization that has delegated rights to the 'supplier' organization.
/// </summary>
public class AccessTokenUpdaterForMultiTenantRequests : AccessTokenUpdater
{
    public AccessTokenUpdaterForMultiTenantRequests(
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        IUserSessionDataStore userSessionDataStore,
        IExpirationTimeCalculator expirationTimeCalculator,
        IUserSessionGetter userSessionGetter) : base(tokenRequestBuilder, payloadClaimsCreatorForClientAssertion, userSessionDataStore, expirationTimeCalculator, userSessionGetter) { }
    
    protected override RefreshTokenRequestParameters CreateRefreshTokenRequestParameters(
        UserSessionData userSessionData,
        ApiIndicators apiIndicators)
    {
        var result = base.CreateRefreshTokenRequestParameters(userSessionData, apiIndicators);
        result.PayloadClaimParameters = new PayloadClaimParameters
        {
            IsAuthCodeRequest = false,
            ParentOrganizationNumber = userSessionData.SelectedOrganization.OrgNoParent,
            ChildOrganizationNumber = GetChildOrganizationNumber(userSessionData),
        };
        return result;
    }

    private static string GetChildOrganizationNumber(UserSessionData userSessionData)
    {
        var childOrganizationNumber = "";
        if (userSessionData.SelectedOrganization.HasChildOrganization)
        {
            childOrganizationNumber = userSessionData.SelectedOrganization.OrgNoChild;
        }
        return childOrganizationNumber;
    }
}