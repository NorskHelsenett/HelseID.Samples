using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelseId.Samples.NativeClientWithUserLoginAndApiCall
{
    class Program
    {
        //
        // The values below will normally be configurable
        //

        // In a production environment you would use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
        const string JwkPrivateKey = "{'d':'ah-htj9aYcA-Ycpec0g84Q9dVP2HRW009nxZW38NypaJgciuN0V4FRWz2JtXOUdTMlZVtJETulYmfKKWYqVATi7STeAFMMT0ZhsQVz8D9z4kb9m9-lKQTJkS4lAdRVajXSeDAC_L9PWMqHGa7n3C_wkYU-ov59STncw1Xxfsk-ZQkGqonBDcWs_emJ6Yfw1mo7Uq4gPWa0ZsHj9oGFfH0ExubzNY6HnrFYnXGTxzLIhzfPQB3Zur2JZAveApyChWcI4Ry-H6pmBkL7VWT78eFCG4RlXBH2rNdXVj2yYlx2ILQTXDPLOOOEQm5w2rk5YeiuDEb16geJtr97rEaQylbfc3crjGpiMAmLinzkSFFjUvDq9uKdAPM7xisW0uRLElmiGsrdvQPGjtpjigDlOklYZgxESVrpVdr-UuDfe-jGiwCINZpbZ5iyvk8X7kpbsjN1tFnpLW7ojOAHKsQCoQiDZUiGQ9eG4SYqsgH_dd1Y67Kl0DeTbOhlGnlTqTCIHuCVXpZT9WXGdDEiluUhtRNApfzoNjzGDDg5gahrjMUN69bCxxVdmGnqQRN4-e4NxkRxbP5dgRkd0f9Eh80p7Aio6-qkUb3U1kY5kjICP5QGCZluQncJLifhIV1Ui0KOZI4t7bK4sgiY1QtrPlHleByR1KsRpQYibGiphupOxQDcE','dp':'EufIi5gLWC7J9CPEP4sYCR4zkxyImE7e4RlPv3S5yNSvwgsHaqy1WxS19iR0sp1_QzOH8Z-RnzWHsG2jcowSJ3cLHoWA059tQZqNMDhtiEAWMs9Le1ctK85rKf9kwPzADyj54tDGWgiBulxX8UwMyF0SWvtlRIn8gPaTEe1HeSiDFOoQJQSSpsX6GC_80P8uf2qwEvDymcYbNB_TwmPwMkB0s-krP3eRshjuokmb2ImSJCTzXi9pIsGjVGl7yjV8k-MwQqz0jr5sabBvDA0hMNghdPmI7DWM65XPma5xf-_BjZfPQ9FRpDoj5PB9mVU-_IkzSkOJ1_fZZxfiYT1Epw','dq':'PU21zTQEjZBfxyS8QzWI6XthzGxdxDdj_293lCtKvTNkm6B_24H-X3yhwKu-99MJR6Khx2UWJVI5BbRvGAhvnp_OeGsy67H_dv59Lq_57ZiYBTCp1_VsE7AyWz_OfSdyS6cVeoWUsxZ1zZJu3j4RuMkQI90qspGpDuCuenyrfUAogxGjzUVh9u0Q5yMOMycI52Qrp5I5BsaTIf92eEZX0oABuaPmNRtarKNzn93g5WMpamWWbTF5LTcjZuM-gZwViUNl1pjC0xCVTody-N38vT91PcNYfPcYdfW88Wk_45PO-DXFEDPBuVF2g-epy5-5FldlGvLGFUYRMo96vs4N0Q','e':'AQAB','kty':'RSA','n':'uJTJ56hHiQpT7xn7ax6n50xomosd6fac_rRXIDvZzSfQVchzqWMibCV0hShv44JsAnTNvmNpoUbFwMuf8MBtfTjbIGjryUgbQFmCBtgSoYy-VCANnaAxx_jSa9sJmmMyrqdoFTdpzVqP6WCpXOSd0VbL6Pi-I3fe3HPKz7d25Q-TO23abw1Rt3fbUL-nf40HZkNHW_Skro7_RVLEe3znKVYxwdU5saSad0hf5gssFiVlBgeZ7-Ua_NygJnzYYRZ-DKPxZHgN-JMKb7Rh3nJzk0szCY15yNvjhWZ2mcbT2Vqtg92t8c5JkJf2uj4WnoDUaoauQT5lQN5w9z2cBHbN22lOdYs92PcWwp6giLJxTwYEarytx2f84tv0JuJA2M6d4oKQSBvgTlZfOPc_Litg4T9EeJ0cs0PUHpegaG-b4UaD3786UkC7caea_1h_-ieFhkMBWz0YnJY40TGD1HhCqu-fmL5_QiuMUH_w3_A3_ZiLIoyPfTe3eIhMC4biGuoVQq-88Z9e5oqk_NgZ4ibfL1WNVp5EZWL-2lnZpqZi0qE7WpX_7_sWqAErIttv-161UzYO_8GIsVmE1CNBe8TGQfTwQ3xRP5pNIiPVcdumPIUgVmNk4SCLCjio5SzunGB7NESGDOXVOWCHUZ5FL998bLD5kTdJThoXQvCn6FGl3tE','p':'2UEy1LV7aLsxGuAw_MiLKhJBgbAsvzOhX5OWRtsFxU6JZFyJQZaDVbIHwld7Fn96cVWncQ16REhPoW8vAbqiNO6-fOh-dP3ACPgXG3_gbcfRzg-iZAqd32R1oICBS-Jro5Nqmehw4ufiUpksd_EwuQZFlG-DvsnGw8NP4SHdL6BO43USf20cBqv2ZrV8KUwnQnGLoceZartovE8tqVnwNS1s3xdCazsUrb9Nqs_uJBB1r23SFcLUx5wU65y3IcTIQ9x0b2DhU9nlhU_60I-FF_wnMUmRYgNfWX70FOuJ9btjITvgBcWe1y-42uT00JczB1NCubJY8nmHvZ9gPy_7Kw','q':'2X_gcTlYKfoyqUA9wQzrCCcuyN82cgvduYjM8Gg0AU33j20lFhbBKC1AmaHBS_lc8k5nsSfEt7ewgT1gvbFjU7MD_pTyA9Z3pJ1Fsst0tocePKfYNMOhqiJ0IRHDbC5WFBYwlWKNl0Vq0v1mkIhex0XcepXgglVPR6V-NuyuNEVDV9bZ9NGI37kmwIartq0gBQ87Qbd_vJXQAEH5dHsRTrc3dUSgcY2UH3YUqvuvxmdQkauHMI9VPcJdU1sZdOS2yeh7pfQoE8g8vqb95dkNIKdy7yfjnxfpkDeyfV6XLvMQPic12t6nmH25j41T-XrMh32NnTZzo3sLigsMufDf8w','qi':'BZRvnc35ZRiwl7jX8qOVnbdN9x7W1jPNJ8SWIpVFVAPYhhgYkAPuUwj5pmpcCVghba1PFpBqNXNf3xzX-G2WZ7oNSXUIyYeW0zGrxUbk568xE9sxshe6Ac8-nreHuzMY2xcaV6ryFqH0HsDAiRLZvqKljb3ZjUx5NNypzRRwQZCo7_2eK1ArwqRc1F1VYtxFc_MjfoaxJ2CzUEKXBqmp2eAdf14gGeBTi_MDPh3PwYH-qhnRGS6QWCAPkUNGWJr6q-hNiwgCiIqRzBQ4hufa3vDfxNiaSDoYs-U1Nd57gduzYKPzPtVEsDBIn7d6A_Mo-ULFuAdwqPl3Xj7iJRUP6w','kid':'5DC654C985D1037A16D82FCA3B9B8843'}";

        // This client_id is only to be used for this particular sample. Your application will use it's own client_id.
        const string ClientId = "f4352589-549d-47ec-9844-5255f4eb0fad";
        
        // The client is configured in the HelseID test environment, so we will point to that
        const string StsUrl = "https://helseid-sts.test.nhn.no";

        // An API which requires a logged in user
        // You can find the source code for this API at https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI
        const string ApiUrl = "https://helseidhackatonapi.azurewebsites.net/api";

        // The port do not need to be preregistred in HelseID, so you can allocate an available port when launching the application
        const string Localhost = "http://localhost:8089";

        const string RedirectUrl = "/callback";
        const string StartPage = "/start";

        static async Task Main(string[] args)
        {
            try
            {
                // Download the HelseID metadata from https://helseid-sts.test.nhn.no/.well-known/openid-configuration to determine endpoints and public keys used by HelseID
                var disco = await GetInformationAboutHelseIdFromMetadata(StsUrl);

                // This is the scope of the API you want to call (get an access token for)
                const string apiScopes = "nhn:helseid-public-samplecode/authorization-code";

                // These scopes indicate that you want an ID-token ("openid"), and what information about the user you want the ID-token to contain
                const string identityScopes = "openid profile helseid://scopes/identity/pid helseid://scopes/identity/security_level";

                // Setup the oidc client for user authentication against HelseID (using the browser)
                var oidcClient = new OidcClient(new OidcClientOptions
                {
                    Authority = StsUrl,
                    RedirectUri = Localhost + RedirectUrl,
                    Scope = apiScopes + " " + identityScopes,
                    ClientId = ClientId,
                    Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                    Policy = new Policy { RequireAccessTokenHash = true, RequireAuthorizationCodeHash = true, ValidateTokenIssuerName = true },
                    ResponseMode = OidcClientOptions.AuthorizeResponseMode.FormPost,
                });

                // Authenticate with HelseID using the request object via the system browser
                var state = await oidcClient.PrepareLoginAsync();
                var response = await Authorize(Localhost, RedirectUrl, StartPage, state);

                // Setup a client assertion - this will authenticate the client (this application)
                // This request is done from the client to the server without using
                // a web browser
                var clientAssertionPayload = GetClientAssertionPayload(disco, ClientId);

                // Get tokens from the token endpoint
                var loginResult = await oidcClient.ProcessResponseAsync(response, state, clientAssertionPayload);

                if (loginResult.IsError)
                {
                    throw new Exception(loginResult.Error);
                }

                // The access token can now be used when calling an api
                // Note that you normally don't need to look at the access token, just pass it on to the API as a bearer token
                // Copy the access token and paste it at https://jwt.ms to decode it
                Console.WriteLine("Access token:");
                Console.WriteLine(loginResult.AccessToken);
                Console.WriteLine();

                // Call an API
                await CallApi(loginResult.AccessToken);                
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static async Task CallApi(string accessToken)
        {
            var httpClient = new HttpClient();
            httpClient.SetBearerToken(accessToken);
            var response = await httpClient.GetStringAsync(ApiUrl);
            Console.WriteLine("Response from API (access token claims):");
            Console.WriteLine(response);
        }


        private static async Task<DiscoveryDocumentResponse> GetInformationAboutHelseIdFromMetadata(string stsUrl)
        {
            var disco = await new HttpClient().GetDiscoveryDocumentAsync(stsUrl);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            return disco;
        }

        private static async Task<string> Authorize(string localhost, string redirectUrl, string startPage, AuthorizeState state)
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


        private static Dictionary<string, string> GetClientAssertionPayload(DiscoveryDocumentResponse disco, string clientId )
        {
            var clientAssertion = BuildClientAssertion(disco, clientId);

            return new Dictionary<string, string>
            {
                { "client_assertion", clientAssertion},
                { "client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer },
            };
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
            var securityKey = new JsonWebKey(JwkPrivateKey);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

    }
}