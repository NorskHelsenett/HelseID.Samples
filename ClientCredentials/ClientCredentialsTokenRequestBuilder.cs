using System;
using System.Net.Http;
using System.Threading.Tasks;
using HelseID.Samples.Configuration;
using IdentityModel;
using IdentityModel.Client;

namespace HelseId.Samples.ClientCredentials;

/// <summary>
/// This class creates a token request that is used against the HelseID service,
/// and it also gets the token endpoint for this request from the discovery document in HelseID.
/// </summary>
public class ClientCredentialsTokenRequestBuilder
{
    private readonly ClientCredentialsClientAssertionsBuilder _clientCredentialsClientAssertionsBuilder;
    private readonly HelseIdSamplesConfiguration _configuration;

    public ClientCredentialsTokenRequestBuilder(
        ClientCredentialsClientAssertionsBuilder clientCredentialsClientAssertionsBuilder,
        HelseIdSamplesConfiguration configuration)
    {
        _clientCredentialsClientAssertionsBuilder = clientCredentialsClientAssertionsBuilder;
        _configuration = configuration;
    }

    public async Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest(HttpClient client)
    {
        var tokenEndpoint = await GetTokenEndpointFromSts(client);

        return CreateTokenRequestWithClientCredentialsGrant(tokenEndpoint);
    }

    private async Task<string> GetTokenEndpointFromSts(HttpClient client)
    {
        var disco = await client.GetDiscoveryDocumentAsync(_configuration.StsUrl);
        if (disco.IsError)
        {
            throw new Exception(disco.Error);
        }
        return disco.TokenEndpoint;
    }

    private ClientCredentialsTokenRequest CreateTokenRequestWithClientCredentialsGrant(string tokenEndpoint)
    {
        var clientAssertion = _clientCredentialsClientAssertionsBuilder.BuildClientAssertion(tokenEndpoint);

        return new ClientCredentialsTokenRequest
        {
            Address = tokenEndpoint,
            ClientId = _configuration.ClientId,
            Scope = _configuration.Scope,
            GrantType = OidcConstants.GrantTypes.ClientCredentials,
            ClientAssertion = clientAssertion,
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        };
    }
}