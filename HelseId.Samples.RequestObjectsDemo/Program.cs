using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                // These values should go into a configuration file
                const string clientId = "ro-demo";
                const string localhost = "http://localhost:8089";
                const string redirectUrl = "/callback";
                const string startPage = "/start";
                const string stsUrl = "https://helseid-sts.utvikling.nhn.no";

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
                    Scope = "udelt:test-api/api openid profile",
                    ClientId = clientId,
                    Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                    Policy = new Policy { RequireAccessTokenHash = true, RequireAuthorizationCodeHash = true, ValidateTokenIssuerName = true },
                    ResponseMode = OidcClientOptions.AuthorizeResponseMode.FormPost,
                });

                // Build the request object - this will authenticate the user and validate the child organization
                var requestObject = GetRequestObjectsPayload(clientId, stsUrl, childOrgNo);

                // Authenticate with HelseID using the request object via the system browser
                var state = await oidcClient.PrepareLoginAsync(requestObject);
                var response = await AuthorizeWithRequestObjects(localhost, redirectUrl, startPage, state);

                // Setup a client assertion - this will authenticate the organization
                // This request is done from the client to the server without using
                // a web browser
                var clientAssertionPayload = GetClientAssertionPayload(clientId, disco);
                var loginResult = await oidcClient.ProcessResponseAsync(response, state, clientAssertionPayload);

                if (loginResult.IsError)
                {
                    throw new Exception(loginResult.Error);
                }

                // The access token can now be used when calling an api
                // It contains the user id, the security level, the organization number 
                // and the child organization
                // Copy the access token and paste it at https://jwt.ms to decode it
                Console.WriteLine("Access token:");
                Console.WriteLine(loginResult.AccessToken);
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

        private static Dictionary<string, string> GetRequestObjectsPayload(string clientId, string stsUrl, string childOrgNo)
        {
            var requestObject = BuildRequestObject(clientId, stsUrl, childOrgNo);
            return new Dictionary<string, string>
                {
                    { "request", requestObject }
                };
        }

        private static void RunBrowser(string url)
        {
            // Thanks Brock! https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
            url = url.Replace("&", "^&");
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private static string BuildRequestObject(string clientId, string audience, string childOrgNo)
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
                { "value", childOrgNo }
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

            var serialized = JsonConvert.SerializeObject(authorizationDetails);

            var claims = new List<Claim>
            {
                new Claim("authorization_details", serialized, "json")
            };

            var token = Jwt.Generate(clientId, 
                audience, 
                GetRequestObjectsSecurityKey(), 
                claims);

            return token;
        }

        private static Dictionary<string, string> GetClientAssertionPayload(string clientId, DiscoveryDocumentResponse disco)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var clientAssertion = Jwt.Generate(clientId, 
                disco.TokenEndpoint,
                GetClientAssertionSecurityKey(),
                claims);

            return new Dictionary<string, string>
            {
                { "client_assertion", clientAssertion},
                { "client_assertion_type", OidcConstants.ClientAssertionTypes.JwtBearer },
            };
        }

        private static SecurityKey GetClientAssertionSecurityKey()
        {
            // TODO: Store the RSA key in a secure location!!
            // TODO: Use different keys for client assertion and request objects
            const string rsaPrivateKey = "<RSAKeyValue><Modulus>sHNMAYJkqAj9970orrqHgjPD0l+PgqVnureLgOvYffUs0NzkQXAlg1L8Kj3eZkldVdW7aTUnvBDtJfw/Ad0XxH00OkV9Lha9ewpJAGchz/bIp6j+GkzYajys6du9d8MJg8VQY3X+9MTjtAH6Kf1wzXE+7fRGFT2PkN/DedwT2KDTwWNOYk9uILka4QLrzonu2TL2Hme82fn744JuPIsV7DTJ9zEoxD2dziywsFz0Rg4KNNQaL+O4HI9tuQx9ivO7hdcgGOy4lCI2U8Kf27O3txW/Jkh7SMgpGL3k+Xb+uvQKuSgeqtpQublm78A3c1vLqepGD4ccuCZ+XSCzqKCn/4CmpFRL8psT1WsWGYbuCU4Ih18viKgOqxaFOHgOC4NnRR0FoUXBdPK1Q3HLHGoPoUV47PNDaasJRVwZWBA7MQICC2mrvPpkcmoDwrsAAcGvY9YOb2e04tMjHvTYUsP+9pk1kfy1N/3hjWCpJDX8i44pWD6eSmTkpQX9lK6HigEPq414tOd4EzfxcNRAfMkg/OKLkaORdG1WKsrOoo8pCPUqTdq72JhYFJ0/vYDzRIWAphm9VM0KDP5lnK2fXku5Kn+5m8u6NJHWxBlm47OEhyWo6r1Z9hdvhXUREti+RnqQsqRzeDn46XCwuKaVIhrShXoViiiFs82DslMDDExUjm0=</Modulus><Exponent>AQAB</Exponent><P>yubldftOSBBcQEXizaxjK2aHwnGOiz4obUT9+mepWe1G/Ev3iG627rA8l2+MSvP/DJEyhypDk5sx7BLpW4oQqBJcUoigaD13OWuUQe52vDcTQlkTrAPSS0xOODISEJ4nAgzAPgoYDvcCYDF61S83LMudQnwmxgdkpkspcfbgiZeNFCPo3W2CKh2GXuvNpk9XDmJ72Vl9g9+rTJl6P2XnjHBy5knSBKWDJI3Zt+waBoQzAkgjsAi9wncc7rxx/eurwp8B7lqoX/Nne+oHZZ3OvRHn5ht10r3qsyQEUfz/TQ74li17IS5o0Sqf5jSFaBUUkhGJiu2AsTkuv2nPtYEyIw==</P><Q>3qBRpO/614MI8zuSl7RvIIaFW+HLNXf3dWC2h32WFLD384BzjD3avyjeTSWsGV+poVevpixnwM7KGK4FtKakynSKHPeIa8twcE+4kOIIVjmwbz4zGOW81Mnfvh8Ee1iLKP81IsaG+nPAZKkTbE5hjEvCP8bLb1gRbNjWOAc+mtPUx4WSjUoTcdbPY3ktO7ZSTD8tsdJ2sTN2ZEwdQ22+BftFTxcOC1J+rAbDeIkk31V2Hf0a8V9RZK15I8jUxH4EtErZ018Ay+tG+tegVSzKcsyyfx1FfHLwqcASfNT1JMS3iFZ7LEacN/IK3drnBhu/d5NCvFOWhHePbFrJHHbeLw==</Q><DP>xqtyviUFL1alnWFQhCZpK9PG1kMuWXTRTLyjGo5pqd3FBcC0bOhLQkdZ7MWSTsm+T+XT3bkqVds99HNH/xOe35Kqxz10It0cYiLOFgiSRhR/TRW/R0yumn/qjuen/JF+jGlDyvtDN1PxBZMtPJRwp/Hu12yM4pXWnWU2/ZnHnbHAt5m5pyZUrzwdl8+3m0JQcYtIzTbsyTU2m1gj9POo10A7oPVjKJ2PXTlvlsEdcof7Eh7korbMZx8OO0xVKVWa5oOe9m3aM6k3CIPMHll4VnSz5gG5SlIe/q0jdcwNhrxD93gs+f5hL31W96cxgQozDBsT2+5VdjIRbecDNCt+lQ==</DP><DQ>Q5X4M1KHnJWzSeR0BIpKkl1EbziFMJ5TCddqkoeV4II5RDti2NiOaCpIErO1I57fKJQuRwyEEwy0Xfm20bklnjDzHQgo6lDAudf5+EImtcadwafoa06TnSYMPvO7sJaY6MFRqFUM9UvexLBvrRm+k5EMT8BSUmMyJxFNN4U7hFV663epnis27ACCxXgsO0yGf49OmAWE8xbkgl55I9dVMQuvZutg4B8TRbZn8VfxUbvoOAJ3A4AkfaQMesilj2GSnAl9R6Y337B1xAFiM3l9nIx4RA7m4XkjhuVAt5UPNzJhZYqbqj1lf7aDhgbGzBvwbKTQRcw6jcyeRg7przKHEQ==</DQ><InverseQ>C8z88QY93r/05id2daU9obsIEe7R1bjUHNj+3rKo8T+L8PUoXuWQTm/NsQryoSgi5/JwZL7gyh7IQDPDFbf4jWg6nZ8ZfJs0Qisjih3cPjPMIxYvi0bG38Z1RECysNqBDTNrULMHIScOA+BhvnSPoXGQU8vJTO4yjH7V4wFcE6J9qcPPAUSy/KtgWRd91JWH/oX7PUgUDMVWc3hQ8RTyPCl60G7pFjeKhSqhRzfXIF45AmfSlOTY2l9aO1swp/cQsebym96AkYA71q2c+08KZvERvUuS0FGpZ7VQSgZ+sUe8WZb9XzJXdirtuU/sz74BFwTiT9YkoGC9hH1aMDBiFA==</InverseQ><D>sGNxtYiN6tSiXUeBJbpdwDDTLrhMlAOZgDP/hu89Sh0PofNPUoMzXOZWIjwa2RG59hZk9LUodX5OM0zIB6rnGYs37JCOpMYiwJ71fyuZx3Uh/UiYS95J8VmaWWVLMC+OkWVsCSFpr3IrVkUruVIbs6PjjqhEbvNNUzv9AxKX3FRZmtcVAn34z0l7rzfmVl/YntOs6ZQ2W4jk3vgCDw/S6H+U7kD8ScB2wiY2svcZUfazCUCGtRzlbdeLjhMIZSFlclQtR/1MPvk8adsDRvOPUbyxiyml5IoDWzJpdWAZIPbYyWNr1MvNKvxGBKGYTP+UxtTlGJyufwAsDhikwItpo/2q8tsK4CIPsR4+vxyzOCwpH7s64MBv6Sf/5sDq46hblIscyCmkgdTSaM9Q8hMxZz7LxZk6IsQhE4X3YW+jCwXRypMzgTQfCsNRLFzhMbjR1/DcFYk3zQIOi9NFiVNSIGYnvuCwUp7/KWc7yvfQ+Bs7jfVtMui+MMKf87HHVUYgDXNsfkFRg7+t86OUVXWcAZK6p0PMI7MagDyglGTN7z5E2v+jwxNBR0nGP9V3RVPl8LnJ7A/OLh1CacIfASxIgOvSEl1tUeyrZaVjnGH2LAUrK8oN3d9TlWH5hjK9RPxrRdxyIuU2q9tHS+IIXCaotOJ8MbTBi/DR9zRL39CQKZk=</D></RSAKeyValue>";
            var rsa = RSA.Create();
            rsa.FromXmlString(rsaPrivateKey);

            return new RsaSecurityKey(rsa.ExportParameters(true));
        }

        private static SecurityKey GetRequestObjectsSecurityKey()
        {
            // TODO: Store the RSA key in a secure location!!
            // TODO: Use different keys for client assertion and request objects
            const string rsaPrivateKey = "<RSAKeyValue><Modulus>sHNMAYJkqAj9970orrqHgjPD0l+PgqVnureLgOvYffUs0NzkQXAlg1L8Kj3eZkldVdW7aTUnvBDtJfw/Ad0XxH00OkV9Lha9ewpJAGchz/bIp6j+GkzYajys6du9d8MJg8VQY3X+9MTjtAH6Kf1wzXE+7fRGFT2PkN/DedwT2KDTwWNOYk9uILka4QLrzonu2TL2Hme82fn744JuPIsV7DTJ9zEoxD2dziywsFz0Rg4KNNQaL+O4HI9tuQx9ivO7hdcgGOy4lCI2U8Kf27O3txW/Jkh7SMgpGL3k+Xb+uvQKuSgeqtpQublm78A3c1vLqepGD4ccuCZ+XSCzqKCn/4CmpFRL8psT1WsWGYbuCU4Ih18viKgOqxaFOHgOC4NnRR0FoUXBdPK1Q3HLHGoPoUV47PNDaasJRVwZWBA7MQICC2mrvPpkcmoDwrsAAcGvY9YOb2e04tMjHvTYUsP+9pk1kfy1N/3hjWCpJDX8i44pWD6eSmTkpQX9lK6HigEPq414tOd4EzfxcNRAfMkg/OKLkaORdG1WKsrOoo8pCPUqTdq72JhYFJ0/vYDzRIWAphm9VM0KDP5lnK2fXku5Kn+5m8u6NJHWxBlm47OEhyWo6r1Z9hdvhXUREti+RnqQsqRzeDn46XCwuKaVIhrShXoViiiFs82DslMDDExUjm0=</Modulus><Exponent>AQAB</Exponent><P>yubldftOSBBcQEXizaxjK2aHwnGOiz4obUT9+mepWe1G/Ev3iG627rA8l2+MSvP/DJEyhypDk5sx7BLpW4oQqBJcUoigaD13OWuUQe52vDcTQlkTrAPSS0xOODISEJ4nAgzAPgoYDvcCYDF61S83LMudQnwmxgdkpkspcfbgiZeNFCPo3W2CKh2GXuvNpk9XDmJ72Vl9g9+rTJl6P2XnjHBy5knSBKWDJI3Zt+waBoQzAkgjsAi9wncc7rxx/eurwp8B7lqoX/Nne+oHZZ3OvRHn5ht10r3qsyQEUfz/TQ74li17IS5o0Sqf5jSFaBUUkhGJiu2AsTkuv2nPtYEyIw==</P><Q>3qBRpO/614MI8zuSl7RvIIaFW+HLNXf3dWC2h32WFLD384BzjD3avyjeTSWsGV+poVevpixnwM7KGK4FtKakynSKHPeIa8twcE+4kOIIVjmwbz4zGOW81Mnfvh8Ee1iLKP81IsaG+nPAZKkTbE5hjEvCP8bLb1gRbNjWOAc+mtPUx4WSjUoTcdbPY3ktO7ZSTD8tsdJ2sTN2ZEwdQ22+BftFTxcOC1J+rAbDeIkk31V2Hf0a8V9RZK15I8jUxH4EtErZ018Ay+tG+tegVSzKcsyyfx1FfHLwqcASfNT1JMS3iFZ7LEacN/IK3drnBhu/d5NCvFOWhHePbFrJHHbeLw==</Q><DP>xqtyviUFL1alnWFQhCZpK9PG1kMuWXTRTLyjGo5pqd3FBcC0bOhLQkdZ7MWSTsm+T+XT3bkqVds99HNH/xOe35Kqxz10It0cYiLOFgiSRhR/TRW/R0yumn/qjuen/JF+jGlDyvtDN1PxBZMtPJRwp/Hu12yM4pXWnWU2/ZnHnbHAt5m5pyZUrzwdl8+3m0JQcYtIzTbsyTU2m1gj9POo10A7oPVjKJ2PXTlvlsEdcof7Eh7korbMZx8OO0xVKVWa5oOe9m3aM6k3CIPMHll4VnSz5gG5SlIe/q0jdcwNhrxD93gs+f5hL31W96cxgQozDBsT2+5VdjIRbecDNCt+lQ==</DP><DQ>Q5X4M1KHnJWzSeR0BIpKkl1EbziFMJ5TCddqkoeV4II5RDti2NiOaCpIErO1I57fKJQuRwyEEwy0Xfm20bklnjDzHQgo6lDAudf5+EImtcadwafoa06TnSYMPvO7sJaY6MFRqFUM9UvexLBvrRm+k5EMT8BSUmMyJxFNN4U7hFV663epnis27ACCxXgsO0yGf49OmAWE8xbkgl55I9dVMQuvZutg4B8TRbZn8VfxUbvoOAJ3A4AkfaQMesilj2GSnAl9R6Y337B1xAFiM3l9nIx4RA7m4XkjhuVAt5UPNzJhZYqbqj1lf7aDhgbGzBvwbKTQRcw6jcyeRg7przKHEQ==</DQ><InverseQ>C8z88QY93r/05id2daU9obsIEe7R1bjUHNj+3rKo8T+L8PUoXuWQTm/NsQryoSgi5/JwZL7gyh7IQDPDFbf4jWg6nZ8ZfJs0Qisjih3cPjPMIxYvi0bG38Z1RECysNqBDTNrULMHIScOA+BhvnSPoXGQU8vJTO4yjH7V4wFcE6J9qcPPAUSy/KtgWRd91JWH/oX7PUgUDMVWc3hQ8RTyPCl60G7pFjeKhSqhRzfXIF45AmfSlOTY2l9aO1swp/cQsebym96AkYA71q2c+08KZvERvUuS0FGpZ7VQSgZ+sUe8WZb9XzJXdirtuU/sz74BFwTiT9YkoGC9hH1aMDBiFA==</InverseQ><D>sGNxtYiN6tSiXUeBJbpdwDDTLrhMlAOZgDP/hu89Sh0PofNPUoMzXOZWIjwa2RG59hZk9LUodX5OM0zIB6rnGYs37JCOpMYiwJ71fyuZx3Uh/UiYS95J8VmaWWVLMC+OkWVsCSFpr3IrVkUruVIbs6PjjqhEbvNNUzv9AxKX3FRZmtcVAn34z0l7rzfmVl/YntOs6ZQ2W4jk3vgCDw/S6H+U7kD8ScB2wiY2svcZUfazCUCGtRzlbdeLjhMIZSFlclQtR/1MPvk8adsDRvOPUbyxiyml5IoDWzJpdWAZIPbYyWNr1MvNKvxGBKGYTP+UxtTlGJyufwAsDhikwItpo/2q8tsK4CIPsR4+vxyzOCwpH7s64MBv6Sf/5sDq46hblIscyCmkgdTSaM9Q8hMxZz7LxZk6IsQhE4X3YW+jCwXRypMzgTQfCsNRLFzhMbjR1/DcFYk3zQIOi9NFiVNSIGYnvuCwUp7/KWc7yvfQ+Bs7jfVtMui+MMKf87HHVUYgDXNsfkFRg7+t86OUVXWcAZK6p0PMI7MagDyglGTN7z5E2v+jwxNBR0nGP9V3RVPl8LnJ7A/OLh1CacIfASxIgOvSEl1tUeyrZaVjnGH2LAUrK8oN3d9TlWH5hjK9RPxrRdxyIuU2q9tHS+IIXCaotOJ8MbTBi/DR9zRL39CQKZk=</D></RSAKeyValue>";
            var rsa = RSA.Create();
            rsa.FromXmlString(rsaPrivateKey);
            return new RsaSecurityKey(rsa.ExportParameters(true));
        }
    }
}