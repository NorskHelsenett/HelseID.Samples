using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HelseId.Samples.ClientCredentials
{
    class Program
    {
        private const string RsaXml = "<RSAKeyValue><Modulus>sHNMAYJkqAj9970orrqHgjPD0l+PgqVnureLgOvYffUs0NzkQXAlg1L8Kj3eZkldVdW7aTUnvBDtJfw/Ad0XxH00OkV9Lha9ewpJAGchz/bIp6j+GkzYajys6du9d8MJg8VQY3X+9MTjtAH6Kf1wzXE+7fRGFT2PkN/DedwT2KDTwWNOYk9uILka4QLrzonu2TL2Hme82fn744JuPIsV7DTJ9zEoxD2dziywsFz0Rg4KNNQaL+O4HI9tuQx9ivO7hdcgGOy4lCI2U8Kf27O3txW/Jkh7SMgpGL3k+Xb+uvQKuSgeqtpQublm78A3c1vLqepGD4ccuCZ+XSCzqKCn/4CmpFRL8psT1WsWGYbuCU4Ih18viKgOqxaFOHgOC4NnRR0FoUXBdPK1Q3HLHGoPoUV47PNDaasJRVwZWBA7MQICC2mrvPpkcmoDwrsAAcGvY9YOb2e04tMjHvTYUsP+9pk1kfy1N/3hjWCpJDX8i44pWD6eSmTkpQX9lK6HigEPq414tOd4EzfxcNRAfMkg/OKLkaORdG1WKsrOoo8pCPUqTdq72JhYFJ0/vYDzRIWAphm9VM0KDP5lnK2fXku5Kn+5m8u6NJHWxBlm47OEhyWo6r1Z9hdvhXUREti+RnqQsqRzeDn46XCwuKaVIhrShXoViiiFs82DslMDDExUjm0=</Modulus><Exponent>AQAB</Exponent><P>yubldftOSBBcQEXizaxjK2aHwnGOiz4obUT9+mepWe1G/Ev3iG627rA8l2+MSvP/DJEyhypDk5sx7BLpW4oQqBJcUoigaD13OWuUQe52vDcTQlkTrAPSS0xOODISEJ4nAgzAPgoYDvcCYDF61S83LMudQnwmxgdkpkspcfbgiZeNFCPo3W2CKh2GXuvNpk9XDmJ72Vl9g9+rTJl6P2XnjHBy5knSBKWDJI3Zt+waBoQzAkgjsAi9wncc7rxx/eurwp8B7lqoX/Nne+oHZZ3OvRHn5ht10r3qsyQEUfz/TQ74li17IS5o0Sqf5jSFaBUUkhGJiu2AsTkuv2nPtYEyIw==</P><Q>3qBRpO/614MI8zuSl7RvIIaFW+HLNXf3dWC2h32WFLD384BzjD3avyjeTSWsGV+poVevpixnwM7KGK4FtKakynSKHPeIa8twcE+4kOIIVjmwbz4zGOW81Mnfvh8Ee1iLKP81IsaG+nPAZKkTbE5hjEvCP8bLb1gRbNjWOAc+mtPUx4WSjUoTcdbPY3ktO7ZSTD8tsdJ2sTN2ZEwdQ22+BftFTxcOC1J+rAbDeIkk31V2Hf0a8V9RZK15I8jUxH4EtErZ018Ay+tG+tegVSzKcsyyfx1FfHLwqcASfNT1JMS3iFZ7LEacN/IK3drnBhu/d5NCvFOWhHePbFrJHHbeLw==</Q><DP>xqtyviUFL1alnWFQhCZpK9PG1kMuWXTRTLyjGo5pqd3FBcC0bOhLQkdZ7MWSTsm+T+XT3bkqVds99HNH/xOe35Kqxz10It0cYiLOFgiSRhR/TRW/R0yumn/qjuen/JF+jGlDyvtDN1PxBZMtPJRwp/Hu12yM4pXWnWU2/ZnHnbHAt5m5pyZUrzwdl8+3m0JQcYtIzTbsyTU2m1gj9POo10A7oPVjKJ2PXTlvlsEdcof7Eh7korbMZx8OO0xVKVWa5oOe9m3aM6k3CIPMHll4VnSz5gG5SlIe/q0jdcwNhrxD93gs+f5hL31W96cxgQozDBsT2+5VdjIRbecDNCt+lQ==</DP><DQ>Q5X4M1KHnJWzSeR0BIpKkl1EbziFMJ5TCddqkoeV4II5RDti2NiOaCpIErO1I57fKJQuRwyEEwy0Xfm20bklnjDzHQgo6lDAudf5+EImtcadwafoa06TnSYMPvO7sJaY6MFRqFUM9UvexLBvrRm+k5EMT8BSUmMyJxFNN4U7hFV663epnis27ACCxXgsO0yGf49OmAWE8xbkgl55I9dVMQuvZutg4B8TRbZn8VfxUbvoOAJ3A4AkfaQMesilj2GSnAl9R6Y337B1xAFiM3l9nIx4RA7m4XkjhuVAt5UPNzJhZYqbqj1lf7aDhgbGzBvwbKTQRcw6jcyeRg7przKHEQ==</DQ><InverseQ>C8z88QY93r/05id2daU9obsIEe7R1bjUHNj+3rKo8T+L8PUoXuWQTm/NsQryoSgi5/JwZL7gyh7IQDPDFbf4jWg6nZ8ZfJs0Qisjih3cPjPMIxYvi0bG38Z1RECysNqBDTNrULMHIScOA+BhvnSPoXGQU8vJTO4yjH7V4wFcE6J9qcPPAUSy/KtgWRd91JWH/oX7PUgUDMVWc3hQ8RTyPCl60G7pFjeKhSqhRzfXIF45AmfSlOTY2l9aO1swp/cQsebym96AkYA71q2c+08KZvERvUuS0FGpZ7VQSgZ+sUe8WZb9XzJXdirtuU/sz74BFwTiT9YkoGC9hH1aMDBiFA==</InverseQ><D>sGNxtYiN6tSiXUeBJbpdwDDTLrhMlAOZgDP/hu89Sh0PofNPUoMzXOZWIjwa2RG59hZk9LUodX5OM0zIB6rnGYs37JCOpMYiwJ71fyuZx3Uh/UiYS95J8VmaWWVLMC+OkWVsCSFpr3IrVkUruVIbs6PjjqhEbvNNUzv9AxKX3FRZmtcVAn34z0l7rzfmVl/YntOs6ZQ2W4jk3vgCDw/S6H+U7kD8ScB2wiY2svcZUfazCUCGtRzlbdeLjhMIZSFlclQtR/1MPvk8adsDRvOPUbyxiyml5IoDWzJpdWAZIPbYyWNr1MvNKvxGBKGYTP+UxtTlGJyufwAsDhikwItpo/2q8tsK4CIPsR4+vxyzOCwpH7s64MBv6Sf/5sDq46hblIscyCmkgdTSaM9Q8hMxZz7LxZk6IsQhE4X3YW+jCwXRypMzgTQfCsNRLFzhMbjR1/DcFYk3zQIOi9NFiVNSIGYnvuCwUp7/KWc7yvfQ+Bs7jfVtMui+MMKf87HHVUYgDXNsfkFRg7+t86OUVXWcAZK6p0PMI7MagDyglGTN7z5E2v+jwxNBR0nGP9V3RVPl8LnJ7A/OLh1CacIfASxIgOvSEl1tUeyrZaVjnGH2LAUrK8oN3d9TlWH5hjK9RPxrRdxyIuU2q9tHS+IIXCaotOJ8MbTBi/DR9zRL39CQKZk=</D></RSAKeyValue>";
        private const string ClientId = "ro-demo";
        private const string Scope = "udelt:test-api/api";
        private const string TokenEndpoint = "https://helseid-sts.test.nhn.no/connect/token";

        static async Task Main(string[] args)
        {

            var rsa = RSA.Create();
            rsa.FromXmlString(RsaXml);
            var securityKey = new RsaSecurityKey(rsa);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, ClientId),
                new Claim(JwtClaimTypes.IssuedAt, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(ClientId, TokenEndpoint, claims, DateTime.Now, DateTime.Now.AddMinutes(1), signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var clientAssertion = tokenHandler.WriteToken(token);

            var request = new ClientCredentialsTokenRequest
            {
                Address = TokenEndpoint,
                ClientId = ClientId,
                Scope = Scope,
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };

            var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);

            if(result.IsError)
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
    }
}
