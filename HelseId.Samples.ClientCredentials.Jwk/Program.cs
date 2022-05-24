using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace HelseId.Demo.ClientCredentials.Jwk
{
    class Program
    {
        private const string _clientId = "kj_client_credentials";
        private const string _scopes = "nhn:hgd-persontjenesten-api/read";

        // NOTE: In a production environment you would use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
        private const string _jwkPrivateKey = @"{'d':'Q-vCzEk9S0z4wRdIGUNkl5ypS01vj-cnmOiWvgHb4FxPwBJXGFaqAmRINv_cBujQyhsE7dGRrFSOqJoBHfRqlgll61XrPcGeWA7TzaA5PCF5qfcaTRNR0KKqrb0tVZanLwgmcfY8rjs5ZY2ReRmoKhPr8N9FjpmBOGX_ti5llsKCzKq-CBB6nPTrQkC7N_wvow-TwrWF3LLczBof1yEkrWg8x2RRgnVPMvgeiaEvtN1ZVJ6ZouUZJ-6NKNvwo0r8yAQ8xUcELthYDDMjvagMedPIH2X_ftNNNF5j_JtQ8eJalk8tolxm5fS-XXpRXpOMM9kIDGgnsb_yElPzIvHiV6Btfrzc0Z-m1gGvt7WsOY9vpHv7KrZ6WeRltA68Fub1FZh9QKx1gwcoi98Bm52NnGzh1WuQLbDA1V6QFm45xNEDqZzExTcf6n9ofbsb8SLVHiu2QJBEGTcZt39NvTEC_8mo1ljdqf6x5kz46iBO2uPUBBFj5iBhanPZ2xOc4LDT7WiEUxEP_srpIKXmmiMBq0jMFmqN8TeAeXNUm3umv7UlJoTBxNUvUm_BaP6Lqs7bOSbQC8ZVYNr2XDRxl7StcWlDd7KrcmMKoCUUSvigpzzbypa2gUqubptF7rJZDuxcgCl4IkSP6pIemRTtsTzhHndv96TDfMUmQQteLudQdUk','dp':'nX3tTX0g6cutqdtOIkUqexiHGpQCZbQxETqMKj12vmJ4f4jJZjU1lGH9-TLPumjdazhD1Ey_wQ1--bd5YdmpGbvpfTHZCSJxxo5kNvrjUBB6T1cIqIrzPGRvxoPZeiVBPWmaI3qfAxkVrJbPKt5gHFJjqHItCs_GaltwycHcAfQf8rmN1QX3fQiTRGiCTk9D-gR-lnl8VQpg904DzXxPsfmcCzmhaf_Mb79MugTPxTF8ohT-crATz-2QEuIM0uvD-KxqkdOWNfcl2XXbx1PtKJxxdL3e9JaWYMNuSfpH5O5F6hAh8FjMCYIYn0PNmWFjTsjKZVeThtbUHRvl7IDipw','dq':'soyZmxug009D3tKP-kS3E38QWw8I5dIs7Tkx0CT1zs2AjF984Kiw_y4wPp7dznSnieLNGRYI3t6Eb2dj7t5oHY2FPI0bzNIUbcHSP80L0YqUReoIPo3nmnMp6AuxBZHOpeCsIV_KS9zzlMyaaIlu7cIpyKKwf55NnVs5riBll1NtiqMeuJVFfOgTqoBdjD2nxL0wyqAEydDBs2-aLoqUX-vuN7m8Hz9LHM2jBClVko4F4hLJvZn8C0WowtdgDigLWKw80EO5oIr9y8MkjaLjAJBvoax6mJdup6AFXMWGkPOEnbS7S7F5MTwFW28udyi01NL_GGwFXoJXVJajC4CXbw','e':'AQAB','kty':'RSA','n':'pB9mfL5z6fjnC1iHUfsb_HZ8Gfazk7Siud8JSx6xqOZ1pjMmjfoE4WIYFMQhNoundB1dUfi1W1USdFSWkV9bLvLXg6uQzrfx5TRsBbOI4qIQFBfTfPdwHTB9gKZhBQChWCvZDnKmbg5ORi1pFFKyM-Bq9-TgeRw4bKcH1f-LddECY8Od8CqdAA1eD0DIfNBBO-3dxwoRMxvSJ78FxP9puFlZiIPGTF4tbBsfX0-LUVDpWLSa4u9x3GTa0wZgOyi4RjwJ1Kelvpzc6VumhwLVONfT7ooPkB6errpTbggEzmbZlxg4McfepYQXDT1cQ3o4qkAbiYaUbyX2F4C9mMlcgI_cwlMkNPKdHMFOeBOy1uDbnKmvDZtvVr9K67tzmqiLJt5km5nKtVrO3TvLaKydNvJ3WDdrR_qz5oNQIH4glLnLSqGmDEWhfAoccV73ABEyUcWivXlrhmUR__2Y8dly4VzZqdC0mA146HXB7RPORARZxXrTAHnArDlqWTwXNRUDk5BM1fFNCBY9LLtpdhhn0UiJS3JSHLtYHC0awoCNyRTEo28-HZ07CnR6L4bDCkGm3g38JmTiTOWXcf-9IBElWSem_VDVB84JsBWm5FDeEeO4A7K5Ehtt24eaerh_951N8bTpnhXMYzduSt_ibTaYmOAwO7KZmWHgqPTe5ndfimE','p':'xcW_yZVSmB1ZsK_X4f7-QoosCPFc1JYnR0Q23TmRYsrxuwxe5t0UGEThmSQLHIlnrD8Jnzjn8tcndXfa9kmudkU_nJ3Njyq77Mq303S4yQ4nSkbmg-Ft7aSYr3f8u6sn40nvQj_FSGWj-ddkpjKHY2XCX-MQ-15ZQaz4NiCv-tIZ4l6vD7Nh7aocTuATQxPBPR3mhF-WboxUUdWLrKuSXn3FScxAtU3N5UPdzpyYF5cxkL9EnjJjoCh_a8ES9Cgel144oJvAan1jFYcXejj91uReexvObghD4qzsywVt8oBnZ3798nIMhQpxpEOeQQdgPykT0qBWUxi96UnsGLJt3w','q':'1HFxGbZQprkFajwIjj3PthyLpTfhDN_dWA0e4YKRUFw1FeB8L-8znLPmiMvZon2LLYtdqiJkjXRe2_TOJTOni3Y8jPKhVKXkO99ykZcxTQc3jmb-aJexQEIOBej2L23ST8d-SGuL19JgJoVgQ4omgdC7EtugshglWOtH3UgzYjl-_6xPfi4uDNzhRthVBMA6m_2XHn9Q_qU5N_81RvqD7DPESOTx7TrYoTkReqP4OoDdOyLEwfAfLr_1oO_CrRJQdHzv5eapBP_Ie2j9BkICdfJdU4r9oUetkGJLTkRKTORW6u3PFI0SNVVW1Jj-F2SGayp3Nfg6YwqJHIqXvSCPvw','qi':'t2IYaNw8fRQ1LHatnu7SJdM7NbaI82L4tp7Mcfw4t9XnAobPAE_0S1tKoz7pNk9r1q5QTE2NSA7o0S_tONTT3wocz1BKJyZJqWNwBpspLQzjCRedFymJPg33OrWuTjq1yMTkkvHMH0WEZoq6jiozqY_mGDb4lDJ8XfAZUFBXT49UpwahFBdRk3xOycQJ0UjTbg2hDcmFX-ThBLWmKIkuShzd6UDXqfMSAy1RF10IDH7cxIBpRozQOQvh5fYSikYV6d9wYVPj59OTqLQEsUPMlrqXz4NcV99PnqAofpj95JAfyZOfKfETAkPg0eXCY9uKCmhQZ4o7lSqOGxN49HciZQ','kid':'8146671A52E5AFEC2F66DC82B378AF27'}";

        // NOTE: This client_id is only to be used for this particular sample. Your application will use it's own client_id.
        private const string _clientId = "41e80943-00af-4f24-a04b-5f1ca2ebd9a4";
        
        private const string _scopes = "nhn:helseid-public-samplecode/client-credentials";

        // You can use the sample API located at
        // https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI
        private const string _apiUrl = "https://localhost:57611/api/client_credentials";

        /// <summary>
        /// Simple sample demonstrating client credentials
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            const string stsUrl = "https://helseid-sts.test.nhn.no/";

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

            //var roClaimValue = new { 
            //    type = "helseid_authorization",
            //    practitioner_role = new
            //    {
            //        organization = new
            //        {
            //            identifier = new
            //            {
            //                system = "urn:oid:2.16.578.1.12.4.1.2.101",
            //                type = "ENH",
            //                value = "999977776"
            //            }
            //        }
            //    }
            //};

            //var amkContextValue = new
            //{
            //    type = "helseid://claims/external/amk-context",
            //    value = new
            //    {
            //        roles = new[]
            //        {
            //            new { name = "role1"},
            //            new { name = "role2"},
            //            new { name = "role3"},
            //        }
            //    }
            //};

            //var kjOrg = new
            //{
            //    type = "helseid://claims/external/nilar-org",
            //    value = new
            //    {
            //        parentOrg = "999888777",
            //        childOrg = "999777666"
            //    }
            //};


            //var serialized = JsonConvert.SerializeObject(new object[] {  roClaimValue});    

            //var authDetailsClaim = new Claim("authorization_details", serialized, "json");

            //claims.Add(authDetailsClaim);


            var token = Jwt.Generate(clientId,
                disco.TokenEndpoint,
                GetSecurityKey(),
                claims
                );

            return token;
        
        }


        private static SecurityKey GetSecurityKey()
        {
            return new JsonWebKey(File.ReadAllText(@"C:\temp\jwktemp.json"));
            // TODO: Store the RSA key in a secure location!!
            //const string rsaPrivateKey = "<RSAKeyValue><Modulus>sHNMAYJkqAj9970orrqHgjPD0l+PgqVnureLgOvYffUs0NzkQXAlg1L8Kj3eZkldVdW7aTUnvBDtJfw/Ad0XxH00OkV9Lha9ewpJAGchz/bIp6j+GkzYajys6du9d8MJg8VQY3X+9MTjtAH6Kf1wzXE+7fRGFT2PkN/DedwT2KDTwWNOYk9uILka4QLrzonu2TL2Hme82fn744JuPIsV7DTJ9zEoxD2dziywsFz0Rg4KNNQaL+O4HI9tuQx9ivO7hdcgGOy4lCI2U8Kf27O3txW/Jkh7SMgpGL3k+Xb+uvQKuSgeqtpQublm78A3c1vLqepGD4ccuCZ+XSCzqKCn/4CmpFRL8psT1WsWGYbuCU4Ih18viKgOqxaFOHgOC4NnRR0FoUXBdPK1Q3HLHGoPoUV47PNDaasJRVwZWBA7MQICC2mrvPpkcmoDwrsAAcGvY9YOb2e04tMjHvTYUsP+9pk1kfy1N/3hjWCpJDX8i44pWD6eSmTkpQX9lK6HigEPq414tOd4EzfxcNRAfMkg/OKLkaORdG1WKsrOoo8pCPUqTdq72JhYFJ0/vYDzRIWAphm9VM0KDP5lnK2fXku5Kn+5m8u6NJHWxBlm47OEhyWo6r1Z9hdvhXUREti+RnqQsqRzeDn46XCwuKaVIhrShXoViiiFs82DslMDDExUjm0=</Modulus><Exponent>AQAB</Exponent><P>yubldftOSBBcQEXizaxjK2aHwnGOiz4obUT9+mepWe1G/Ev3iG627rA8l2+MSvP/DJEyhypDk5sx7BLpW4oQqBJcUoigaD13OWuUQe52vDcTQlkTrAPSS0xOODISEJ4nAgzAPgoYDvcCYDF61S83LMudQnwmxgdkpkspcfbgiZeNFCPo3W2CKh2GXuvNpk9XDmJ72Vl9g9+rTJl6P2XnjHBy5knSBKWDJI3Zt+waBoQzAkgjsAi9wncc7rxx/eurwp8B7lqoX/Nne+oHZZ3OvRHn5ht10r3qsyQEUfz/TQ74li17IS5o0Sqf5jSFaBUUkhGJiu2AsTkuv2nPtYEyIw==</P><Q>3qBRpO/614MI8zuSl7RvIIaFW+HLNXf3dWC2h32WFLD384BzjD3avyjeTSWsGV+poVevpixnwM7KGK4FtKakynSKHPeIa8twcE+4kOIIVjmwbz4zGOW81Mnfvh8Ee1iLKP81IsaG+nPAZKkTbE5hjEvCP8bLb1gRbNjWOAc+mtPUx4WSjUoTcdbPY3ktO7ZSTD8tsdJ2sTN2ZEwdQ22+BftFTxcOC1J+rAbDeIkk31V2Hf0a8V9RZK15I8jUxH4EtErZ018Ay+tG+tegVSzKcsyyfx1FfHLwqcASfNT1JMS3iFZ7LEacN/IK3drnBhu/d5NCvFOWhHePbFrJHHbeLw==</Q><DP>xqtyviUFL1alnWFQhCZpK9PG1kMuWXTRTLyjGo5pqd3FBcC0bOhLQkdZ7MWSTsm+T+XT3bkqVds99HNH/xOe35Kqxz10It0cYiLOFgiSRhR/TRW/R0yumn/qjuen/JF+jGlDyvtDN1PxBZMtPJRwp/Hu12yM4pXWnWU2/ZnHnbHAt5m5pyZUrzwdl8+3m0JQcYtIzTbsyTU2m1gj9POo10A7oPVjKJ2PXTlvlsEdcof7Eh7korbMZx8OO0xVKVWa5oOe9m3aM6k3CIPMHll4VnSz5gG5SlIe/q0jdcwNhrxD93gs+f5hL31W96cxgQozDBsT2+5VdjIRbecDNCt+lQ==</DP><DQ>Q5X4M1KHnJWzSeR0BIpKkl1EbziFMJ5TCddqkoeV4II5RDti2NiOaCpIErO1I57fKJQuRwyEEwy0Xfm20bklnjDzHQgo6lDAudf5+EImtcadwafoa06TnSYMPvO7sJaY6MFRqFUM9UvexLBvrRm+k5EMT8BSUmMyJxFNN4U7hFV663epnis27ACCxXgsO0yGf49OmAWE8xbkgl55I9dVMQuvZutg4B8TRbZn8VfxUbvoOAJ3A4AkfaQMesilj2GSnAl9R6Y337B1xAFiM3l9nIx4RA7m4XkjhuVAt5UPNzJhZYqbqj1lf7aDhgbGzBvwbKTQRcw6jcyeRg7przKHEQ==</DQ><InverseQ>C8z88QY93r/05id2daU9obsIEe7R1bjUHNj+3rKo8T+L8PUoXuWQTm/NsQryoSgi5/JwZL7gyh7IQDPDFbf4jWg6nZ8ZfJs0Qisjih3cPjPMIxYvi0bG38Z1RECysNqBDTNrULMHIScOA+BhvnSPoXGQU8vJTO4yjH7V4wFcE6J9qcPPAUSy/KtgWRd91JWH/oX7PUgUDMVWc3hQ8RTyPCl60G7pFjeKhSqhRzfXIF45AmfSlOTY2l9aO1swp/cQsebym96AkYA71q2c+08KZvERvUuS0FGpZ7VQSgZ+sUe8WZb9XzJXdirtuU/sz74BFwTiT9YkoGC9hH1aMDBiFA==</InverseQ><D>sGNxtYiN6tSiXUeBJbpdwDDTLrhMlAOZgDP/hu89Sh0PofNPUoMzXOZWIjwa2RG59hZk9LUodX5OM0zIB6rnGYs37JCOpMYiwJ71fyuZx3Uh/UiYS95J8VmaWWVLMC+OkWVsCSFpr3IrVkUruVIbs6PjjqhEbvNNUzv9AxKX3FRZmtcVAn34z0l7rzfmVl/YntOs6ZQ2W4jk3vgCDw/S6H+U7kD8ScB2wiY2svcZUfazCUCGtRzlbdeLjhMIZSFlclQtR/1MPvk8adsDRvOPUbyxiyml5IoDWzJpdWAZIPbYyWNr1MvNKvxGBKGYTP+UxtTlGJyufwAsDhikwItpo/2q8tsK4CIPsR4+vxyzOCwpH7s64MBv6Sf/5sDq46hblIscyCmkgdTSaM9Q8hMxZz7LxZk6IsQhE4X3YW+jCwXRypMzgTQfCsNRLFzhMbjR1/DcFYk3zQIOi9NFiVNSIGYnvuCwUp7/KWc7yvfQ+Bs7jfVtMui+MMKf87HHVUYgDXNsfkFRg7+t86OUVXWcAZK6p0PMI7MagDyglGTN7z5E2v+jwxNBR0nGP9V3RVPl8LnJ7A/OLh1CacIfASxIgOvSEl1tUeyrZaVjnGH2LAUrK8oN3d9TlWH5hjK9RPxrRdxyIuU2q9tHS+IIXCaotOJ8MbTBi/DR9zRL39CQKZk=</D></RSAKeyValue>";
            //var rsa = RSA.Create();
            //rsa.FromXmlString(rsaPrivateKey);

            //return new RsaSecurityKey(rsa.ExportParameters(true));
        }
    }
    public class Jwt
    {
        private static readonly int TokenLifeTimeSeconds = 60;

        /// <summary>
        /// Generates a new JWT
        /// </summary>
        /// <param name="clientId">The OAuth/OIDC client ID</param>
        /// <param name="audience">The Authorization Server (STS)</param>
        /// <param name="securityKey"></param>
        /// <param name="extraClaims">Additional claims to add to the jwt</param>
        /// <returns></returns>
        public static string Generate(string clientId, string audience, SecurityKey securityKey, List<Claim> extraClaims = null)
        {
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            var payload = CreatePayload(clientId, audience, extraClaims);
            var header = new JwtHeader(signingCredentials);
            UpdateJwtHeader(securityKey, header);

            JsonExtensions.Serializer = o => {
                return Newtonsoft.Json.JsonConvert.SerializeObject(o);
            };


            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(new JwtSecurityToken(header, payload));
        }

        private static JwtPayload CreatePayload(string clientId, string audience, List<Claim> claims = null)
        {
            var payload = new JwtPayload(
               clientId,
               audience,
               null,
               DateTime.UtcNow,
               DateTime.UtcNow.AddSeconds(TokenLifeTimeSeconds));

            if (claims == null)
            {
                return payload;
            }

            var jsonClaims = claims.Where(x => x.ValueType == "json");
            var normalClaims = claims.Except(jsonClaims);

            payload.AddClaims(normalClaims);

            return AddJsonToPayload(payload, jsonClaims);
        }

        // We need to handle json objects ourself as this isn't directly supported by JwtSecurityHandler yet
        private static JwtPayload AddJsonToPayload(JwtPayload payload, IEnumerable<Claim> jsonClaims)
        {
            try
            {
                var jsonTokens = jsonClaims.Select(x => new { x.Type, JsonValue = JRaw.Parse(x.Value) }).ToArray();

                var jsonObjects = jsonTokens.Where(x => x.JsonValue.Type == JTokenType.Object).ToArray();
                var jsonObjectGroups = jsonObjects.GroupBy(x => x.Type).ToArray();
                foreach (var group in jsonObjectGroups)
                {
                    if (payload.ContainsKey(group.Key))
                    {
                        throw new Exception(string.Format("Can't add two claims where one is a JSON object and the other is not a JSON object ({0})", group.Key));
                    }

                    if (group.Skip(1).Any())
                    {
                        // add as array
                        payload.Add(group.Key, group.Select(x => x.JsonValue).ToArray());
                    }
                    else
                    {
                        // add just one
                        payload.Add(group.Key, group.First().JsonValue);
                    }
                }

                var jsonArrays = jsonTokens.Where(x => x.JsonValue.Type == JTokenType.Array).ToArray();
                var jsonArrayGroups = jsonArrays.GroupBy(x => x.Type).ToArray();
                foreach (var group in jsonArrayGroups)
                {
                    if (payload.ContainsKey(group.Key))
                    {
                        throw new Exception(string.Format("Can't add two claims where one is a JSON array and the other is not a JSON array ({0})", group.Key));
                    }

                    var newArr = new List<JToken>();
                    foreach (var arrays in group)
                    {
                        var arr = (JArray)arrays.JsonValue;
                        newArr.AddRange(arr);
                    }

                    // add just one array for the group/key/claim type
                    payload.Add(group.Key, newArr.ToArray());
                }

                var unsupportedJsonTokens = jsonTokens.Except(jsonObjects).Except(jsonArrays);
                var unsupportedJsonClaimTypes = unsupportedJsonTokens.Select(x => x.Type).Distinct();
                if (unsupportedJsonClaimTypes.Any())
                {
                    throw new Exception(string.Format("Unsupported JSON type for claim types: {0}", unsupportedJsonClaimTypes.Aggregate((x, y) => x + ", " + y)));
                }

                return payload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void UpdateJwtHeader(SecurityKey key, JwtHeader header)
        {
            if (key is X509SecurityKey x509Key)
            {
                var thumbprint = Base64Url.Encode(x509Key.Certificate.GetCertHash());
                var x5c = GenerateX5c(x509Key.Certificate);
                var pubKey = x509Key.PublicKey as RSA;
                var parameters = pubKey.ExportParameters(false);
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                header.Add("x5c", x5c);
                header.Add("kty", pubKey.SignatureAlgorithm);
                header.Add("use", "sig");
                header.Add("x5t", thumbprint);
                header.Add("e", exponent);
                header.Add("n", modulus);
            }

            if (key is RsaSecurityKey rsaKey)
            {
                var parameters = rsaKey.Rsa?.ExportParameters(false) ?? rsaKey.Parameters;
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                header.Add("kty", "RSA");
                header.Add("use", "sig");
                header.Add("e", exponent);
                header.Add("n", modulus);
            }
        }

        private static List<string> GenerateX5c(X509Certificate2 certificate)
        {

            var x5C = new List<string>();

            var chain = GetCertificateChain(certificate);
            if (chain != null)
            {
                foreach (var cert in chain.ChainElements)
                {
                    var x509Base64 = Convert.ToBase64String(cert.Certificate.RawData);
                    x5C.Add(x509Base64);
                }
            }
            return x5C;
        }

        private static X509Chain GetCertificateChain(X509Certificate2 cert)
        {
            var certificateChain = X509Chain.Create();
            certificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            certificateChain.Build(cert);
            return certificateChain;
        }
    }
}
