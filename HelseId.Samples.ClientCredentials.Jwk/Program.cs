using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace HelseId.Demo.ClientCredentials.Jwk
{
    class Program
    {
        private const string _jwkPrivateKey = @"Add your Jwk Private Key Here";
        private const string _clientId = "Add your client_id here";
        private const string _scopes = "Add scopes here";

        /// <summary>
        /// Simple sample demonstrating client credentials
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            const string stsUrl = "https://helseid-sts.test.nhn.no";

            try
            {
                var c = new HttpClient();
                var disco = await c.GetDiscoveryDocumentAsync(stsUrl);
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                var response = await c.RequestClientCredentialsTokenAsync(new IdentityModel.Client.ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _clientId,
                    GrantType = GrantTypes.ClientCredentials,
                    Scope = _scopes,
                    ClientAssertion = new ClientAssertion
                    {
                        Type = ClientAssertionTypes.JwtBearer,
                        Value = BuildClientAssertion(disco, _clientId)
                    }
                });

                if (response.IsError)
                {
                    throw new Exception(response.Error);
                }

                Console.WriteLine("Access token:");
                Console.WriteLine(response.AccessToken);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }

        }

        private static string BuildClientAssertion(DiscoveryDocumentResponse disco, string clientId)
        {


            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
            };

            var credentials = new JwtSecurityToken(clientId, disco.TokenEndpoint, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(60), GetClientAssertionSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(credentials);
        }
        private static SigningCredentials GetClientAssertionSigningCredentials()
        {
            var securityKey = new JsonWebKey(_jwkPrivateKey);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }
    }
}
