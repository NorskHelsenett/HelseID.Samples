using Duende.AccessTokenManagement;
using HelseId.Core.BFF.Sample.Models.Model;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace HelseId.Core.BFF.Sample.Client.Auth;

/// <summary>
/// This handler is a workaround for a bug in Duende.AccessTokenManagement that affects logon.
/// They are working on a fix.
/// </summary>
public class DPoPOpenIdConnectHandler : OpenIdConnectHandler
{
    private readonly IDPoPProofService _dpopProofService;
    private readonly DPoPJwk _dpopJwk;

    public DPoPOpenIdConnectHandler(
        IDPoPProofService dpopProofService,
        DPoPJwk dpopJwk,
        IOptionsMonitor<OpenIdConnectOptions> options,
        ILoggerFactory logger,
        HtmlEncoder htmlEncoder,
        UrlEncoder encoder) : base(options, logger, htmlEncoder, encoder)
    {
        _dpopProofService = dpopProofService;
        _dpopJwk = dpopJwk;
    }

    protected override async Task<OpenIdConnectMessage> RedeemAuthorizationCodeAsync(OpenIdConnectMessage tokenEndpointRequest)
    {
        OpenIdConnectConfiguration? configuration = null;

        if (Options.ConfigurationManager != null)
        {
            configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
        }

        string? tokenEndpoint = tokenEndpointRequest.TokenEndpoint ?? configuration?.TokenEndpoint;

        if (tokenEndpoint == null)
        {
            throw new Exception("Token endpoint must be set.");
        }

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(tokenEndpointRequest.Parameters),
            Version = Backchannel.DefaultRequestVersion
        };
        var responseMessage = await Backchannel.SendAsync(requestMessage, Context.RequestAborted);

        const string dpopNonceHeaderName = "DPoP-Nonce";

        OpenIdConnectMessage? result;

        if (responseMessage.Headers.Contains(dpopNonceHeaderName))
        {
            string dpopNonce = responseMessage.Headers.GetValues(dpopNonceHeaderName).Single();

            var dpopProof = await _dpopProofService.CreateProofTokenAsync(new DPoPProofRequest
            {
                DPoPJsonWebKey = _dpopJwk.Jwk,
                DPoPNonce = dpopNonce,
                Method = "POST",
                Url = tokenEndpoint
            });

            if (dpopProof == null)
            {
                throw new Exception("Could not create DPoP proof.");
            }

            string dpopProofToken = dpopProof.ProofToken;

            Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
            Backchannel.DefaultRequestHeaders.Add(OidcConstants.HttpHeaders.DPoP, dpopProofToken);

            result = await base.RedeemAuthorizationCodeAsync(tokenEndpointRequest);

            Backchannel.DefaultRequestHeaders.Remove(OidcConstants.HttpHeaders.DPoP);
        }
        else
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync(Context.RequestAborted);
            result = new OpenIdConnectMessage(responseContent);
        }

        if (!string.IsNullOrEmpty(result.AccessToken) && result.TokenType != "DPoP")
        {
            throw new Exception($"Expected 'DPoP' token type, but received '{result.TokenType}' token");
        }

        return result;
    }
}