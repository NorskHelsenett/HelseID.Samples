using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HelseId.RequestObjectsDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {

            try
            {
                // NOTE: In a production environment you would use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
                const string jwkPrivateKey = "{'d':'wUJFbzCvRwDlDyVyCmLmXQy0Xod81R5Cwk8U1vW2cJg1E88dQurAkgGAYcISUJKGW1haCVn-WZqmJm2WXLTjNHvGIH-sZapWqINVuwrl1FF_hQ-Cf2hRCyV8P-eU0tn_GH0gCRS2_ER5AbtDw26JfkHy9Y3ZRrL9EXjH_ZEd-7fNM_g_UelGe1a0xBLrCf80HkZaO-U10MV-iu88kqsUFb3RdIsbykGgrYVHmr87Y6K1DLYzJESQU3z_rJmubSELE6-HWw_gf1zb-FXhb1M3i1fbFlB0muom_BnbyOvOFRhGV5ngi8tyRBaz9BMbtyWLEEJzLorjz7C4iKfelefjjaUh-BinhpZ35j_Ki0aY5rwXjcyxQiciUHdfDcntzu815Rq5vu2lcL7VHz4mIp-X7Er4PfKqlrIgBp52SVJpWI1JEL8c7vA2ABGM9hqqY_Akh6YJmdMwNUpqE_Madr_cI2X3R9D0AxeGYrhwwYx41izST9X8dPrJ9X9w2UGlOCweHBi3Ok8gGIYvZzbi6cmXMkzo5J0-qCTQYDS2Lb6h4YKEVN7TQpp3PjXOfeZrc4AW8pHVjnirMoI7GGioGDMMEMA2n60I2qMmX-nyb5K5OsbWPDqMKZLBdTk5mfNfQvQy6cF5BR25QjxFGvXH0ThWjFaUWCgSBa0O3azzkcG1-b0','dp':'Y9CvDg6yfbZGyE-ycLasng1NLT1_cfiYkMLXa4c9TN2L7Ta1R4sBc4vX4IvZ_kSD3ubHD4q3vKSGxEawD7x-3odyrkamLZPHmkvafrkIZPZWcNqGTZAwqCpHxmodJsJIOkPWI_xn2uD2yU9aAaZEoxTqA7Oac-kLdDPjLKsvCRJcilQyD4kvodoK0n4YKINJM-taLIgsxwA7ZkH2GBlV49ZCvKLRZGERhEiunxncx4LNKUd5CBbKCBTZse8EpdhFxbzRgUxOQZujXaVOzdZB_IUkfDPFsN5Fjf6rYQmbyp1W7rUlqJQ5sqRgZngjrCMUdKCzMhVN7X34IMzUDv8ppQ','dq':'zcQhwDxvPDIPUMbIyyWw5G-tnMueZS8KlSmSTzUaLC9QDOU9RbSK6SbNSP-zahG13IJwtAJ9TqYlmj9AbVpdrXtctdcZkUn_wX5P-Qz6J_w-0xODjM_YB3ma_qYsh4yYoEm4lgS0JZN2F7fnc1rOXpzBjm-jsJyRbulD1K908dy6ui9kuO4FfoWpXwX7Jixqfz55koIm1umSVjwI6otz7NOLd8gZcapeBFPrCZnCMIVrGBU0DDVAolEF2GMps4a3259VYPYPkDDs7lwSY8Jk1jFUmdkqwXLwaDzlgnpJJI2boHMOxt1QAuMdToLphah5R3HNLh1NTrLQn0Uehiazhw','e':'AQAB','kty':'RSA','n':'4e76k7QF01kw3hhdHyc3iyUn6c465yrLD9KV7m4gNIc3Tm66Iq3P72xq9w5abAXJG9GJUkbhtTG5isFmXLCU4MOlU5T2a7iBqx2dmao72LSsQ7WZQVtIv5JvWYAXpd4rlgTaJfO3Unv9Tn8v5wWgL9ZbplVmR_GWMY5l-7i44PWwLwGZge_KUVGQmKtx7XXsnezG4JfEAPJbO9zfD4CH6AGtRdcAn2r-2-jqk_-uU1BVoWwDdrCJ_DKOyYNDUfkRneTATY5RDdH5flNd-19XW31L1q3dTMqHbcMzFMfiqwyBYX5kFJrDT-W7poIex7jhZfA5by8K1tfwJqdYRGH4Qp5QtBTs76iuANNBUz3tO0vmV8bYez2AWegvRqrZGHPsMPtA5pCjXw1rueJdH5WqpCHrBsnNkOHNVcd8yHLPmXRokwi3cSanOuquLOI5Qh-pqeuTTJAv8QQ3X5aQRnHmZoyVuOP9Qqq05MGRPp6W-7Vdbi6mslDP-FwUnkHb2C-XenxHesqfcbqBOELa-PD6Fj_usVKPcL9HR_J4IK38XFFOT669_Xhpyaq6iRtvlmj1n_fQNvRcGpZIfIAFgf64cIwLAz2vimj5ywXneyDIRv5Wge8VyhfsAe9S01x0dNq-aR16clayKDn48e6fETeTWJJaPK7lvi1-Oc-tlaA7Pfk','p':'8fDDtrup_sHpmpnAQ6arzA6S2zG23OlRwsrPQu1bByDJSAB6Y0RQDIxQB06NjNCyyoetpiUlblgpcLQpy-R9xAgdvw-gcxNYW4hxkQdbnSP0U9vv8V3tGtxN8szjBEsNz0vhs0Gc4Wz01IYGVY6OJhOg3qshAFrjZie49MsIRw_w0dJA6UAXbx19fvoFtKDSyp-w2FnFQHMnzwsoJULiCPB6R77XJZLx3gyQN4ad-s63E7f89055JNIO8rt-cigkL_ilQh-x9m-oX_nBWb_N4gjVdnyoH2Vjlq2os2l209URLPl4bq6AMCfe7SAnuyLA9U_aTiLOF2eAlbgznmUWjw','q':'7xAWBXurm-Qv60KwKF2dqkGbCyNTrQv1Ep8L2ArTVeTMcQb54dQiwLZb-NDaiRIAyBXOsrkWEyh8tru5fvicn-EdAnajGTxEwwKktaUA-ufDw8vy4HOfATXGuA0DFt1L6nZ_n9lFYxd09iEUj0GwvaRM21A7nbOz3Qf6JThCaP8JOK3doOYsr7fufD8E9gcE7CiTcbbpm8BOa6-h2fY3tENCOV76a9_G2RfPOrvIu-tgVPU_2K-r8vSWQobXhGELOYc0XAipUxEAPxGLOq3T-OTQk2SvPnom2mEG_-5V1PG1sxH0WGf53XpUjjsUW1Zj9bidmuhLPqbMBf-ZhbLm9w','qi':'3h-0vyBkKf0WsPhCiIWXm8yPU-J_qgM4JjkrBzwWOU6CpnQyIdoSfStXhas7ehoODnb7sAQ2PEh4RBls2kJrXmzqC_0wsUEFdnfSERsf8eb8Sgu1NF5JgqRU9UBe9-4bvLZAcFpBG_PJyMg8QhCEuqFbjrjNjg9r3EVfeRHyjl7dPEGF5RwIUiGbkyLmXmcqf0L4GhuvqGdd56krht_kvwI-lppMB4OoZDgwfOH0fcKuHJUyc0RyfZ-iS9YZTQI-1AO2gA-RsjCjvtZB2QBdhvqPp2OujkYlcGXukSBtoBsU3elGBqFPRqlPsDMN8dj0bw_xjNxue7fKh9a68CPedA','kid':'B2C61A07EE0661237D19BEE1E0A1463C'}";
                // Corresponding public key is: {'e':'AQAB','kty':'RSA','n':'4e76k7QF01kw3hhdHyc3iyUn6c465yrLD9KV7m4gNIc3Tm66Iq3P72xq9w5abAXJG9GJUkbhtTG5isFmXLCU4MOlU5T2a7iBqx2dmao72LSsQ7WZQVtIv5JvWYAXpd4rlgTaJfO3Unv9Tn8v5wWgL9ZbplVmR_GWMY5l-7i44PWwLwGZge_KUVGQmKtx7XXsnezG4JfEAPJbO9zfD4CH6AGtRdcAn2r-2-jqk_-uU1BVoWwDdrCJ_DKOyYNDUfkRneTATY5RDdH5flNd-19XW31L1q3dTMqHbcMzFMfiqwyBYX5kFJrDT-W7poIex7jhZfA5by8K1tfwJqdYRGH4Qp5QtBTs76iuANNBUz3tO0vmV8bYez2AWegvRqrZGHPsMPtA5pCjXw1rueJdH5WqpCHrBsnNkOHNVcd8yHLPmXRokwi3cSanOuquLOI5Qh-pqeuTTJAv8QQ3X5aQRnHmZoyVuOP9Qqq05MGRPp6W-7Vdbi6mslDP-FwUnkHb2C-XenxHesqfcbqBOELa-PD6Fj_usVKPcL9HR_J4IK38XFFOT669_Xhpyaq6iRtvlmj1n_fQNvRcGpZIfIAFgf64cIwLAz2vimj5ywXneyDIRv5Wge8VyhfsAe9S01x0dNq-aR16clayKDn48e6fETeTWJJaPK7lvi1-Oc-tlaA7Pfk','kid':'B2C61A07EE0661237D19BEE1E0A1463C'}

                // These values should go into a configuration file
                const string clientId = "helseid-sample-request-objects-console";
                const string localhost = "http://localhost:8090";
                const string redirectUrl = "/callback";
                const string startPage = "/start";
                const string stsUrl = "https://helseid-sts.test.nhn.no";

                // The child organization number is provided by the EPJ
                const string childOrgNo = "999977775";

                var disco = await new HttpClient().GetDiscoveryDocumentAsync(stsUrl);
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                // Setup the oidc client for authentication against HelseID
                var oidcClient = new OidcClient(new OidcClientOptions
                {
                    Authority = stsUrl,
                    RedirectUri = localhost + redirectUrl,
                    Scope = "openid profile offline_access nhn:helseid-public-samplecode/authorization-code",
                    ClientId = clientId,
                    Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                    Policy = new Policy { RequireAccessTokenHash = true, RequireAuthorizationCodeHash = true, ValidateTokenIssuerName = true },
                    ResponseMode = OidcClientOptions.AuthorizeResponseMode.FormPost,
                });

                // Build the request object - this will authenticate the user and validate the child organization
                var requestObject = GetRequestObjectsPayload(clientId, stsUrl, jwkPrivateKey);

                // Authenticate with HelseID using the request object via the system browser
                var state = await oidcClient.PrepareLoginAsync();
                var response = await AuthorizeWithRequestObjects(localhost, redirectUrl, startPage, state);

                // Setup a client assertion - this will authenticate the organization
                // This request is done from the client to the server without using
                // a web browser
                var clientAssertionPayload = GetClientAssertionPayload(clientId, disco, jwkPrivateKey);
                var loginResult = await oidcClient.ProcessResponseAsync(response, state, clientAssertionPayload);

                if (loginResult.IsError)
                {
                    throw new Exception(loginResult.Error);
                }

                var token = new JsonWebTokenHandler().ReadJsonWebToken(loginResult.IdentityToken);


                // The access token can now be used when calling an api
                // It contains the user id, the security level, the organization number
                // and the child organization
                // Copy the access token and paste it at https://jwt.ms to decode it
                Console.WriteLine("ID Token:");
                Console.WriteLine(loginResult.IdentityToken);
                Console.WriteLine();
                Console.WriteLine("Access token:");
                Console.WriteLine(loginResult.AccessToken);

                var rt = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    RefreshToken = loginResult.RefreshToken,
                    Address = disco.TokenEndpoint,
                    ClientAssertion = new ClientAssertion
                    {
                        Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                        Value = GetClientAssertionPayload(clientId, disco, jwkPrivateKey)["client_assertion"]
                    }
                });

                Console.WriteLine();
                Console.WriteLine("New Access Token after using Refresh Token:");
                Console.WriteLine(rt.AccessToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(ex.ToString());
            }
        }

        private static async Task<string> AuthorizeWithRequestObjects(string localhost, string redirectUrl, string startPage, AuthorizeState state)
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

        private static Dictionary<string, string> GetRequestObjectsPayload(string clientId, string stsUrl, string jwkPrivateKey)
        {
            var requestObject = BuildRequestObject(clientId, stsUrl, jwkPrivateKey);
            return new Dictionary<string, string>
                {
                   { "request", requestObject },
                   { "prompt", "login" }, // Disables single sign-on
                };
        }

        private static void RunBrowser(string url)
        {
            // Thanks Brock! https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private static string BuildRequestObject(string clientId, string audience, string jwkPrivateKey)
        {
            // Builds a request object containing the authorization details claim with
            // the following structure:
            //        "authorization_details":{
            //            "type":"helseid_authorization",
            //            "practitioner_role":{
            //                "organization":{
            //                    "identifier": {
            //                        "system":"urn:oid:2.16.578.1.12.4.1.2.101",
            //                        "type":"ENH",
            //                        "value":"[orgnummer]"
            //                    }
            //                }
            //            }
            //        }

            var orgNumberDetails = new Dictionary<string, string>
            {
                { "system", "urn:oid:2.16.578.1.12.4.1.2.101" },
                { "type", "ENH" },
                { "value", "999977775" } // Client configuration in HelseID contains a orgnumber whitelist with this number
            };

            var identifier = new Dictionary<string, object>
            {
                { "identifier", orgNumberDetails }
            };

            var organization = new Dictionary<string, object>
            {
                { "organization", identifier }
            };

            var authorizationDetails = new Dictionary<string, object>
            {
                { "type", "helseid_authorization" },
                { "practitioner_role", organization }
            };

            var serialized = JsonConvert.SerializeObject(new object[] { authorizationDetails });

            var claims = new List<Claim>
            {
                new Claim("authorization_details", serialized, "json")
            };

            var token = Jwt.Generate(clientId,
                audience,
                GetSecurityKey(jwkPrivateKey),
                claims);

            return token;
        }

        private static Dictionary<string, string> GetClientAssertionPayload(string clientId, DiscoveryDocumentResponse disco, string jwkPrivateKey)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };
        
            var clientAssertion = Jwt.Generate(clientId,
                disco.TokenEndpoint,
                GetSecurityKey(jwkPrivateKey),
                claims);
            
            return new Dictionary<string, string>
            {
                { "client_assertion", clientAssertion},
                { "client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer }
            };
        }

        private static SecurityKey GetSecurityKey(string jwkPrivateKey)
        {
            return new JsonWebKey(jwkPrivateKey);
        }
    }
}
