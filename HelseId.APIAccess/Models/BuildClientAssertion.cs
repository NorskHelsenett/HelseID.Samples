using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HelseId.APIAccess.Models
{
    public static class BuildClientAssertion
    {
        public static string Generate(string clientId, string authority, SecurityKey securityKey)
        {
            /* The method generates a signed Json Web Token (Jwt) based on the given parameters */

            // Generates a digital signature from the cryptographic securityKey by using a security algorithm
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);


            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.ClientId, clientId),
            };

            // Creates a security token designed for representing a Jwt with the given parameters
            var credentials = new JwtSecurityToken(clientId, authority, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(10), signingCredentials);

            // The tokenhandler serializes the credentials into a Jwt
            var tokenhandler = new JwtSecurityTokenHandler();
            var jwt = tokenhandler.WriteToken(credentials);

            return jwt;
        }
    }
}
