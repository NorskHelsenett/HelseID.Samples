using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using IdentityModel;
using IdentityModel.Client;

namespace HelseId.Samples.Common.TokenRequests;

/// <summary>
/// This class creates a token requests which can be used as refresh token grants,
/// token exchange grants, or client credentials grants to the HelseID service.
/// It requires an IClientAssertionsBuilder that will create the client
/// assertion which is used for building the token request.
/// It also requires an IHelseIdEndpointDiscoverer for finding the URL of the token endpoint.
/// </summary>
public class TokenRequestBuilder : ITokenRequestBuilder
{
    private readonly IClientAssertionsBuilder _clientAssertionsBuilder;
    private readonly IHelseIdEndpointsDiscoverer _endpointsDiscoverer;
    private readonly HelseIdConfiguration _configuration;
    private readonly IDPoPProofCreator _dPoPProofCreator;

    public TokenRequestBuilder(
        IClientAssertionsBuilder clientAssertionsBuilder,
        IHelseIdEndpointsDiscoverer endpointsDiscoverer,
        HelseIdConfiguration configuration,
        IDPoPProofCreator dPoPProofCreator)
    {
        _clientAssertionsBuilder = clientAssertionsBuilder;
        _endpointsDiscoverer = endpointsDiscoverer;
        _configuration = configuration;
        _dPoPProofCreator = dPoPProofCreator;
    }

    public async Task<RefreshTokenRequest> CreateRefreshTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        RefreshTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the refresh token grant
        var request = new RefreshTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            GrantType = OidcConstants.GrantTypes.RefreshToken,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            RefreshToken = tokenRequestParameters.RefreshToken,
            DPoPProofToken = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", dPoPNonce: dPoPNonce),
        };
        if (tokenRequestParameters.HasResourceIndicator)
        {
            request.Resource = tokenRequestParameters.Resource;
        }
        return request;
    }

    public async Task<TokenExchangeTokenRequest> CreateTokenExchangeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        TokenExchangeTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the exchange token grant
        var request = new TokenExchangeTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            Scope = _configuration.Scope,
            GrantType = OidcConstants.GrantTypes.TokenExchange,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken,
            SubjectToken = tokenRequestParameters.SubjectToken,
            DPoPProofToken = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", dPoPNonce: dPoPNonce),
        };
        return request;
    }

    public async Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        ClientCredentialsTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the client credential grant
        return new ClientCredentialsTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            Scope = _configuration.Scope,
            GrantType = OidcConstants.GrantTypes.ClientCredentials,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            DPoPProofToken = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", dPoPNonce: dPoPNonce),
        };
    }

    public async Task<AuthorizationCodeTokenRequest> CreateAuthorizationCodeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        AuthorizationCodeTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the authorization code grant
        return new AuthorizationCodeTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            Resource = tokenRequestParameters.Resource,
            Code = tokenRequestParameters.Code,
            RedirectUri = tokenRequestParameters.RedirectUri,
            CodeVerifier = tokenRequestParameters.CodeVerifier,
            DPoPProofToken = _dPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", dPoPNonce: dPoPNonce),
        };
    }

    private async Task<string> FindTokenEndpoint()
    {
        return await _endpointsDiscoverer.GetTokenEndpointFromHelseId();
    }

    private ClientAssertion BuildClientAssertion(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        // HelseID requires a client assertion in order to recognize this client
        return _clientAssertionsBuilder.BuildClientAssertion(payloadClaimsCreator, payloadClaimParameters);
    }
}
