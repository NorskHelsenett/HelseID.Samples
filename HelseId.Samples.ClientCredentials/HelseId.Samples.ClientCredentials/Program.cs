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
        private const string RsaXml = "<RSAKeyValue><Modulus>p1v3lwzMuCBKyrqjkrYS444qoovenHcaV5w0yM8NMEnUCkr3UqQ0UDkgfLt1LARUTzn2/O5VKtX6nDP63q3yO7/HMY8C9HmjJG03YmSiFRpRXL2MdDKdlL+VDwcctLEs2+5+6gWUG36uKM1XyBybumefyua1Uba4guQIS8uaJXIeFbYYzqmbMD41yio682m4Yek82hy4W5qarAA0pe3GminchUQVS4hF3vI3K7s+QBUFsneiSkyYucElWs/RgYyLVqJSGTncR48Cs20LmBHZ9CKMzzK+3YZ3sGXtJg+6ttTctUiJW3+68LDqJcSqU2P78pcAp1tD1cGUOUGG6rfc/YXUAIiDQ5r9nrscaaQBBpqNlP8PNb9b+IFLT2zEwCMp4aB0llJ4lp7JCdxUPJ4WVuRWOCeRgSqfR9l51Veq802mXrmaYyvGICBnxJJrRX06NeoIftmeRYPcOnpmRBOEhl/NXQMvCmvQPapCXHpDjuDuFF1STyBqPaPjsamrGeWXUkBhhngA+uA3FXEf3ATaaMgEED1xead+n//h0CXFwBdaBkhc31h0hl2BMdIN/Zov+8O6lNyhOwLqFCHzw0Gp2pWPEskv52Qv32/d08GgCsuYH34CyIrBp7TjJ1CnFYivMPNVoPo8/KgmkNdFs2ILHKa6iq7HrPMQ28Pmx/pCXKU=</Modulus><Exponent>AQAB</Exponent><P>0pJuMuivWzvMUROWT0lDj1H2oGF9eZSpRaTGUL8Z1q3K17PmGy/+j3WMRm81XRJr+9qidPBIXhmk7LnPmN5/nl2fLMHrfm59mWXZHL5W9ncE46BkYz0iRk4vFUbNRcPDA5OIlxbWrPc+TA1rzzCPMxo80qInur9Nq1khSd2W/zG6Etx+Kp9q6Py0ylFRkP+Fc+RU11iWnFzDiA4eC0iq67IeJF/AOoc/ir1kAIhkn/qhUczK5t6ebu2N+MyVEFKSMhzZBSver6Uku08YqaDWA5j/iMPP9Aw8L40coNwxGMtjUx5MpyFhmWFdV2m8F8eCMzw514Mty1FVUpUAqPX2gw==</P><Q>y3b2cUbeQCeHkL+U1CCgPkbxU56EQLdN/y8Izq9aLLBf6OcJsfnDANCc3OMweRD20alUmxIX41gQTYJ0K9pgM7lRTuPRcWJW89xKnCsc+Bg4+hDghbLqFFgg+/6ptwfDaCTdPD+WsYQRSJe/JjiMFbe5XU+hSa7ZbspO1k869acJOO9kB/oDZnHwEAx28BkRBR7LCP/h8h6uMbte0yiBmnl5ovfyBfyICCU642WPOra9K2wmtCdaloWkmRBIyiEYao7x0AroePnqDLTOXYd4GoavqZVq50+BrRgua5+arzAqvrCWykO2kUDP3JQujFLIo5y/qOTtRI2z737DelM3tw==</Q><DP>qh+L0K2VHwyM4eQFSEFUx/HcY27gRN4KdC3P22TJp1v5yZOakNSRwa2iizVF09ASVgQpxHhsvznQuUDVrBf22yegdjSl4hu6dbiHVGWjNLSryovHDzZQ/qQj/fiZ14d1guorLIZTIqMOPbuKInaE+zBze2lu172/LnRwJJFWcQ7n2l0xwZXSdjHUjrBsSc1nMF6E/Qahh+qaPs3JECzBinL5T0HcuGyUta6VoKiRQ37l3oSqWSP6tHxQe3Yt6GYNn1cXLspmu1mc94fL0SAUSAvQR9qLpAxOg8xqGLxNHk8UDA8qtsyNYbH8C6dtQ3j4hBRgVvGwiddIK9QeGGO/qQ==</DP><DQ>a3id+f2d/bMjl2Cqw1WsbtjYNfwADZMFXupAM7RJ5FsRfhszcs/jofWPNdnHS9ubE+nmZ7ap6YslqVtj85n4wLl9ajdJ9SMlnM/alRzsw1tAFU5+2gBERpS6b4D3slcmb0cxmNZZydBhtL961zx9Oid+gPxDzIDQFwZDmE3nbcRaSbmhU9lKnH1IeaGr3WzQIa0/P7Sxa0urZVd8Yfr+YlMR3fQr4d+fFvZbYavOeQv3Zg1NcFFtNx7Gb5c7a5EJrZdtwR9R5jzT1PxYGO0qkpBcDy5+dkn3zC9+rZhzg1/k5C6wp4wWzii24uNepv4/PrTYQ+UQMurKhZGmvWFhhw==</DQ><InverseQ>xJHmtrcMnMu5Vg0FiFWfgcivU4wIANjdNbbdaBWJU/KLnHzctWnBaB9mZlydzCWYRGmmhzwAm9X0OEb1gpT6eiSgg01/pzeIeevhBkFquw3fIRgJK2nxfiDxsSQAfjHfCQ3fEfTHSXQDd+jExbw5w681VBx+45FtU69VvJmadWQ0kCJsQVMt3e11Al07gmILlU1bjvpGeb1KZ1NiCR1O75aUgdCHyRwLwSZly2XzliO7prbnc7URnMsu+HxuQ6JybSPEWX14Pcn1BH6jJkmqflKLSKT/qiISCxD/GP+L7Jeda3fAUtW2aevFKneIk1hARa842mW/H8MQJUK6BrOTmA==</InverseQ><D>YMiiqkvQqDqkhhDhP5rj2Y0Bwva4Sivmo/vF2stCiUZoxXsNBFHJnwsqanfODyKBzz9qQmNiBV+xilvVHKnjiAIkI9jckJ03Z31xpgkkYqfRnZxQeXI8ByW0AfjO9P/xPU7zPkrzl+LuvNHjjepddLMwiZpaCWNt2OQemBaqkjUoiM3CEuGqyX9wg/VgGhxtcNH9SvWI+BC0mfuUdtDHJahHyxnQZtnr7j6NAVFLcqu1m7vrsqQRPnsgKyA7vHuWqQc+CzCW3xspKLJLHipUrQa9/6UNE/cLiIupVXWLOOhoqr3EEZIQfdkRz72n8onDzkrdKetxk1Bbc7EdYOfreBJX7Udh0cJ7bAW0gdzDinCUnu3iYOb8lnptcVlIKsXIfX1+1PoK2NOHhIX5NkhzjA0WkmYZc6LlpAFLDcC5FeNC0zIpJWlPNeFmxVqD5LhozwHz/6HcX1ohv+oD+hbF7d8tsLWOw2ROVVFSkqyN2IOC30DGrUfJxQ7si/oDeX8T9IAledZZR8UC5XmJytdLY2Ak3tw39ZaNbFlFAAQkOSKgg29Bmw5ZPzzV74qeJIUFp9Z6afsq2kLGWTOGyiuh/qQkbBv/WlCYMtKfCHPdgtOe7vISn8lrWoyfWZ5imkhCmlNqJHnawFyGD0kB3mlZtzxM6jSzOB73VRGXAoVznRE=</D></RSAKeyValue>";
        private const string ClientId = "ro-demo";
        private const string Scope = "udelt:test-api/api";
        private const string TokenEndpoint = "https://helseid-sts.utvikling.nhn.no/connect/token";

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
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" }
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
