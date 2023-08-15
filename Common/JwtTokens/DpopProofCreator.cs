using System.IdentityModel.Tokens.Jwt;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.JwtTokens;

/// <summary>
/// This class creates a JWT token that is used for demonstration of possession of a private key.
/// See https://www.ietf.org/archive/id/draft-ietf-oauth-dpop-16.html#DPoP-Proof-Syntax
/// </summary>
public class DpopProofCreator : IDpopProofCreator
{
    private readonly HelseIdConfiguration _configuration;
    
    public DpopProofCreator(HelseIdConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateDpopProof(string? dPoPNonce, string url, string httpMethod)
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
            [JwtClaimTypes.DPoPHttpMethod] = httpMethod,
            [JwtClaimTypes.DPoPHttpUrl] = url,
            [JwtClaimTypes.IssuedAt] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            [JwtClaimTypes.Nonce] = dPoPNonce,
        };
        
        var jwtSecurityToken = new JwtSecurityToken(jwtHeader, payload);
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}