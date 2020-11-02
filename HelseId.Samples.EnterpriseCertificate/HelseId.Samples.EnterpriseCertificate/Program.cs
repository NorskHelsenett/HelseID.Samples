using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HelseId.Samples.EnterpriseCertificate
{
    class Program
    {
        private const string ClientId = "0a7d931d-7a83-4a3d-95cf-9abc94ef3ef3";
        private const string Scope = "udelt:test-api/api";
        private const string TokenEndpoint = "https://helseid-sts.utvikling.nhn.no/connect/token";

        static async Task Main(string[] args)
        {
            var certificate = new X509Certificate2(@"GothamSykehus.p12", "bMKXs98yOizPLHVQ");
            var securityKey = new X509SecurityKey(certificate);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, ClientId),
                new Claim(JwtClaimTypes.IssuedAt, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(ClientId, TokenEndpoint, claims, DateTime.Now, DateTime.Now.AddMinutes(1), signingCredentials);

            var thumbprint = Base64Url.Encode(certificate.GetCertHash());
            var x5C = GenerateX5C(certificate);
            var pubKey = securityKey.PublicKey as RSA;
            var parameters = pubKey.ExportParameters(false);
            var exponent = Base64Url.Encode(parameters.Exponent);
            var modulus = Base64Url.Encode(parameters.Modulus);

            token.Header.Add("x5c", x5C);
            token.Header.Add("kty", pubKey.SignatureAlgorithm);
            token.Header.Add("use", "sig");
            token.Header.Add("x5t", thumbprint);
            token.Header.Add("e", exponent);
            token.Header.Add("n", modulus);

            var tokenHandler = new JwtSecurityTokenHandler();
            var clientAssertion = tokenHandler.WriteToken(token);

            var request = new ClientCredentialsTokenRequest
            {
                Address = TokenEndpoint,
                ClientId = ClientId,
                Scope = Scope,
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = IdentityModel.OidcConstants.ClientAssertionTypes.JwtBearer }
            };

            var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);

            if(result.IsError)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(result.Error);
            }
            else
            {
                Console.WriteLine("Access token:");
                Console.WriteLine(result.AccessToken);
                Console.WriteLine("Copy/paste the access token at https://jwt.ms to see the contents");
            }
        }
        private static List<string> GenerateX5C(X509Certificate2 certificate)
        {
            var x5C = new List<string>();

            var certificateChain = X509Chain.Create();
            certificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            certificateChain.Build(certificate);

            if (certificateChain != null)
            {
                foreach (var cert in certificateChain.ChainElements)
                {
                    var x509Base64 = Convert.ToBase64String(cert.Certificate.RawData);
                    x5C.Add(x509Base64);
                }
            }
            return x5C;
        }     
    }
}
