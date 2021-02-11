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
using System.Threading.Tasks;

namespace HelseId.RefreshTokenDemo
{
    class Program
    {
        const string ClientId = "388a8c48-7889-4ead-b453-cda6dfdd10f4";
        const string Localhost = "http://localhost:8089";
        const string RedirectUrl = "/callback";
        const string StartPage = "/start";
        const string StsUrl = "https://helseid-sts.test.nhn.no";

        static async Task Main()
        {
            try
            {
                var httpClient = new HttpClient();
                var disco = await httpClient.GetDiscoveryDocumentAsync(StsUrl);
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                var oidcClient = new OidcClient(new OidcClientOptions
                {
                    Authority = StsUrl,
                    RedirectUri = "http://localhost:8089/callback",
                    Scope = "openid profile helseid://scopes/identity/pid offline_access",
                    ClientId = ClientId,
                    Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                    Policy = new Policy { RequireAccessTokenHash = true, RequireAuthorizationCodeHash = true, ValidateTokenIssuerName = true },                    
                });

                var state = await oidcClient.PrepareLoginAsync();
                var response = await RunLocalWebBrowserUntilCallback(Localhost, RedirectUrl, StartPage, state);

                var clientAssertionPayload = GetClientAssertionPayload(ClientId, disco);
                var loginResult = await oidcClient.ProcessResponseAsync(response, state, clientAssertionPayload);

                if (loginResult.IsError)
                {
                    throw new Exception(loginResult.Error);
                }

                var refreshTokenRequest = new RefreshTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = ClientId,
                    RefreshToken = loginResult.RefreshToken,
                    Parameters = GetClientAssertionPayload(ClientId, disco)
                };

                var refreshTokenResult = await httpClient.RequestRefreshTokenAsync(refreshTokenRequest);
                
                if (refreshTokenResult.IsError)
                {
                    throw new Exception(refreshTokenResult.Error);
                }

                Console.WriteLine("Access Token: " + refreshTokenResult.AccessToken);
                Console.WriteLine("Refresh Token: " + refreshTokenResult.RefreshToken);

            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }
        }

        private static Dictionary<string, string> GetClientAssertionPayload(string clientId, DiscoveryDocumentResponse disco)
        {
            var clientAssertion = BuildClientAssertion(clientId, disco);

            return new Dictionary<string, string>
            {
                { "client_assertion", clientAssertion},
                { "client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer },
            };
        }

        private static string BuildClientAssertion(string clientId, DiscoveryDocumentResponse disco)
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
            var jwk = File.ReadAllText("jwk.json");
            var securityKey = new JsonWebKey(jwk);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
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
