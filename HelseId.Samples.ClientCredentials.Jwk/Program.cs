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
        private const string _jwkPrivateKey = @"{'d':'Q-vCzEk9S0z4wRdIGUNkl5ypS01vj-cnmOiWvgHb4FxPwBJXGFaqAmRINv_cBujQyhsE7dGRrFSOqJoBHfRqlgll61XrPcGeWA7TzaA5PCF5qfcaTRNR0KKqrb0tVZanLwgmcfY8rjs5ZY2ReRmoKhPr8N9FjpmBOGX_ti5llsKCzKq-CBB6nPTrQkC7N_wvow-TwrWF3LLczBof1yEkrWg8x2RRgnVPMvgeiaEvtN1ZVJ6ZouUZJ-6NKNvwo0r8yAQ8xUcELthYDDMjvagMedPIH2X_ftNNNF5j_JtQ8eJalk8tolxm5fS-XXpRXpOMM9kIDGgnsb_yElPzIvHiV6Btfrzc0Z-m1gGvt7WsOY9vpHv7KrZ6WeRltA68Fub1FZh9QKx1gwcoi98Bm52NnGzh1WuQLbDA1V6QFm45xNEDqZzExTcf6n9ofbsb8SLVHiu2QJBEGTcZt39NvTEC_8mo1ljdqf6x5kz46iBO2uPUBBFj5iBhanPZ2xOc4LDT7WiEUxEP_srpIKXmmiMBq0jMFmqN8TeAeXNUm3umv7UlJoTBxNUvUm_BaP6Lqs7bOSbQC8ZVYNr2XDRxl7StcWlDd7KrcmMKoCUUSvigpzzbypa2gUqubptF7rJZDuxcgCl4IkSP6pIemRTtsTzhHndv96TDfMUmQQteLudQdUk','dp':'nX3tTX0g6cutqdtOIkUqexiHGpQCZbQxETqMKj12vmJ4f4jJZjU1lGH9-TLPumjdazhD1Ey_wQ1--bd5YdmpGbvpfTHZCSJxxo5kNvrjUBB6T1cIqIrzPGRvxoPZeiVBPWmaI3qfAxkVrJbPKt5gHFJjqHItCs_GaltwycHcAfQf8rmN1QX3fQiTRGiCTk9D-gR-lnl8VQpg904DzXxPsfmcCzmhaf_Mb79MugTPxTF8ohT-crATz-2QEuIM0uvD-KxqkdOWNfcl2XXbx1PtKJxxdL3e9JaWYMNuSfpH5O5F6hAh8FjMCYIYn0PNmWFjTsjKZVeThtbUHRvl7IDipw','dq':'soyZmxug009D3tKP-kS3E38QWw8I5dIs7Tkx0CT1zs2AjF984Kiw_y4wPp7dznSnieLNGRYI3t6Eb2dj7t5oHY2FPI0bzNIUbcHSP80L0YqUReoIPo3nmnMp6AuxBZHOpeCsIV_KS9zzlMyaaIlu7cIpyKKwf55NnVs5riBll1NtiqMeuJVFfOgTqoBdjD2nxL0wyqAEydDBs2-aLoqUX-vuN7m8Hz9LHM2jBClVko4F4hLJvZn8C0WowtdgDigLWKw80EO5oIr9y8MkjaLjAJBvoax6mJdup6AFXMWGkPOEnbS7S7F5MTwFW28udyi01NL_GGwFXoJXVJajC4CXbw','e':'AQAB','kty':'RSA','n':'pB9mfL5z6fjnC1iHUfsb_HZ8Gfazk7Siud8JSx6xqOZ1pjMmjfoE4WIYFMQhNoundB1dUfi1W1USdFSWkV9bLvLXg6uQzrfx5TRsBbOI4qIQFBfTfPdwHTB9gKZhBQChWCvZDnKmbg5ORi1pFFKyM-Bq9-TgeRw4bKcH1f-LddECY8Od8CqdAA1eD0DIfNBBO-3dxwoRMxvSJ78FxP9puFlZiIPGTF4tbBsfX0-LUVDpWLSa4u9x3GTa0wZgOyi4RjwJ1Kelvpzc6VumhwLVONfT7ooPkB6errpTbggEzmbZlxg4McfepYQXDT1cQ3o4qkAbiYaUbyX2F4C9mMlcgI_cwlMkNPKdHMFOeBOy1uDbnKmvDZtvVr9K67tzmqiLJt5km5nKtVrO3TvLaKydNvJ3WDdrR_qz5oNQIH4glLnLSqGmDEWhfAoccV73ABEyUcWivXlrhmUR__2Y8dly4VzZqdC0mA146HXB7RPORARZxXrTAHnArDlqWTwXNRUDk5BM1fFNCBY9LLtpdhhn0UiJS3JSHLtYHC0awoCNyRTEo28-HZ07CnR6L4bDCkGm3g38JmTiTOWXcf-9IBElWSem_VDVB84JsBWm5FDeEeO4A7K5Ehtt24eaerh_951N8bTpnhXMYzduSt_ibTaYmOAwO7KZmWHgqPTe5ndfimE','p':'xcW_yZVSmB1ZsK_X4f7-QoosCPFc1JYnR0Q23TmRYsrxuwxe5t0UGEThmSQLHIlnrD8Jnzjn8tcndXfa9kmudkU_nJ3Njyq77Mq303S4yQ4nSkbmg-Ft7aSYr3f8u6sn40nvQj_FSGWj-ddkpjKHY2XCX-MQ-15ZQaz4NiCv-tIZ4l6vD7Nh7aocTuATQxPBPR3mhF-WboxUUdWLrKuSXn3FScxAtU3N5UPdzpyYF5cxkL9EnjJjoCh_a8ES9Cgel144oJvAan1jFYcXejj91uReexvObghD4qzsywVt8oBnZ3798nIMhQpxpEOeQQdgPykT0qBWUxi96UnsGLJt3w','q':'1HFxGbZQprkFajwIjj3PthyLpTfhDN_dWA0e4YKRUFw1FeB8L-8znLPmiMvZon2LLYtdqiJkjXRe2_TOJTOni3Y8jPKhVKXkO99ykZcxTQc3jmb-aJexQEIOBej2L23ST8d-SGuL19JgJoVgQ4omgdC7EtugshglWOtH3UgzYjl-_6xPfi4uDNzhRthVBMA6m_2XHn9Q_qU5N_81RvqD7DPESOTx7TrYoTkReqP4OoDdOyLEwfAfLr_1oO_CrRJQdHzv5eapBP_Ie2j9BkICdfJdU4r9oUetkGJLTkRKTORW6u3PFI0SNVVW1Jj-F2SGayp3Nfg6YwqJHIqXvSCPvw','qi':'t2IYaNw8fRQ1LHatnu7SJdM7NbaI82L4tp7Mcfw4t9XnAobPAE_0S1tKoz7pNk9r1q5QTE2NSA7o0S_tONTT3wocz1BKJyZJqWNwBpspLQzjCRedFymJPg33OrWuTjq1yMTkkvHMH0WEZoq6jiozqY_mGDb4lDJ8XfAZUFBXT49UpwahFBdRk3xOycQJ0UjTbg2hDcmFX-ThBLWmKIkuShzd6UDXqfMSAy1RF10IDH7cxIBpRozQOQvh5fYSikYV6d9wYVPj59OTqLQEsUPMlrqXz4NcV99PnqAofpj95JAfyZOfKfETAkPg0eXCY9uKCmhQZ4o7lSqOGxN49HciZQ','kid':'8146671A52E5AFEC2F66DC82B378AF27'}";

        // NOTE: This client_id is only to be used for this particular sample. Your application will use it's own client_id.
        private const string _clientId = "41e80943-00af-4f24-a04b-5f1ca2ebd9a4";

        private const string _scopes = "nhn:helseid-public-samplecode/client-credentials";

        // You can use the sample API located at
        // https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI
        private const string _apiUrl = "http://helseidhackatonapi.azurewebsites.net/api";

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
