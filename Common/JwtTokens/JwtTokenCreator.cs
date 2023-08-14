using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.JwtTokens;

/// <summary>
/// This class creates a JWT token that can be used in client assertions or in request objects.
/// This class requires an IPayloadClaimsCreator instance that will create the claims that are needed in the token request.
/// </summary>
public class JwtTokenCreator : IJwtTokenCreator
{
    private readonly IJwtPayloadCreator _jwtPayloadCreator;
    private readonly HelseIdConfiguration _configuration;
    
    public JwtTokenCreator(
        IJwtPayloadCreator jwtPayloadCreator,
        HelseIdConfiguration configuration)
    {
        _jwtPayloadCreator = jwtPayloadCreator;
        _configuration = configuration;
    }
    
    public string CreateDPoPToken(string? dPoPNonce, string url)
    {
        var securityKey = new JsonWebKey(_configuration.RsaPrivateKeyJwk.JwkValue);
        var signingCredentials = new SigningCredentials(securityKey, _configuration.RsaPrivateKeyJwk.Algorithm);

        var jwk = new Dictionary<string, string>
        {
            ["kty"] = securityKey.Kty,
            ["n"] = securityKey.N,
            ["e"] = securityKey.E,
            ["alg"] = signingCredentials.Algorithm,
            ["kid"] = securityKey.Kid,
        };

        var jwtHeader = new JwtHeader(signingCredentials)
        {
            [JwtClaimTypes.TokenType] = "dpop+jwt",
            [JwtClaimTypes.JsonWebKey] = jwk,
        };

        var payload = new JwtPayload
        {
            [JwtClaimTypes.JwtId] = Guid.NewGuid().ToString(),
            [JwtClaimTypes.DPoPHttpMethod] = "POST",
            [JwtClaimTypes.DPoPHttpUrl] = url,
            [JwtClaimTypes.IssuedAt] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            [JwtClaimTypes.Nonce] = dPoPNonce,
        };
        
        var jwtSecurityToken = new JwtSecurityToken(jwtHeader, payload);
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public string CreateSigningToken(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        var header = CreateJwtHeaderWithSigningCredentials();
        var payload = _jwtPayloadCreator.CreateJwtPayload(payloadClaimsCreator, payloadClaimParameters, _configuration);

        // The JwtSecurityToken class is part of the System.IdentityModel library.
        var jwtSecurityToken = new JwtSecurityToken(header, payload);

        // This creates a (signed) jwt token which is used for the client assertion.
        // The JwtSecurityTokenHandler class is part of the System.IdentityModel library.
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    /// <summary>
    /// This method uses our (secure) private key to sign the token which is used for the client assertion 
    /// </summary>
    private JwtHeader CreateJwtHeaderWithSigningCredentials()
    {
        var signingCredentials = GetClientAssertionSigningCredentials();
        // The JwtHeader class is part of the System.IdentityModel library.
        return new JwtHeader(signingCredentials);
    }

    private SigningCredentials GetClientAssertionSigningCredentials()
    {
        var securityKey = new JsonWebKey(_configuration.RsaPrivateKeyJwk.JwkValue);
        return new SigningCredentials(securityKey, _configuration.RsaPrivateKeyJwk.Algorithm);
    }
}