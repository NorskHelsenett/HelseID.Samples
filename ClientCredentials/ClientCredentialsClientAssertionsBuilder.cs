using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HelseID.Samples.Configuration;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.ClientCredentials;

/// <summary>
/// This class builds a client assertion, which includes a JWT token that establishes the identity of this
/// client application for the STS. See https://www.rfc-editor.org/rfc/rfc7523#section-2.2 for more information
/// about this mechanism.
/// </summary>
public class ClientCredentialsClientAssertionsBuilder
{
    private readonly HelseIdSamplesConfiguration _configuration;
    
    public ClientCredentialsClientAssertionsBuilder(HelseIdSamplesConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ClientAssertion BuildClientAssertion(string stsTokenEndpoint)
    {
        var token = CreateSigningTokenForClientAssertion(stsTokenEndpoint);

        return new ClientAssertion
        {
            Value = token,
            Type = OidcConstants.ClientAssertionTypes.JwtBearer
        };
    }

    private string CreateSigningTokenForClientAssertion(string stsTokenEndpoint)
    {
        var claims = GetClaimsForClientAssertionToken();

        var signingCredentials = GetClientAssertionSigningCredentials();

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration.ClientId,
            audience: stsTokenEndpoint,
            claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(60),
            signingCredentials);

        // This method can be used to enrich the token with special information,
        jwtSecurityToken = ExpandSigningToken(jwtSecurityToken);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    // Can be used by subclasses to expand the signing token with extra information before signing
    protected virtual JwtSecurityToken ExpandSigningToken(JwtSecurityToken jwtSecurityToken)
    {
        return jwtSecurityToken;
    }

    private List<Claim> GetClaimsForClientAssertionToken()
    {
        return new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, _configuration.ClientId),
            new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };
    }

    private SigningCredentials GetClientAssertionSigningCredentials()
    {
        var securityKey = new JsonWebKey(_configuration.RsaPrivateKeyJwk);
        return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
    }
}