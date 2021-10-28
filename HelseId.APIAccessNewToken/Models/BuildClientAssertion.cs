using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;

namespace HelseId.APIAccessNewToken.Models
{
    public static class BuildClientAssertion
    {
        /// <summary>
        /// Generates a signed JWT
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="authority"></param>
        /// <param name="privateJwkFilename"></param>
        /// <returns></returns>
        public static string Generate(string clientId, string authority, string privateJwkFilename)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString()),
                new Claim(JwtClaimTypes.ClientId, clientId)
            };

            // Creates a security token designed for representing a Jwt with the given parameters
            var signingCredentials = GetClientAssertionSigningCredentials(privateJwkFilename);
            var credentials = new JwtSecurityToken(clientId, authority, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(10), signingCredentials);

            // The tokenhandler serializes the credentials into a Jwt
            var tokenhandler = new JwtSecurityTokenHandler();
            var jwt = tokenhandler.WriteToken(credentials);
            return jwt;
        }

        /// <summary>
        /// Loads a private jwk in json format and returns the corresponding signing credentials
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SigningCredentials GetClientAssertionSigningCredentials(string filename)
        {
            //NB! In production environment the private JWK must be protected (stored at a secure location)
            var securityKey = new JsonWebKey(File.ReadAllText(filename));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
            return signingCredentials;
        }
    }
}
