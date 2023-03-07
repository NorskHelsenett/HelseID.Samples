using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.Stores;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Samples.ApiAccess;

public class OpenIdConnectOptionsInitializer : IConfigureNamedOptions<OpenIdConnectOptions>
{
    private readonly IClientAssertionsBuilder _clientAssertionsBuilder;
    private readonly ISigningJwtTokenCreator _signingJwtTokenCreator;
    private readonly IUserSessionDataStore _userSessionDataStore;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreatorForClientAssertion;
    private readonly IPayloadClaimsCreatorForRequestObjects _payloadClaimsCreatorForRequestObjects;
    private readonly Settings _settings;
    private readonly IExpirationTimeCalculator _expirationTimeCalculator;

    public OpenIdConnectOptionsInitializer(
        IClientAssertionsBuilder clientAssertionsBuilder,
        ISigningJwtTokenCreator signingJwtTokenCreator,
        IUserSessionDataStore userSessionDataStore,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        IPayloadClaimsCreatorForRequestObjects payloadClaimsCreatorForRequestObjects,
        Settings settings,
        IExpirationTimeCalculator expirationTimeCalculator)
    {
        _clientAssertionsBuilder = clientAssertionsBuilder;
        _signingJwtTokenCreator = signingJwtTokenCreator;
        _userSessionDataStore = userSessionDataStore;
        _payloadClaimsCreatorForClientAssertion = payloadClaimsCreatorForClientAssertion;
        _payloadClaimsCreatorForRequestObjects = payloadClaimsCreatorForRequestObjects;
        _settings = settings;
        _expirationTimeCalculator = expirationTimeCalculator;
    }

    // This gets called from the Startup class
    public void Configure(string? name, OpenIdConnectOptions openIdConnectOptions)
    {
        OpenIdConnectOptionsManagement(openIdConnectOptions);
        OpenIdConnectEventManagement(openIdConnectOptions);
    }

    public void Configure(OpenIdConnectOptions openIdConnectOptions)
    {
        Configure(null, openIdConnectOptions);
    }

    private void OpenIdConnectOptionsManagement(OpenIdConnectOptions openIdConnectOptions)
    {
        // Application-specific configuration for the OpenID Connect handler
        // (we read data from ../Configuration/HelseIdSamplesConfiguration.cs)
        openIdConnectOptions.Authority = _settings.HelseIdConfiguration.StsUrl;
        openIdConnectOptions.ClientId = _settings.HelseIdConfiguration.ClientId;
        SetUpScopes(openIdConnectOptions);

        // Required settings:
        openIdConnectOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        openIdConnectOptions.ResponseType = OpenIdConnectResponseType.Code;

        // These defaults to false: tokens should preferably not be saved in the browser:
        openIdConnectOptions.SaveTokens = false;

        // Uses information from the user info endpoint
        // Comment these lines out if you don't need information about HPR authorization:
        openIdConnectOptions.GetClaimsFromUserInfoEndpoint = true;
        openIdConnectOptions.ClaimActions.MapCustomJson("helseid://claims/hpr/hpr_details",jsonElement =>
        {
            return jsonElement.TryGetProperty("helseid://claims/hpr/hpr_details", out var claimContent) ?
                claimContent.GetRawText() :
                string.Empty;
        });

        // These default to true, set here for instructional purposes:
        openIdConnectOptions.RequireHttpsMetadata = true;
        openIdConnectOptions.UsePkce = true;

        // This matches the value set on the HelseID clients:
        openIdConnectOptions.SignedOutCallbackPath = "/signout-callback-oidc";

        // We use POST as the authentication method
        openIdConnectOptions.AuthenticationMethod = OpenIdConnectRedirectBehavior.FormPost;
    }

    
    private void SetUpScopes(OpenIdConnectOptions openIdConnectOptions)
    {
        openIdConnectOptions.Scope.Clear();
        foreach (var scope in _settings.HelseIdConfiguration.Scope.Split(' '))
        {
            openIdConnectOptions.Scope.Add(scope);
        }
    }

