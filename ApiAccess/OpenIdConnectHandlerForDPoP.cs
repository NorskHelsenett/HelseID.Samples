using System.Globalization;
using System.Net;
using System.Text.Encodings.Web;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Samples.ApiAccess;

public class OpenIdConnectHandlerForDPoP : OpenIdConnectHandler
{
    private readonly IDPoPProofCreator _dPoPProofCreator;
    private readonly ITokenRequestBuilder _tokenRequestBuilder;
    private IHelseIdEndpointsDiscoverer _endpointsDiscoverer;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreator;
    private OpenIdConnectConfiguration? _configuration;

    public OpenIdConnectHandlerForDPoP(
        IDPoPProofCreator dPoPProofCreator,
        IHelseIdEndpointsDiscoverer endpointsDiscoverer,
        ITokenRequestBuilder tokenRequestBuilder,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreator,
        IOptionsMonitor<OpenIdConnectOptions> options,
        ILoggerFactory logger,
        HtmlEncoder htmlEncoder,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, htmlEncoder, encoder, clock)
    {
        _dPoPProofCreator = dPoPProofCreator;
        _endpointsDiscoverer = endpointsDiscoverer;
        _tokenRequestBuilder = tokenRequestBuilder;
        _payloadClaimsCreator = payloadClaimsCreator;
    }

    protected override async Task<OpenIdConnectMessage> RedeemAuthorizationCodeAsync(OpenIdConnectMessage tokenEndpointRequest)
    {
        Logger.LogDebug("Creating first call for getting a DPoP token.");

        var tokenEndpoint = await _endpointsDiscoverer.GetTokenEndpointFromHelseId();
        
        var dPoPProof = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", null);
        // Remove any residual DPoP header:
        Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
        Backchannel.DefaultRequestHeaders.Add(OidcConstants.HttpHeaders.DPoP, dPoPProof);
        
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
        requestMessage.Content = new FormUrlEncodedContent(tokenEndpointRequest.Parameters);
        requestMessage.Version = Backchannel.DefaultRequestVersion;

        string responseContent;
        var responseMessage = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);
        try
        {
            responseContent = await responseMessage.Content.ReadAsStringAsync(Context.RequestAborted);
        }
        catch (Exception ex)
        {
            throw new OpenIdConnectProtocolException($"Failed to parse token response body as JSON. Status Code: {(int)responseMessage.StatusCode}. Content-Type: {responseMessage.Content.Headers.ContentType}", ex);
        }

        var message = new OpenIdConnectMessage(responseContent);
        if (responseMessage.StatusCode != HttpStatusCode.BadRequest ||
            !responseMessage.Headers.Contains(OidcConstants.HttpHeaders.DPoPNonce) ||
            message.Error != "use_dpop_nonce")
        {
            throw new OpenIdConnectProtocolException("Expected a DPoP nonce to be returned from the authorization server.");
        }

        // We got the nonce from HelseID; use it to get a DPoP token
        responseMessage.Headers.TryGetValues(OidcConstants.HttpHeaders.DPoPNonce, out var nonce);
        dPoPProof = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", nonce?.SingleOrDefault());
        // Replace the DPoP proof:
        Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
        Backchannel.DefaultRequestHeaders.Add(OidcConstants.HttpHeaders.DPoP, dPoPProof);
        
        var result = await base.RedeemAuthorizationCodeAsync(tokenEndpointRequest);
        // Remove the residual DPoP header:
        Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);

        return result;

/*
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
        */
    }
}