using System.Security.Claims;
using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using IdentityModel.Client;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

/// <summary>
/// If the lifetime for the current access token has elapsed, this class uses the refresh token in
/// HttpContext to retrieve a new access token from the HelseID server.
/// </summary>
public class AccessTokenUpdater : IAccessTokenUpdater
{
    private readonly ITokenRequestBuilder _tokenRequestBuilder;
    private readonly IUserSessionDataStore _userSessionDataStore;
    private readonly IExpirationTimeCalculator _expirationTimeCalculator;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreatorForClientAssertion;
    private readonly IUserSessionGetter _userSessionGetter;

    public AccessTokenUpdater(
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        IUserSessionDataStore userSessionDataStore,
        IExpirationTimeCalculator expirationTimeCalculator,
        IUserSessionGetter userSessionGetter)
    {
        _tokenRequestBuilder = tokenRequestBuilder;
        _payloadClaimsCreatorForClientAssertion = payloadClaimsCreatorForClientAssertion;
        _userSessionDataStore = userSessionDataStore;
        _expirationTimeCalculator = expirationTimeCalculator;
        _userSessionGetter = userSessionGetter;
    }

    // Gets an access token for the logged on user. Refreshes the access token if it has expired.
    public async Task<string> GetValidAccessToken(
        HttpClient httpClient,
        ClaimsPrincipal loggedOnUser,
        ApiIndicators apiIndicators)
    {
        var userSessionData = await _userSessionGetter.GetUserSessionData(loggedOnUser);

        if (!userSessionData.AccessTokenHasExpired(apiIndicators.ApiAudience, _expirationTimeCalculator))
        {
            // The access token is still fresh, we return it:
            return userSessionData.AccessTokens[apiIndicators.ApiAudience].AccessToken;
        }

        if (userSessionData.RefreshTokenHasExpired(_expirationTimeCalculator))
        {
            // The access token has expired, but we can't refresh it without a valid refresh token
            // The refresh token will have a long expiration time, so we allow ourselves an exception for this case:
            throw new RefreshTokenExpiredException();
        }

        userSessionData = await RefreshAccessTokenAndUpdateStore(httpClient, userSessionData, apiIndicators);

        return userSessionData.AccessTokens[apiIndicators.ApiAudience].AccessToken;
    }

    // When the user selects a new organization, the current Access token(s) are deleted
    public async Task SetOrganizationAndDeleteTokens(ClaimsPrincipal loggedOnUser, Organization organization)
    {
        var userSessionData = await _userSessionGetter.GetUserSessionData(loggedOnUser);

        if (userSessionData.SelectedOrganization.Equals(organization))
        {
            return;
        }

        userSessionData.SelectedOrganization = organization;
        userSessionData.AccessTokens = new AccessTokenDictionary();

        await _userSessionDataStore.UpsertUserSessionData(userSessionData.SessionId, userSessionData);
    }

    private async Task<UserSessionData> RefreshAccessTokenAndUpdateStore(
        HttpClient httpClient,
        UserSessionData userSessionData,
        ApiIndicators apiIndicators)
    {
        var tokenResponse = await GetRefreshTokenResponseFromHelseId(httpClient, userSessionData, apiIndicators);

        if (tokenResponse.IsError)
        {
            throw new TokenResponseErrorException(tokenResponse.Error ?? "No token response error found");
        }
        
        if (tokenResponse.AccessToken == null)
        {
            throw new TokenResponseErrorException("No access token response found");
        }

        if (tokenResponse.IdentityToken == null)
        {
            throw new TokenResponseErrorException("No identity token response found");
        }

        if (tokenResponse.RefreshToken == null)
        {
            throw new TokenResponseErrorException("No refresh token response found");
        }
        
        return await UpdateUserSessionData(userSessionData, apiIndicators, tokenResponse);
    }
    
    private async Task<TokenResponse> GetRefreshTokenResponseFromHelseId(
        HttpClient httpClient,
        UserSessionData userSessionData,
        ApiIndicators apiIndicators)
    {
        var tokenRequestParameters = CreateRefreshTokenRequestParameters(userSessionData, apiIndicators);

        // If the value for refreshToken is null, we expect this method to fail
        var request = await _tokenRequestBuilder.CreateRefreshTokenRequest(_payloadClaimsCreatorForClientAssertion, tokenRequestParameters!);

        // Send request using IdentityModel extension method
        return await httpClient.RequestRefreshTokenAsync(request);
    }

    protected virtual RefreshTokenRequestParameters CreateRefreshTokenRequestParameters(
        UserSessionData userSessionData,
        ApiIndicators apiIndicators)
    {
        return new RefreshTokenRequestParameters(
            userSessionData.RefreshToken,
            apiIndicators.ResourceIndicator);
    }

    private async Task<UserSessionData> UpdateUserSessionData(
        UserSessionData userSessionData,
        ApiIndicators apiIndicators,
        TokenResponse tokenResponse)
    {
        var accessTokenExpiresAtUtc =
            _expirationTimeCalculator.CalculateTokenExpirationTimeUtc(tokenResponse.ExpiresIn);

        var accessTokenWithExpiration = new AccessTokenWithExpiration(
            tokenResponse.AccessToken!,
            accessTokenExpiresAtUtc);

        // The 'rt_expires_in' parameter is specific for HelseID, and is not present in the common library:
        var refreshTokenExpiresAtUtc =
            _expirationTimeCalculator.CalculateTokenExpirationTimeUtc(
                tokenResponse.GetRefreshTokenExpiresInValue());

        userSessionData.IdToken = tokenResponse.IdentityToken!;
        userSessionData.RefreshToken = tokenResponse.RefreshToken!;
        userSessionData.RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc;
        userSessionData.AccessTokens.Add(apiIndicators.ApiAudience, accessTokenWithExpiration);

        await _userSessionDataStore.UpsertUserSessionData(userSessionData.SessionId, userSessionData);

        return userSessionData;
    }
}