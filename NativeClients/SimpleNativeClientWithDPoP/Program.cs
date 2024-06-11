using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.DPoP;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;
using Microsoft.AspNetCore.WebUtilities;

namespace HelseId.Samples.NativeClientWithUserLoginAndApiCall
{
    class Program
    {
        //
        // The values below will normally be configurable
        //

        // In a production environment, you MUST use your own private key stored in a safe environment (a vault, a certificate store, a secure database or similar).
        // The library expects an 'alg' property in the JWK key:
        private const string JwkPrivateKey = """
                                             {
                                               "alg": "RS256",
                                               "d":"ah-htj9aYcA-Ycpec0g84Q9dVP2HRW009nxZW38NypaJgciuN0V4FRWz2JtXOUdTMlZVtJETulYmfKKWYqVATi7STeAFMMT0ZhsQVz8D9z4kb9m9-lKQTJkS4lAdRVajXSeDAC_L9PWMqHGa7n3C_wkYU-ov59STncw1Xxfsk-ZQkGqonBDcWs_emJ6Yfw1mo7Uq4gPWa0ZsHj9oGFfH0ExubzNY6HnrFYnXGTxzLIhzfPQB3Zur2JZAveApyChWcI4Ry-H6pmBkL7VWT78eFCG4RlXBH2rNdXVj2yYlx2ILQTXDPLOOOEQm5w2rk5YeiuDEb16geJtr97rEaQylbfc3crjGpiMAmLinzkSFFjUvDq9uKdAPM7xisW0uRLElmiGsrdvQPGjtpjigDlOklYZgxESVrpVdr-UuDfe-jGiwCINZpbZ5iyvk8X7kpbsjN1tFnpLW7ojOAHKsQCoQiDZUiGQ9eG4SYqsgH_dd1Y67Kl0DeTbOhlGnlTqTCIHuCVXpZT9WXGdDEiluUhtRNApfzoNjzGDDg5gahrjMUN69bCxxVdmGnqQRN4-e4NxkRxbP5dgRkd0f9Eh80p7Aio6-qkUb3U1kY5kjICP5QGCZluQncJLifhIV1Ui0KOZI4t7bK4sgiY1QtrPlHleByR1KsRpQYibGiphupOxQDcE",
                                               "dp":"EufIi5gLWC7J9CPEP4sYCR4zkxyImE7e4RlPv3S5yNSvwgsHaqy1WxS19iR0sp1_QzOH8Z-RnzWHsG2jcowSJ3cLHoWA059tQZqNMDhtiEAWMs9Le1ctK85rKf9kwPzADyj54tDGWgiBulxX8UwMyF0SWvtlRIn8gPaTEe1HeSiDFOoQJQSSpsX6GC_80P8uf2qwEvDymcYbNB_TwmPwMkB0s-krP3eRshjuokmb2ImSJCTzXi9pIsGjVGl7yjV8k-MwQqz0jr5sabBvDA0hMNghdPmI7DWM65XPma5xf-_BjZfPQ9FRpDoj5PB9mVU-_IkzSkOJ1_fZZxfiYT1Epw",
                                               "dq":"PU21zTQEjZBfxyS8QzWI6XthzGxdxDdj_293lCtKvTNkm6B_24H-X3yhwKu-99MJR6Khx2UWJVI5BbRvGAhvnp_OeGsy67H_dv59Lq_57ZiYBTCp1_VsE7AyWz_OfSdyS6cVeoWUsxZ1zZJu3j4RuMkQI90qspGpDuCuenyrfUAogxGjzUVh9u0Q5yMOMycI52Qrp5I5BsaTIf92eEZX0oABuaPmNRtarKNzn93g5WMpamWWbTF5LTcjZuM-gZwViUNl1pjC0xCVTody-N38vT91PcNYfPcYdfW88Wk_45PO-DXFEDPBuVF2g-epy5-5FldlGvLGFUYRMo96vs4N0Q",
                                               "e":"AQAB",
                                               "kty":"RSA",
                                               "n":"uJTJ56hHiQpT7xn7ax6n50xomosd6fac_rRXIDvZzSfQVchzqWMibCV0hShv44JsAnTNvmNpoUbFwMuf8MBtfTjbIGjryUgbQFmCBtgSoYy-VCANnaAxx_jSa9sJmmMyrqdoFTdpzVqP6WCpXOSd0VbL6Pi-I3fe3HPKz7d25Q-TO23abw1Rt3fbUL-nf40HZkNHW_Skro7_RVLEe3znKVYxwdU5saSad0hf5gssFiVlBgeZ7-Ua_NygJnzYYRZ-DKPxZHgN-JMKb7Rh3nJzk0szCY15yNvjhWZ2mcbT2Vqtg92t8c5JkJf2uj4WnoDUaoauQT5lQN5w9z2cBHbN22lOdYs92PcWwp6giLJxTwYEarytx2f84tv0JuJA2M6d4oKQSBvgTlZfOPc_Litg4T9EeJ0cs0PUHpegaG-b4UaD3786UkC7caea_1h_-ieFhkMBWz0YnJY40TGD1HhCqu-fmL5_QiuMUH_w3_A3_ZiLIoyPfTe3eIhMC4biGuoVQq-88Z9e5oqk_NgZ4ibfL1WNVp5EZWL-2lnZpqZi0qE7WpX_7_sWqAErIttv-161UzYO_8GIsVmE1CNBe8TGQfTwQ3xRP5pNIiPVcdumPIUgVmNk4SCLCjio5SzunGB7NESGDOXVOWCHUZ5FL998bLD5kTdJThoXQvCn6FGl3tE",
                                               "p":"2UEy1LV7aLsxGuAw_MiLKhJBgbAsvzOhX5OWRtsFxU6JZFyJQZaDVbIHwld7Fn96cVWncQ16REhPoW8vAbqiNO6-fOh-dP3ACPgXG3_gbcfRzg-iZAqd32R1oICBS-Jro5Nqmehw4ufiUpksd_EwuQZFlG-DvsnGw8NP4SHdL6BO43USf20cBqv2ZrV8KUwnQnGLoceZartovE8tqVnwNS1s3xdCazsUrb9Nqs_uJBB1r23SFcLUx5wU65y3IcTIQ9x0b2DhU9nlhU_60I-FF_wnMUmRYgNfWX70FOuJ9btjITvgBcWe1y-42uT00JczB1NCubJY8nmHvZ9gPy_7Kw",
                                               "q":"2X_gcTlYKfoyqUA9wQzrCCcuyN82cgvduYjM8Gg0AU33j20lFhbBKC1AmaHBS_lc8k5nsSfEt7ewgT1gvbFjU7MD_pTyA9Z3pJ1Fsst0tocePKfYNMOhqiJ0IRHDbC5WFBYwlWKNl0Vq0v1mkIhex0XcepXgglVPR6V-NuyuNEVDV9bZ9NGI37kmwIartq0gBQ87Qbd_vJXQAEH5dHsRTrc3dUSgcY2UH3YUqvuvxmdQkauHMI9VPcJdU1sZdOS2yeh7pfQoE8g8vqb95dkNIKdy7yfjnxfpkDeyfV6XLvMQPic12t6nmH25j41T-XrMh32NnTZzo3sLigsMufDf8w",
                                               "qi":"BZRvnc35ZRiwl7jX8qOVnbdN9x7W1jPNJ8SWIpVFVAPYhhgYkAPuUwj5pmpcCVghba1PFpBqNXNf3xzX-G2WZ7oNSXUIyYeW0zGrxUbk568xE9sxshe6Ac8-nreHuzMY2xcaV6ryFqH0HsDAiRLZvqKljb3ZjUx5NNypzRRwQZCo7_2eK1ArwqRc1F1VYtxFc_MjfoaxJ2CzUEKXBqmp2eAdf14gGeBTi_MDPh3PwYH-qhnRGS6QWCAPkUNGWJr6q-hNiwgCiIqRzBQ4hufa3vDfxNiaSDoYs-U1Nd57gduzYKPzPtVEsDBIn7d6A_Mo-ULFuAdwqPl3Xj7iJRUP6w",
                                               "kid":"5DC654C985D1037A16D82FCA3B9B8843"
                                             }
                                             """;

