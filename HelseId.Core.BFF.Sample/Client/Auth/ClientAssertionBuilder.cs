using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Duende.IdentityModel.Client;

namespace HelseId.Core.BFF.Sample.Client.Auth;

public static class ClientAssertionBuilder
{
    public static ClientAssertion GetClientAssertion(string clientId, string jwk, string authority)
    {
        var clientAssertionString = BuildClientAssertion(clientId, jwk, authority);

        return new ClientAssertion
        {
            Type = OidcConstants.ClientAssertionTypes.JwtBearer,
            Value = clientAssertionString
        };
    }

    private static string BuildClientAssertion(string clientId, string jwk, string authority)
    {
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, clientId),
            new(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };

        var header = new JwtHeader(GetClientAssertionSigningCredentials(jwk)) { ["typ"] = "client-authentication+jwt" };
        var payload = new JwtPayload(
            issuer: clientId,
            audience: authority,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddSeconds(60)
        );

        var credentials = new JwtSecurityToken(header, payload);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(credentials);
    }

    private static SigningCredentials GetClientAssertionSigningCredentials(string jwk)
    {
        var securityKey = new JsonWebKey(jwk);

        var alg = securityKey.Alg;
        if (string.IsNullOrWhiteSpace(alg))
        {
            throw new Exception("JWK must include the 'alg' parameter");
        }

        return new SigningCredentials(securityKey, alg);
    }
}