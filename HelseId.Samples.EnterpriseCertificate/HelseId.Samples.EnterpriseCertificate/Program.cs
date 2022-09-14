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
    /// <summary>
    /// DEPRECATED
    /// DEPRECATED
    /// DEPRECATED
    /// 
    /// Note that the use of "virksomhetssertifikater" is deprecated in HelseID, and not supported by Selvbetjening.
    ///
    /// The Private Keys stored in virksomhetssertifikater may still be used for signing JWTs as shown in the client assertion samples,
    /// but the organization number of the virksomhet is deducted using other mechanisms.
    /// 
    /// DEPRECATED
    /// DEPRECATED
    /// DEPRECATED
    /// </summary>

    class Program
    {
        private const string ClientId = "e1fd8e62-b32d-4043-bdb2-fbefcda3ecdf";
        private const string Scope = "udelt:test-api/api";
        private const string TokenEndpoint = "https://helseid-sts.test.nhn.no/connect/token";
        private const string PathToP12File = @"some_path\some_certificate.p12";
        private const string CertificatePassword = @"some_path\some_certificate.p12";


        static async Task Main(string[] args)
        {
            var certificate = new X509Certificate2(PathToP12File, CertificatePassword);
            var securityKey = new X509SecurityKey(certificate);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            TokenResponse result = await GetTokenResponse(certificate, securityKey, signingCredentials);

            if (result.IsError)
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

        private static async Task<TokenResponse> GetTokenResponse(X509Certificate2 certificate, X509SecurityKey securityKey, SigningCredentials signingCredentials)
        {
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
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = IdentityModel.OidcConstants.ClientAssertionTypes.JwtBearer },
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };

            var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);
            return result;
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
