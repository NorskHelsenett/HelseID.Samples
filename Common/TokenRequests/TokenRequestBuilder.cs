using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.JwtTokens;
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
    private readonly IDPoPProofCreator _idPoPProofCreator;

    public TokenRequestBuilder(
        IClientAssertionsBuilder clientAssertionsBuilder,
        IHelseIdEndpointsDiscoverer endpointsDiscoverer,
        HelseIdConfiguration configuration,
        IDPoPProofCreator idPoPProofCreator)
    {
        _clientAssertionsBuilder = clientAssertionsBuilder;
        _endpointsDiscoverer = endpointsDiscoverer;
        _configuration = configuration;
        _idPoPProofCreator = idPoPProofCreator;
    }

    public async Task<RefreshTokenRequest> CreateRefreshTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        RefreshTokenRequestParameters tokenRequestParameters)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the refresh token grant
        var result = new RefreshTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            GrantType = OidcConstants.GrantTypes.RefreshToken,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            RefreshToken = tokenRequestParameters.RefreshToken,
        };

        if (tokenRequestParameters.HasResourceIndicator)
        {
            result.Resource = tokenRequestParameters.Resource;
        }
        return result;
    }

    public async Task<TokenExchangeTokenRequest> CreateTokenExchangeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        TokenExchangeTokenRequestParameters tokenRequestParameters)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the exchange token grant 
        return new TokenExchangeTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            Scope = _configuration.Scope,
            GrantType = OidcConstants.GrantTypes.TokenExchange,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken,
            SubjectToken = tokenRequestParameters.SubjectToken,
        };
    }

    public async Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        ClientCredentialsTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        // This class comes from the IdentityModel library and abstracts a request for a token using the client credential grant 
        var request = new ClientCredentialsTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _configuration.ClientId,
            Scope = _configuration.Scope,
            GrantType = OidcConstants.GrantTypes.ClientCredentials,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
        };
        if (_configuration.UseDPoP)
        {
            request.DPoPProofToken = _idPoPProofCreator.CreateDPoPProof(tokenEndpoint, "POST", dPoPNonce: dPoPNonce);
        }
        return request;
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
