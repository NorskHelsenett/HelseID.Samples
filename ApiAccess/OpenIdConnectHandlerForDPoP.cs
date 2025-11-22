using System.Text.Encodings.Web;
using Duende.IdentityModel;
using Duende.IdentityModel.Client;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Samples.ApiAccess;

public class OpenIdConnectHandlerForDPoP : OpenIdConnectHandler
{
    private readonly IDPoPProofCreator _dPoPProofCreator;
    private readonly ITokenRequestBuilder _tokenRequestBuilder;
    private readonly IHelseIdEndpointsDiscoverer _endpointsDiscoverer;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreator;

    public OpenIdConnectHandlerForDPoP(
        IDPoPProofCreator dPoPProofCreator,
        IHelseIdEndpointsDiscoverer endpointsDiscoverer,
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreator,
        IOptionsMonitor<OpenIdConnectOptions> options,
        ILoggerFactory logger,
        HtmlEncoder htmlEncoder,
        UrlEncoder encoder) : base(options, logger, htmlEncoder, encoder)
    {
        _dPoPProofCreator = dPoPProofCreator;
        _endpointsDiscoverer = endpointsDiscoverer;
        _tokenRequestBuilder = tokenRequestBuilder;
        _payloadClaimsCreator = payloadClaimsCreator;
    }

    protected override async Task<OpenIdConnectMessage> RedeemAuthorizationCodeAsync(OpenIdConnectMessage tokenEndpointRequest)
    {
        // HelseID will require a DPoP nonce from the client. When this nonce is not set,
        // the Resource Server (HelseID) will return a 400 response, with a DPoP nonce in a header.
        // This nonce can then be used in a second call to the Resource Server

        Logger.LogDebug("Creating first call for getting a DPoP token.");

        var authorizationCodeTokenRequestParameters = new AuthorizationCodeTokenRequestParameters(
            tokenEndpointRequest.Parameters["code"],
            tokenEndpointRequest.Parameters["code_verifier"],
            tokenEndpointRequest.Parameters["redirect_uri"],
            new List<string>());

        var authCodeRequest = await _tokenRequestBuilder.CreateAuthorizationCodeTokenRequest(_payloadClaimsCreator, authorizationCodeTokenRequestParameters, null);

        var tokenResponse = await Backchannel.RequestAuthorizationCodeTokenAsync(authCodeRequest);
        if (!tokenResponse.IsError || string.IsNullOrEmpty(tokenResponse.DPoPNonce))
        {
            throw new OpenIdConnectProtocolException("Expected a DPoP nonce to be returned from the authorization server.");
        }

        var tokenEndpoint = await _endpointsDiscoverer.GetTokenEndpointFromHelseId();
        // We got the nonce from HelseID; use it to get a DPoP proof
        var dPoPProof = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", tokenResponse.DPoPNonce);
        // Remove any existing DPoP proof and set the new DPoP proof:
        Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
        Backchannel.DefaultRequestHeaders.Add(OidcConstants.HttpHeaders.DPoP, dPoPProof);

        var result = await base.RedeemAuthorizationCodeAsync(tokenEndpointRequest);
        // Remove the residual DPoP header:
        Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
        return result;
    }
}