        // This client_id is only to be used for this particular sample. Your application will use its own client_id.
        private const string ClientId = "f4352589-549d-47ec-9844-5255f4eb0fad";

        // The client is configured in the HelseID test environment:
        private const string StsUrl = "https://helseid-sts.test.nhn.no";

        // An API that requires a logged in user
        // You can find README for this API in the SampleApi folder in this repository
        private const string ApiUrl = "https://localhost:5081/user-login-clients/greetings";

        // The port do not need to be pre-registered in HelseID, which means that you can allocate an available port on your localhost when launching the application:
        private const int LocalhostPort = 8089;

        // In a test environment, the port does not need to be pre-registered in HelseID Selvbetjening;
        // this means that you can allocate any available port when launching the application:
        private static readonly string RedirectUrl = $"http://localhost:{LocalhostPort.ToString()}/callback";

        // This is the scope of the API you want to call (get an access token for)
        private const string ApiScopes = "nhn:helseid-public-samplecode/authorization-code";

        // These scopes indicate that you want an ID-token ("openid"), and what information about the user you want the ID-token to contain
        private const string IdentityScopes = "openid profile helseid://scopes/identity/pid helseid://scopes/identity/security_level";

        static async Task Main(string[] args)
        {
            try
            {
                using var httpClient = new HttpClient();

                // Setup the oidc client for user authentication against HelseID
                var options = new OidcClientOptions
                {
                    Authority = StsUrl,
                    ClientId = ClientId,
                    RedirectUri = RedirectUrl,
                    FilterClaims = false,
                    // This validates the identity token (important!):
                    IdentityTokenValidator = new JwtHandlerIdentityTokenValidator(),
                };

                // Set the DPoP proof, we can use the same key for this as for the client assertion:
                options.ConfigureDPoP(JwkPrivateKey);

                var oidcClient = new OidcClient(options);

                // Authenticate with HelseID using the request object via the system browser
                // The authorizeState object contains the state that needs to be held between starting the authorize request and the response
                var authorizeState = await oidcClient.PrepareLoginAsync();

                // Download the HelseID metadata from https://helseid-sts.test.nhn.no/.well-known/openid-configuration to determine endpoints and public keys used by HelseID:
                // In a production environment, this document must be cached for better efficiency (both for this client and for HelseID)
                var disco = await httpClient.GetDiscoveryDocumentAsync(StsUrl);

                var pushedAuthorizationResponse = await GetPushedAuthorizationResponse(
                    httpClient,
                    disco,
                    authorizeState);

                if (pushedAuthorizationResponse.IsError)
                {
                    throw new Exception($"{pushedAuthorizationResponse.Error}: JSON: {pushedAuthorizationResponse.Json}");
                }

                var urlForAuthorizeEndpoint = $"{disco.AuthorizeEndpoint}?client_id={ClientId}&request_uri={pushedAuthorizationResponse.RequestUri}";

                var browserOptions = new BrowserOptions(urlForAuthorizeEndpoint, RedirectUrl);

                // Create a redirect URI using an available port on the loopback address.
                var browser = new SystemBrowser(port:LocalhostPort);

                var browserResult = await browser.InvokeAsync(browserOptions, default);

                // We need a new client assertion for the call to the /token endpoint
                oidcClient.Options.ClientAssertion = GetClientAssertionPayload(disco);

                // If the result type is success, the browser result should contain the authorization code.
                // We can now call the /token endpoint with the authorization code in order to get tokens:
                var loginResult = await oidcClient.ProcessResponseAsync(browserResult.Response, authorizeState);

                if (loginResult.IsError == false)
                {
                    loginResult = ValidateIdentityClaims(loginResult);
                }

                if (loginResult.IsError)
                {
                    throw new Exception($"{loginResult.Error}: Description: {loginResult.ErrorDescription}");
                }

                Console.WriteLine($"Identity token from login: {loginResult.IdentityToken}");
                Console.WriteLine($"DPoP token from login: {loginResult.AccessToken}");
                // Call the example API
                await CallApi(loginResult.AccessToken);

            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync("Error:");
                await Console.Error.WriteLineAsync(ex.ToString());
            }
        }

