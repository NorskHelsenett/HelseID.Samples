using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HelseId.RefreshTokenDemo
{
    class Program
    {
        const string SubjectClientId = "token_exchange_subject_client";
        const string ActorClientId = "token_exchange_actor_client";
        const string Localhost = "http://localhost:8089";
        const string RedirectUrl = "/callback";
        const string StartPage = "/start";
        const string StsUrl = "https://helseid-sts.test.nhn.no";

        static DiscoveryDocumentResponse _discoveryDocument;

        static async Task Main()
        {
            try
            {
                _discoveryDocument = await new HttpClient().GetDiscoveryDocumentAsync(StsUrl);
                if (_discoveryDocument.IsError)
                {
                    throw new Exception(_discoveryDocument.Error);
                }

                // Client system performs a logon and get an access token that is valid for an api
                var loginResult = await GetSubjectAccessToken();
                if (loginResult.IsError)
                {
                    throw new Exception(loginResult.Error);
                }

                // Here the client system calls the api and the api picks up the token
                // For this demo we just fake it and extract the access token from the login result
                // Everything below this line would be done by the API that receives the call from the client (subject)
                var subjectAccessToken = loginResult.AccessToken;

                // Perform the token exchange process as a separate client
                var exchangeResponse = await PerformTokenExchange(subjectAccessToken);
                if (exchangeResponse.IsError)
                {
                    throw new Exception(exchangeResponse.Error);
                }

                var exchangedAccessToken = exchangeResponse.AccessToken;

                Console.WriteLine("Original (subject) access token:");
                Console.WriteLine(subjectAccessToken);
                Console.WriteLine();
                Console.WriteLine("Exchanged access token:");
                Console.WriteLine(exchangedAccessToken);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }
        }

        private static async Task<LoginResult> GetSubjectAccessToken()
        {
            // Perform a user logon via the browser. This could be done in any of many ways
            // Here we use a client assertion as the secret

            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = StsUrl,
                RedirectUri = Localhost + RedirectUrl,
                Scope = "openid profile helseid://scopes/identity/pid helseid://scopes/identity/security_level udelt:token_exchange_actor_api/scope",
                ClientId = SubjectClientId,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                Policy = new Policy { RequireAccessTokenHash = true, RequireAuthorizationCodeHash = true, ValidateTokenIssuerName = true },
            });

            var state = await oidcClient.PrepareLoginAsync();
            var response = await RunLocalWebBrowserUntilCallback(Localhost, RedirectUrl, StartPage, state);

            var clientAssertionPayload = GetClientAssertionPayload(SubjectClientId, _discoveryDocument, GetClientAssertionSecurityKey());
            var loginResult = await oidcClient.ProcessResponseAsync(response, state, clientAssertionPayload);
            return loginResult;
        }

        private static async Task<TokenResponse> PerformTokenExchange(string subjectToken)
        {
            // Perform the token exchange 
            // To do a token exchange HelseID requires that an enterprise certificate is used as the client secret
            // Also note the SubjectToken and SubjectTokenType parameters
            var request = new TokenExchangeTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientAssertion = new ClientAssertion
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = BuildClientAssertion(ActorClientId, _discoveryDocument, GetEnterpriseCertificateSecurityKey())
                },
                ClientId = ActorClientId,
                Scope = "e-helse:nasjonalt_api/scope",
                SubjectToken = subjectToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token"            
            };

            return await new HttpClient().RequestTokenExchangeTokenAsync(request);
        }

        private static Dictionary<string, string> GetClientAssertionPayload(string clientId, DiscoveryDocumentResponse disco, SecurityKey securityKey)
        {
            var clientAssertion = BuildClientAssertion(clientId, disco, securityKey);

            return new Dictionary<string, string>
            {
                { "client_assertion", clientAssertion},
                { "client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer },
            };
        }

        private static string BuildClientAssertion(string clientId, DiscoveryDocumentResponse disco, SecurityKey securityKey)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
            };

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
            var token = new JwtSecurityToken(clientId, disco.TokenEndpoint, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(60), signingCredentials);

            if (securityKey is X509SecurityKey)
            {
                // Format JWT Header for Enterprise Certificates
                var x509SecurityKey = (X509SecurityKey)securityKey;
                var thumbprint = Base64Url.Encode(x509SecurityKey.Certificate.GetCertHash());
                var x5C = GenerateX5C(x509SecurityKey.Certificate);
                var pubKey = x509SecurityKey.PublicKey as RSA;
                var parameters = pubKey.ExportParameters(false);
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                token.Header.Add("x5c", x5C);
                token.Header.Add("kty", pubKey.SignatureAlgorithm);
                token.Header.Add("use", "sig");
                token.Header.Add("x5t", thumbprint);
                token.Header.Add("e", exponent);
                token.Header.Add("n", modulus);
            }

            return new JwtSecurityTokenHandler().WriteToken(token);
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

        private static SecurityKey GetClientAssertionSecurityKey()
        {
            var jwk = File.ReadAllText("jwk.json");
            return new JsonWebKey(jwk);
        }

        private static SecurityKey GetEnterpriseCertificateSecurityKey()
        {
            var certificate = new X509Certificate2(@"GothamSykehus.p12", "bMKXs98yOizPLHVQ");
            return new X509SecurityKey(certificate);
        }

        private static async Task<string> RunLocalWebBrowserUntilCallback(string localhost, string redirectUrl, string startPage, AuthorizeState state)
        {
            // Build a HTML form that does a POST of the data from the url
            // This is a workaround since the url may be too long to pass to the browser directly
            var startPageHtml = UrlToHtmlForm.Parse(state.StartUrl);

            // Setup a temporary http server that listens to the given redirect uri and to 
            // the given start page. At the start page we can publish the html that we
            // generated from the StartUrl and at the redirect uri we can retrieve the 
            // authorization code and return it to the application
            var listener = new ContainedHttpServer(localhost, redirectUrl,
                new Dictionary<string, Action<HttpContext>> {
                    { startPage, async ctx => await ctx.Response.WriteAsync(startPageHtml) }
                });

            RunBrowser(localhost + startPage);

            return await listener.WaitForCallbackAsync();
        }

        private static void RunBrowser(string url)
        {
            // Thanks Brock! https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }
    }
}
