using System.Text.Encodings.Web;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Samples.ApiAccess;

public class OpenIdConnectHandlerForDPoP : OpenIdConnectHandler
{
    private readonly ITokenRequestBuilder _tokenRequestBuilder;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreator;

    public OpenIdConnectHandlerForDPoP(
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreator,
        IOptionsMonitor<OpenIdConnectOptions> options,
        ILoggerFactory logger,
        HtmlEncoder htmlEncoder,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, htmlEncoder, encoder, clock)
    {
        _tokenRequestBuilder = tokenRequestBuilder;
        _payloadClaimsCreator = payloadClaimsCreator;
    }

    protected override async Task<OpenIdConnectMessage> RedeemAuthorizationCodeAsync(OpenIdConnectMessage tokenEndpointRequest)
    {
        Logger.LogDebug(new EventId(19, "RedeemingCodeForTokens"), "Redeeming code for tokens.");

        var parameters = tokenEndpointRequest.Parameters;
        var resource = new List<string>();
        var authorizationCodeTokenRequestParameters = new AuthorizationCodeTokenRequestParameters(parameters["code"], parameters["code_verifier"], parameters["redirect_uri"], resource);
        var authCodeRequest = await _tokenRequestBuilder.CreateAuthorizationCodeTokenRequest(_payloadClaimsCreator, authorizationCodeTokenRequestParameters, null);
        
        var tokenResponse = await Backchannel.RequestAuthorizationCodeTokenAsync(authCodeRequest);
        if (tokenResponse.IsError && tokenResponse.Error == "use_dpop_nonce" && !string.IsNullOrEmpty(tokenResponse.DPoPNonce))
        {
            // Expected result when doing DPoP
            authCodeRequest = await _tokenRequestBuilder.CreateAuthorizationCodeTokenRequest(_payloadClaimsCreator, authorizationCodeTokenRequestParameters, tokenResponse.DPoPNonce);
            tokenResponse = await Backchannel.RequestAuthorizationCodeTokenAsync(authCodeRequest);
        }

        if (tokenResponse.IsError)
        {
            throw new OpenIdConnectProtocolException(tokenResponse.Error, tokenResponse.Exception);
        }

        var responseContent = await tokenResponse.HttpResponse.Content.ReadAsStringAsync(Context.RequestAborted);
        return new OpenIdConnectMessage(responseContent);
    }
}