        private static async Task<PushedAuthorizationResponse> GetPushedAuthorizationResponse(
            HttpClient httpClient,
            DiscoveryDocumentResponse disco,
            AuthorizeState authorizeState)
        {
            // Sets the pushed authorization request parameters:
            var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(authorizeState.CodeVerifier));
            var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);

            // Setup a client assertion - this will authenticate the client (this application)
            var clientAssertionPayload = GetClientAssertionPayload(disco);

            var pushedAuthorizationRequest = new PushedAuthorizationRequest
            {
                Address = disco.PushedAuthorizationRequestEndpoint,
                ClientId = ClientId,
                ClientAssertion = clientAssertionPayload,
                RedirectUri = RedirectUrl,
                Scope = ApiScopes + " " + IdentityScopes,
                ResponseType = OidcConstants.ResponseTypes.Code,
                ClientCredentialStyle = ClientCredentialStyle.PostBody,
                CodeChallenge = codeChallenge,
                CodeChallengeMethod = OidcConstants.CodeChallengeMethods.Sha256,
                State = authorizeState.State,
            };

            // Calls the /par endpoint in order to get a request URI for the /authorize endpoint
            return await httpClient.PushAuthorizationAsync(pushedAuthorizationRequest);
        }

        private static ClientAssertion GetClientAssertionPayload(DiscoveryDocumentResponse disco)
        {
            var clientAssertion = BuildClientAssertion(disco);

            return new ClientAssertion
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = clientAssertion,
            };
        }

        private static string BuildClientAssertion(DiscoveryDocumentResponse disco)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, ClientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
            };

            var credentials =
                new JwtSecurityToken(
                    ClientId,
                    disco.Issuer,
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddSeconds(60),
                    GetClientAssertionSigningCredentials());

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(credentials);
        }

        private static SigningCredentials GetClientAssertionSigningCredentials()
        {
            var securityKey = new JsonWebKey(JwkPrivateKey);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        private static async Task CallApi(string accessToken)
        {
            // We need to use the HttpRequestMessage type for this functionality:
            var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);

            var proofRequest = new DPoPProofRequest
            {
                Method = request.Method.ToString(),
                Url = request.GetDPoPUrl(),
                // This binds the access token to the DPoP proof:
                AccessToken = accessToken,
            };

            // Set the DPoP proof, we use the same key for this as for the client assertion:
            var proof = new DPoPProofTokenFactory(JwkPrivateKey).CreateProofToken(proofRequest);

            request.SetDPoPToken(accessToken, proof.ProofToken);

            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);

            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Response from the API:");
            Console.WriteLine(response);
            Console.WriteLine("Response body from the API:");
            Console.WriteLine(responseBody);
        }

        private static LoginResult ValidateIdentityClaims(LoginResult loginResult)
        {
            // The claims from the identity token has ben set on the User object;
            // We validate that the user claims match the required security level:
            if (loginResult.User.Claims.Any(c => c is
                {
                    Type: "helseid://claims/identity/security_level",
                    Value: "4",
                }))
            {
                return loginResult;
            }

            return new LoginResult("Invalid security level", "The security level is not at the required value");
        }
    }
}
