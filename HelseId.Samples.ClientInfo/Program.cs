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

namespace HelseId.Samples.ClientInfo
{
    class Program
    {
        // Corresponding public key:
        // {     "kty": "RSA",     "e": "AQAB",     "use": "sig",     "kid": "/aZZYfn4F1RmjT8ssydGnK9/QRw=",     "alg": "RS256",     "n": "lsUj2L89bYfU-ilbOFXKgxTV1cRYhdDUmWwbulOVFLd_q51Tt2zzIeBJUqNY_-9offUM2enz9MpvAi7-UOjxGNwiKp6Ob0PLFswFhPi6Vv1mCiPx9BtIyDFSjHzYW1y3l7BmGtHyeNeF-uT-hf73Z8SCJTxhzhu29fTrSzYUF1jQ4nuWoGa2W-TJxs6OH71Sp-wsODlU99oqbjzu1AcKC24ro4xO_Sn3BDj66B_y51EpV63pBj9hKAiC_DGdEU95_TWMFOVEjvhcl3DeCKDcqbqTO-8AdUBZwQSIFe6I9NbS8QOhCYBZNsxkCPpRsmsiZ9gVAKCXckiMFH1u8G99ZQ" }

        private const string _jwkPrivateKey = @"{
    'p': '6O-Qui3cqSwYluQRkvJ0uXa38NNl0MzrXLaWWHD9vQOsQ_jVSwL9TM0xQj-AA8gMMEtzbULfqjfaoiuZ2TjTGiXMqjRmHeHjVDcLA39MgwNFjjCwAzOxxEZ7hdV81Ii6Dxquvu_BDJMHbxjZyYekKGHn1Upei9KACjMvjVC3qpk',
    'kty': 'RSA',
    'q': 'pbLYf3gWIIHlpKK7-h4-UWb_jrvOffpods_A2NrKnVM777JifzlvwxNue0raNMo0ch5yF6XUuoiaLvLH6Dh_GQ-JXSJ0nEFvUDlx6s9xDX_yeWOeA6v7G6RaKo5myiNpFoskfSC7Yj-Blf4IEZX1ENq18wrupTT78o6e0mhvVK0',
    'd': 'MfvTgYUNOWXBztmtG0Ud6U0cA02fx2VPRjnYC3KwRvj6w4tZ-MKO7FkuFc5_sAHP04pNI2VzcmE2YjPwLiS74XEBhr2gHPUNvFYRMl8_b518mboG-dWd0HjBlD_Dfq6DUy-w-g1GZJeXHHGUjBnWLbmxJ-UXy7vWcT4sT3fzOzW7igBMgN-zxIdd-4wZrMHMwRWrdDFtNuMipqi8qBGYMLIFXneutzMpMd0YwiDOQGtFQEmiH0U3nqMRYacKecT_FT5eBJ77-uoq3gcbH7ZiRZYb9Ivikpsc1kwbSDRkLYwb1ISLxR5uM6U-Mea4UWgx9BnU1yvEFX4Ruei9k6dd4Q',
    'e': 'AQAB',
    'use': 'sig',
    'kid': '/aZZYfn4F1RmjT8ssydGnK9/QRw=',
    'qi': 'fjSxi_ve1ZLsQE-gAE5IDuKBzNABUvKq5kkwK3uF91dbskUhIjwfP29OBQppZyOv_98JZubHy7_08x9MSNHZD7IZ9lo5vQxeNDDU-nvbxOcGZHbsKpgnTy-obfx_wKN4ViY4nMKnpfMKQrXvwJX55GhozMdJokge7IwzrITq3J4',
    'dp': 'dCn8qAx1DdzSynUkmn7VXSRqaOxTy0RWX98irSp0L83kG-W9IPJ1tdZiqWIXiks6YN9Pyf5eonnGS7eout6O0GxnW75T6rUa9IWatXzHgFKiXl3DeWVPUs2_jifAYBFrkFrDKK9SO94bB_mBqvI9GHJy9jhnXB13Ax8xqKzHW4k',
    'alg': 'RS256',
    'dq': 'ltZO5QLhSahV72BAtHiRjDKx0zI90EqCjB2lVQMezMa3WgVOSrhzd-aZfVzvdHzZ70St4b8Q_tlZWgGiX1AGyz5scj7qXk_mz-XrQLCkHoDprv0zG-6UAV7Ewdat1bcUc_QoPEvuqIpdIbiFidSzqSsf1OaPxg6MiAqyo6F0L2U',
    'n': 'lsUj2L89bYfU-ilbOFXKgxTV1cRYhdDUmWwbulOVFLd_q51Tt2zzIeBJUqNY_-9offUM2enz9MpvAi7-UOjxGNwiKp6Ob0PLFswFhPi6Vv1mCiPx9BtIyDFSjHzYW1y3l7BmGtHyeNeF-uT-hf73Z8SCJTxhzhu29fTrSzYUF1jQ4nuWoGa2W-TJxs6OH71Sp-wsODlU99oqbjzu1AcKC24ro4xO_Sn3BDj66B_y51EpV63pBj9hKAiC_DGdEU95_TWMFOVEjvhcl3DeCKDcqbqTO-8AdUBZwQSIFe6I9NbS8QOhCYBZNsxkCPpRsmsiZ9gVAKCXckiMFH1u8G99ZQ'
}";

        private const string _clientId = "helseid-sample-client-info";
        private const string _scopes = "helseid://scopes/client/info";
        private const string _sts = "https://helseid-sts.test.nhn.no";

        /// <summary>
        /// Simple sample demonstrating client credentials
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            try
            {
                var c = new HttpClient();
                var disco = await c.GetDiscoveryDocumentAsync(_sts);
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }

                var response = await c.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = _clientId,
                    GrantType = GrantTypes.ClientCredentials,
                    Scope = _scopes,
                    ClientAssertion = new ClientAssertion
                    {
                        Type = ClientAssertionTypes.JwtBearer,
                        Value = BuildClientAssertion(disco, _clientId)
                    }
                });

                if (response.IsError)
                {
                    throw new Exception(response.Error);
                }

                Console.WriteLine("Access token:");
                Console.WriteLine(response.AccessToken);
                Console.WriteLine();


                var clientInfoEndpointName = "clientinfo_endpoint";
                var clientInfoUrl = disco.TryGet(clientInfoEndpointName);
                if (string.IsNullOrEmpty(clientInfoUrl))
                {
                    throw new Exception($"Unable to get URL of clientInfo. Looking for '{clientInfoEndpointName}'");
                }

                var token = response.AccessToken;
                c.SetBearerToken(token);



                var clientInfo = await c.GetStringAsync(clientInfoUrl);
                Console.WriteLine("Client Info:");
                Console.WriteLine(clientInfo);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error:");
                Console.Error.WriteLine(e.ToString());
            }

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
