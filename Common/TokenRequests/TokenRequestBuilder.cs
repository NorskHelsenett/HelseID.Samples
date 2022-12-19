using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.Endpoints;
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
    private readonly IHelseIdEndpointDiscoverer _endpointDiscoverer;
    private readonly HelseIdConfiguration _configuration;

    public TokenRequestBuilder(
        IClientAssertionsBuilder clientAssertionsBuilder,
        IHelseIdEndpointDiscoverer endpointDiscoverer,
        HelseIdConfiguration configuration)
    {
        _clientAssertionsBuilder = clientAssertionsBuilder;
        _endpointDiscoverer = endpointDiscoverer;
        _configuration = configuration;
    }

    public async Task<RefreshTokenRequest> CreateRefreshTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        RefreshTokenRequestParameters tokenRequestParameters)
    {
        var tokenEndpoint = await FindTokenEndpoint();
        var clientAssertion = BuildClientAssertion(payloadClaimsCreator, tokenRequestParameters.PayloadClaimParameters);

        return CreateRefreshTokenRequest(tokenRequestParameters, tokenEndpoint, clientAssertion);
    }

    private RefreshTokenRequest CreateRefreshTokenRequest(
        RefreshTokenRequestParameters tokenRequestParameters,
        string tokenEndpoint,
        ClientAssertion clientAssertion)
    {
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
        ClientCredentialsTokenRequestParameters tokenRequestParameters)
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
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        };
    }

    private async Task<string> FindTokenEndpoint()
    {
        return await _endpointDiscoverer.GetTokenEndpointFromHelseId();
    }

    private ClientAssertion BuildClientAssertion(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        // HelseID requires a client assertion in order to recognize this client
        return _clientAssertionsBuilder.BuildClientAssertion(payloadClaimsCreator, payloadClaimParameters);
    }
}