    // This method sets up the receiving of events from the OpenID Connect library
    private void OpenIdConnectEventManagement(OpenIdConnectOptions openIdConnectOptions)
    {
        // See the class Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents for
        // more events that might be useful in a production scenario

        // -----------------------------------------------------------------------
        // Authorization code has been received
        // -----------------------------------------------------------------------
        openIdConnectOptions.Events.OnAuthorizationCodeReceived = authCodeReceivedContext =>
        {
            // Invoked after security token validation if an authorization code is present in the protocol message.

            // Gets parameters for the payload that is used in the client assertion;
            // for instance the underenhet org number if needed.
            // This will typically be aligned with the logged on user:
            var payloadClaimParameters = CreatePayloadClaimParameters();

            // We have received the authorization code (the user has logged on), and we need to
            // authorize ourselves for HelseId. This requires a client assertion, which we must build.
            // The ClientAssertion type comes from the IdentityModel library.
            var clientAssertion = _clientAssertionsBuilder.BuildClientAssertion(
                _payloadClaimsCreatorForClientAssertion, payloadClaimParameters);
            // The client assertion type is hardcoded as a Jwt bearer type
            authCodeReceivedContext.TokenEndpointRequest!.ClientAssertionType = clientAssertion.Type;
            // Asserts the client by using the generated Jwt (the value from the type)
            authCodeReceivedContext.TokenEndpointRequest.ClientAssertion = clientAssertion.Value;

            return Task.CompletedTask;
        };

        // -----------------------------------------------------------------------
        // Returned token are validated
        // -----------------------------------------------------------------------
        openIdConnectOptions.Events.OnTokenValidated = async tokenValidatedContext =>
        {
            // Invoked when an IdToken has been validated and produced an AuthenticationTicket.
            // Note there are additional checks after this event that validate other aspects of
            // the authentication flow like the nonce.
            // We extract the tokens from the response, calculate expiration time, and put them in the token store
            var sessionId = GetSessionIdFromPrincipal(tokenValidatedContext);
            var openIdConnectMessage = GetOpenIdConnectMessage(tokenValidatedContext);

            await UpsertUserSessionData(sessionId, openIdConnectMessage);
        };

        // -----------------------------------------------------------------------
        // Sign out
        // -----------------------------------------------------------------------
        openIdConnectOptions.Events.OnRedirectToIdentityProviderForSignOut = async redirectContext =>
        {
            // Invoked before redirecting to the identity provider to sign out.
            // Set the ID token on the redirect context in order to get redirected back after logout:
            var claims = redirectContext.HttpContext.User.Claims;
            var sessionId = claims.Single(c => c.Type == JwtClaimTypes.SessionId).Value;
            try
            {
                var userSessionData = await _userSessionDataStore.GetUserUserSessionData(sessionId);
                await _userSessionDataStore.ClearUserSession(sessionId);
                redirectContext.ProtocolMessage.IdTokenHint = userSessionData.IdToken;
            }
            catch (SessionIdDoesNotExistException) { }
        };

        // -----------------------------------------------------------------------
        // Redirecting to the authentication endpoint
        // -----------------------------------------------------------------------
        openIdConnectOptions.Events.OnRedirectToIdentityProvider = redirectContext =>
        {
            // Invoked before redirecting to the identity provider to authenticate. This can be used to
            // set a ProtocolMessage.State that will be persisted through the authentication process.
            // The ProtocolMessage can also be used to add or customize parameters sent to the identity provider.
            var customOpenIdConnectMessageParameters =
                new CustomOpenIdConnectMessageParameters
                {
                    RequestObject = CreateRequestObject(),
                    ResourceIndicators = _settings.HelseIdConfiguration.ResourceIndicators,
                }; 
            
            // For certain features, we need to establish a custom request message for creating
            // request objects or resource indicators.  The implementation of the former ('resource')
            // is not in conformance with the specification (https://www.rfc-editor.org/rfc/rfc8707), and
            // the (optional) 'request' parameter (https://openid.net/specs/openid-connect-core-1_0.html#JWTRequests)
            // is not currently implemented
            if (redirectContext.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
            {
                // We need a custom class for creating the message to the authorization endpoint:
                var customProtocolMessage = new CustomOpenIdConnectMessage(
                    customOpenIdConnectMessageParameters,
                    redirectContext.ProtocolMessage.Clone());

                redirectContext.ProtocolMessage = customProtocolMessage;

                /*
                The metadata endpoint https://helseid-sts.test.nhn.no/connect/availableidps list the available IDPs
                in the test environment. If you want to redirect the user to a specific IDP, you can specify this value.
                */

                // redirectContext.ProtocolMessage.AcrValues = "idp:idporten-oidc";

                /*
                See https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/5571426/Use+of+ID-porten for more examples:
                If you want to authenticate the user on behalf of a specific organization, you can add this parameter:
                */

                // redirectContext.ProtocolMessage.Parameters.Add("on_behalf_of", "912159523");
            }

            return Task.CompletedTask;
        };

        // -----------------------------------------------------------------------
        // Authentication failed
        // -----------------------------------------------------------------------
        openIdConnectOptions.Events.OnAuthenticationFailed = authenticationFailedContext =>
        {
            // Invoked if exceptions are thrown during request processing.
            // The exceptions will be re-thrown after this event unless suppressed.

            // Here, you might want to inform the user about a failed authentication
            return Task.CompletedTask;
        }; 
    }

    // Created a request object if the client type requires it.
    // In this sample, we use the request object as a mechanism for getting a child organization number
    // in the access token
    private string CreateRequestObject()
    {
        if (_settings.ClientType != ClientType.ApiAccessWithRequestObject)
        {
            return string.Empty;
        }

        // This value is set as a static value from configuration here:
        var payloadClaimParameters = new PayloadClaimParameters
        {
            ChildOrganizationNumber = ConfigurationValues.ApiAccessWithRequestObjectChildOrganizationNumber
        };
        // We create a signing token (as used in a client assertion), and use this as a request object: 
        return _signingJwtTokenCreator.CreateSigningToken(_payloadClaimsCreatorForRequestObjects, payloadClaimParameters);
    }

    private async Task UpsertUserSessionData(
        string sessionId,
        OpenIdConnectMessage openIdConnectMessage)
    {
        var userSessionData = new UserSessionData();

        // The 'rt_expires_in' parameter is specific for HelseID, and is not present in the common library:
        var refreshTokenExpiresAtUtc = GetRefreshTokenExpiration(openIdConnectMessage);

        userSessionData.IdToken = openIdConnectMessage.IdToken;
        userSessionData.RefreshToken = openIdConnectMessage.RefreshToken;
        userSessionData.RefreshTokenExpiresAtUtc = refreshTokenExpiresAtUtc;
        userSessionData.SessionId = sessionId;

        // The resource indicator application is a special case: we get an access token that has more than one
        // audience (i.e. the audience for both resources that were requested). Hence, we don't want to
        // persist this token, but rather use the refresh token to get new access tokens, each with a
        // specific audience:
        if (_settings.ClientType != ClientType.ApiAccessWithResourceIndicators)
        {
            StoreAccessTokenInUserSession(openIdConnectMessage, userSessionData);
        }

        await _userSessionDataStore.UpsertUserSessionData(sessionId, userSessionData);
    }

    private DateTime GetRefreshTokenExpiration(OpenIdConnectMessage openIdConnectMessage)
    {
        if (!openIdConnectMessage.Parameters.ContainsKey(HelseIdConstants.RefreshTokenExpiresIn))
        {
            return DateTime.MinValue;
        }
        return _expirationTimeCalculator.CalculateTokenExpirationTimeUtc(
                int.Parse(openIdConnectMessage.Parameters[HelseIdConstants.RefreshTokenExpiresIn]));
    }

    private void StoreAccessTokenInUserSession(OpenIdConnectMessage openIdConnectMessage, UserSessionData userSessionData)
    {
        // This is the default storing of the access token when we can assume that it only
        // has one 'aud' claim:
        var accessTokenExpiresAtUtc =
            _expirationTimeCalculator.CalculateTokenExpirationTimeUtc(
                int.Parse(openIdConnectMessage.ExpiresIn));

        var accessTokenWithExpiration = new AccessTokenWithExpiration(
            openIdConnectMessage.AccessToken,
            accessTokenExpiresAtUtc);

        userSessionData.AccessTokens.Add(_settings.ApiAudience1, accessTokenWithExpiration);
    }

    private static string GetSessionIdFromPrincipal(TokenValidatedContext tokenValidatedContext)
    {
        if (tokenValidatedContext.Principal == null)
        {
            throw new Exception("Could not find a logged on user.");
        }
        var claims = tokenValidatedContext.Principal.Claims;
        var result = claims.Single(c => c.Type == JwtClaimTypes.SessionId).Value;
        if (result == null)
        {
            throw new Exception("Could not find a session ID for the logged on user.");
        }
        return result;
    }

    private static OpenIdConnectMessage GetOpenIdConnectMessage(TokenValidatedContext tokenValidatedContext)
    {
        var openIdConnectMessage = tokenValidatedContext.TokenEndpointResponse;
        if (openIdConnectMessage == null)
        {
            throw new Exception("The token endpoint response was not found in the OpenID Connect message");
        }
        return openIdConnectMessage;
    }

    PayloadClaimParameters CreatePayloadClaimParameters()
    {
        var result = new PayloadClaimParameters();
        if (_settings.ClientType == ClientType.ApiAccessWithRequestObject)
        {
            // This value will typically be assigned to a logged on user:
            result.ChildOrganizationNumber = ConfigurationValues.ApiAccessWithRequestObjectChildOrganizationNumber;
        }

        if (_settings.ClientType == ClientType.ApiAccessForMultiTenantClient)
        {
            // This instructs the payload claim creator for multi-tenancy to not create an 'authorization_details' claim
            // (it is not validated with the code grant).
            result.IsAuthCodeRequest = true;
        }

        if (_settings.ClientType == ClientType.ApiAccessWithContextualClaims)
        {
            // This sets the contextual claim type for the call to HelseID:
            result.ContextualClaimType = ConfigurationValues.TestContextClaim;
        }

        return result;
    }
}