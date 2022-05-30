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

        // NOTE: In a production environment you would use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
        const string _jwkPrivateKey = "{'d':'ah-htj9aYcA-Ycpec0g84Q9dVP2HRW009nxZW38NypaJgciuN0V4FRWz2JtXOUdTMlZVtJETulYmfKKWYqVATi7STeAFMMT0ZhsQVz8D9z4kb9m9-lKQTJkS4lAdRVajXSeDAC_L9PWMqHGa7n3C_wkYU-ov59STncw1Xxfsk-ZQkGqonBDcWs_emJ6Yfw1mo7Uq4gPWa0ZsHj9oGFfH0ExubzNY6HnrFYnXGTxzLIhzfPQB3Zur2JZAveApyChWcI4Ry-H6pmBkL7VWT78eFCG4RlXBH2rNdXVj2yYlx2ILQTXDPLOOOEQm5w2rk5YeiuDEb16geJtr97rEaQylbfc3crjGpiMAmLinzkSFFjUvDq9uKdAPM7xisW0uRLElmiGsrdvQPGjtpjigDlOklYZgxESVrpVdr-UuDfe-jGiwCINZpbZ5iyvk8X7kpbsjN1tFnpLW7ojOAHKsQCoQiDZUiGQ9eG4SYqsgH_dd1Y67Kl0DeTbOhlGnlTqTCIHuCVXpZT9WXGdDEiluUhtRNApfzoNjzGDDg5gahrjMUN69bCxxVdmGnqQRN4-e4NxkRxbP5dgRkd0f9Eh80p7Aio6-qkUb3U1kY5kjICP5QGCZluQncJLifhIV1Ui0KOZI4t7bK4sgiY1QtrPlHleByR1KsRpQYibGiphupOxQDcE','dp':'EufIi5gLWC7J9CPEP4sYCR4zkxyImE7e4RlPv3S5yNSvwgsHaqy1WxS19iR0sp1_QzOH8Z-RnzWHsG2jcowSJ3cLHoWA059tQZqNMDhtiEAWMs9Le1ctK85rKf9kwPzADyj54tDGWgiBulxX8UwMyF0SWvtlRIn8gPaTEe1HeSiDFOoQJQSSpsX6GC_80P8uf2qwEvDymcYbNB_TwmPwMkB0s-krP3eRshjuokmb2ImSJCTzXi9pIsGjVGl7yjV8k-MwQqz0jr5sabBvDA0hMNghdPmI7DWM65XPma5xf-_BjZfPQ9FRpDoj5PB9mVU-_IkzSkOJ1_fZZxfiYT1Epw','dq':'PU21zTQEjZBfxyS8QzWI6XthzGxdxDdj_293lCtKvTNkm6B_24H-X3yhwKu-99MJR6Khx2UWJVI5BbRvGAhvnp_OeGsy67H_dv59Lq_57ZiYBTCp1_VsE7AyWz_OfSdyS6cVeoWUsxZ1zZJu3j4RuMkQI90qspGpDuCuenyrfUAogxGjzUVh9u0Q5yMOMycI52Qrp5I5BsaTIf92eEZX0oABuaPmNRtarKNzn93g5WMpamWWbTF5LTcjZuM-gZwViUNl1pjC0xCVTody-N38vT91PcNYfPcYdfW88Wk_45PO-DXFEDPBuVF2g-epy5-5FldlGvLGFUYRMo96vs4N0Q','e':'AQAB','kty':'RSA','n':'uJTJ56hHiQpT7xn7ax6n50xomosd6fac_rRXIDvZzSfQVchzqWMibCV0hShv44JsAnTNvmNpoUbFwMuf8MBtfTjbIGjryUgbQFmCBtgSoYy-VCANnaAxx_jSa9sJmmMyrqdoFTdpzVqP6WCpXOSd0VbL6Pi-I3fe3HPKz7d25Q-TO23abw1Rt3fbUL-nf40HZkNHW_Skro7_RVLEe3znKVYxwdU5saSad0hf5gssFiVlBgeZ7-Ua_NygJnzYYRZ-DKPxZHgN-JMKb7Rh3nJzk0szCY15yNvjhWZ2mcbT2Vqtg92t8c5JkJf2uj4WnoDUaoauQT5lQN5w9z2cBHbN22lOdYs92PcWwp6giLJxTwYEarytx2f84tv0JuJA2M6d4oKQSBvgTlZfOPc_Litg4T9EeJ0cs0PUHpegaG-b4UaD3786UkC7caea_1h_-ieFhkMBWz0YnJY40TGD1HhCqu-fmL5_QiuMUH_w3_A3_ZiLIoyPfTe3eIhMC4biGuoVQq-88Z9e5oqk_NgZ4ibfL1WNVp5EZWL-2lnZpqZi0qE7WpX_7_sWqAErIttv-161UzYO_8GIsVmE1CNBe8TGQfTwQ3xRP5pNIiPVcdumPIUgVmNk4SCLCjio5SzunGB7NESGDOXVOWCHUZ5FL998bLD5kTdJThoXQvCn6FGl3tE','p':'2UEy1LV7aLsxGuAw_MiLKhJBgbAsvzOhX5OWRtsFxU6JZFyJQZaDVbIHwld7Fn96cVWncQ16REhPoW8vAbqiNO6-fOh-dP3ACPgXG3_gbcfRzg-iZAqd32R1oICBS-Jro5Nqmehw4ufiUpksd_EwuQZFlG-DvsnGw8NP4SHdL6BO43USf20cBqv2ZrV8KUwnQnGLoceZartovE8tqVnwNS1s3xdCazsUrb9Nqs_uJBB1r23SFcLUx5wU65y3IcTIQ9x0b2DhU9nlhU_60I-FF_wnMUmRYgNfWX70FOuJ9btjITvgBcWe1y-42uT00JczB1NCubJY8nmHvZ9gPy_7Kw','q':'2X_gcTlYKfoyqUA9wQzrCCcuyN82cgvduYjM8Gg0AU33j20lFhbBKC1AmaHBS_lc8k5nsSfEt7ewgT1gvbFjU7MD_pTyA9Z3pJ1Fsst0tocePKfYNMOhqiJ0IRHDbC5WFBYwlWKNl0Vq0v1mkIhex0XcepXgglVPR6V-NuyuNEVDV9bZ9NGI37kmwIartq0gBQ87Qbd_vJXQAEH5dHsRTrc3dUSgcY2UH3YUqvuvxmdQkauHMI9VPcJdU1sZdOS2yeh7pfQoE8g8vqb95dkNIKdy7yfjnxfpkDeyfV6XLvMQPic12t6nmH25j41T-XrMh32NnTZzo3sLigsMufDf8w','qi':'BZRvnc35ZRiwl7jX8qOVnbdN9x7W1jPNJ8SWIpVFVAPYhhgYkAPuUwj5pmpcCVghba1PFpBqNXNf3xzX-G2WZ7oNSXUIyYeW0zGrxUbk568xE9sxshe6Ac8-nreHuzMY2xcaV6ryFqH0HsDAiRLZvqKljb3ZjUx5NNypzRRwQZCo7_2eK1ArwqRc1F1VYtxFc_MjfoaxJ2CzUEKXBqmp2eAdf14gGeBTi_MDPh3PwYH-qhnRGS6QWCAPkUNGWJr6q-hNiwgCiIqRzBQ4hufa3vDfxNiaSDoYs-U1Nd57gduzYKPzPtVEsDBIn7d6A_Mo-ULFuAdwqPl3Xj7iJRUP6w','kid':'5DC654C985D1037A16D82FCA3B9B8843'}";

        // NOTE: This client_id is only to be used for this particular sample. Your application will use it's own client_id.
        private const string _clientId = "f4352589-549d-47ec-9844-5255f4eb0fad";

        private const string _scopes = "nhn:helseid-public-samplecode/client-credentials";

        // You can use the sample API located at
        // https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI
        private const string _apiUrl = "http://helseidhackatonapi.azurewebsites.net/api/client_credentials";

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
                    },
                    ClientCredentialStyle = ClientCredentialStyle.PostBody
                });

                if (response.IsError)
                {
                    throw new Exception(response.Error);
                }

                Console.WriteLine("Access token:");
                Console.WriteLine(response.AccessToken);

                await CallApi(response.AccessToken);



            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }

        }

        private static async Task CallApi(string accessToken)
        {
            var c = new HttpClient();
            c.SetBearerToken(accessToken);
            var response = await c.GetStringAsync(_apiUrl);
            Console.WriteLine("Response from API (access token claims):");
            Console.WriteLine(response);
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
