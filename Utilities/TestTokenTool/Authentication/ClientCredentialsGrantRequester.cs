using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestTokenTool.Constants;

namespace TestTokenTool.Authentication;

public class ClientCredentialsGrantRequester
{
    private IConfiguration _config;

    public ClientCredentialsGrantRequester(IConfiguration config)
    {
        _config = config;
    }
    
    public async Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest()
    {
        var tokenEndpoint = await GetTokenEndpointFromHelseId();
        var clientAssertion = BuildClientAssertion();

        return new ClientCredentialsTokenRequest
        {
            Address = tokenEndpoint,
            ClientAssertion = clientAssertion,
            ClientId = _config[ConfigurationConstants.AuthenticationClientId]!,
            Scope = _config[ConfigurationConstants.AuthenticationScope],
            GrantType = OidcConstants.GrantTypes.ClientCredentials,
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        };
    }

    private async Task<string> GetTokenEndpointFromHelseId()
    {
        using var httpClient = new HttpClient();
        var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(_config[ConfigurationConstants.AuthenticationStsUrl]);
        if (discoveryDocument.IsError)
        {
            throw new Exception(discoveryDocument.Error);
        }
        if (discoveryDocument.TokenEndpoint == null)
        {
            throw new Exception("No token endpoint found at the authorization server.");
        }
        return discoveryDocument.TokenEndpoint;
    }

    private ClientAssertion BuildClientAssertion()
    {
        var token = CreateSigningToken();
        
        return new ClientAssertion
        {
            Value = token,
            Type = OidcConstants.ClientAssertionTypes.JwtBearer
        };
    }

    private string CreateSigningToken()
    {
        var header = CreateJwtHeaderWithSigningCredentials();
        var payload = CreateJwtPayload();

        var jwtSecurityToken = new JwtSecurityToken(header, payload);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private JwtHeader CreateJwtHeaderWithSigningCredentials()
    {
        var jwk = File.ReadAllText(FileConstants.JwkFileName);
        var securityKey = new JsonWebKey(jwk);
        var signingCredentials = new SigningCredentials(securityKey, _config[ConfigurationConstants.AuthenticationSigningKeyAlgorithm]);
        return new JwtHeader(signingCredentials);
    }

    private JwtPayload CreateJwtPayload()
    {
        var tokenIssuedAtEpochTime = EpochTime.GetIntDate(DateTime.UtcNow);
        return new JwtPayload
        {
            [JwtRegisteredClaimNames.Iss] = _config[ConfigurationConstants.AuthenticationClientId],
            [JwtRegisteredClaimNames.Sub] = _config[ConfigurationConstants.AuthenticationClientId],
            [JwtRegisteredClaimNames.Aud] = _config[ConfigurationConstants.AuthenticationStsUrl],
            [JwtRegisteredClaimNames.Exp] = tokenIssuedAtEpochTime + 30,
            [JwtRegisteredClaimNames.Nbf] = tokenIssuedAtEpochTime,
            [JwtRegisteredClaimNames.Iat] = tokenIssuedAtEpochTime,
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString("N")
        };
    }
}
