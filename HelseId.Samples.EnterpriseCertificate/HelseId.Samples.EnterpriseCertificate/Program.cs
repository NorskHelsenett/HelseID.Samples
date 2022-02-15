using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HelseId.Samples.EnterpriseCertificate
{
    class Program
    {
        private const string ClientId = "bcf9673e-2e77-44de-af53-b7f4e5535b99";
        private const string Scope = "udelt:test/api";
        private const string TokenEndpoint = "https://localhost:44366/connect/token";

        static async Task Main(string[] args)
        {
            var certificate = new X509Certificate2(@"GothamSykehus.p12", "bMKXs98yOizPLHVQ");
            var securityKey = new X509SecurityKey(certificate);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, ClientId),
                new Claim(JwtClaimTypes.IssuedAt, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(ClientId, TokenEndpoint, claims, DateTime.Now, DateTime.Now.AddMinutes(1), signingCredentials);

            var thumbprint = Base64Url.Encode(certificate.GetCertHash());
            var x5C = GenerateX5C(certificate);
            var pubKey = securityKey.PublicKey as RSA;
            var parameters = pubKey.ExportParameters(false);
            var exponent = Base64Url.Encode(parameters.Exponent);
            var modulus = Base64Url.Encode(parameters.Modulus);

            token.Header.Add("x5c", x5C);
            token.Header.Add("kty", pubKey.SignatureAlgorithm);
            token.Header.Add("use", "sig");
            token.Header.Add("x5t", thumbprint);
            token.Header.Add("e", exponent);
            token.Header.Add("n", modulus);

            var tokenHandler = new JwtSecurityTokenHandler();
            var clientAssertion = tokenHandler.WriteToken(token);
            clientAssertion = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkRDRjgwMTcyREZGMjEwNkQxRTAzNDM4MTgzRTczMjk0M0ZBNTM4QzEiLCJ4NXQiOiIzUGdCY3RfeUVHMGVBME9CZy1jeWxELWxPTUUiLCJ0eXAiOiJKV1QiLCJ4NWMiOlsiTUlJRkd6Q0NCQU9nQXdJQkFnSUxBajUrQytES3RpV2YzN2d3RFFZSktvWklodmNOQVFFTEJRQXdVVEVMTUFrR0ExVUVCaE1DVGs4eEhUQWJCZ05WQkFvTUZFSjFlWEJoYzNNZ1FWTXRPVGd6TVRZek16STNNU013SVFZRFZRUUREQnBDZFhsd1lYTnpJRU5zWVhOeklETWdWR1Z6ZERRZ1EwRWdNekFlRncweU1EQXpNRFl4TURJNE5EVmFGdzB5TXpBek1EWXlNalU1TURCYU1HSXhDekFKQmdOVkJBWVRBazVQTVJ3d0dnWURWUVFLREJOSVJVeFRSVkJNUVZSVVJrOVNUVVZPSUVGVE1TRXdId1lEVlFRRERCaElSVXhUUlZCTVFWUlVSazlTVFVWT0lFRlRJRlJGVTFReEVqQVFCZ05WQkFVVENUa3lNak13TnpneE5EQ0NBU0l3RFFZSktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQUtSdVY4QnNSSENMWXdKaVBXajRZMVBVVEUxUkZLWDloREpETC9RSmVkKzlKTTRYTnNQZjE4Q2F2TjhLQzB6SHp3a2V2UlN1cGpNRWxlKy94RDV4SGtzMDNtRmNPSDBXV0FZQWFuejNsNHBtRmc3WU4xa2kzUGZFOWN0T1kzeHV5RUpxWXRuRHgyQTc0aHdnMVlNY2o1cU9BNDExWTJNb1pBRW5QbkhWVkh1MlhDNEhYbUJ5VDlVdHRLZUhkdUJacDNyUlRoM3d0RkliL2cvbmtvdkxnMEs0Wi9wK2JXaEE3Uzh1Q05kNWlsdTdaMzRKZjBGNlIwaFd6REZZQUdQMlJrdjhpOS9SakhXWDBpclFHNVhxSHdoYzZRTHRQVlRIZ3VvTFhpOElVb2xyaWJKUUtEcmJQblVCc29sbnZjMTQ2ZWNYdXZiYlNjNUNOUS9MQjNiY2JETUNBd0VBQWFPQ0FlRXdnZ0hkTUFrR0ExVWRFd1FDTUFBd0h3WURWUjBqQkJnd0ZvQVVQNjcxZUF1U28zQWdOVjlhK3Zja29GSUI4RUV3SFFZRFZSME9CQllFRkdYWlFjdlhCYWl2S21NUVVjM3FrRXZOZXBub01BNEdBMVVkRHdFQi93UUVBd0lFc0RBZEJnTlZIU1VFRmpBVUJnZ3JCZ0VGQlFjREFnWUlLd1lCQlFVSEF3UXdGZ1lEVlIwZ0JBOHdEVEFMQmdsZ2hFSUJHZ0VBQXdJd2dic0dBMVVkSHdTQnN6Q0JzREEzb0RXZ000WXhhSFIwY0RvdkwyTnliQzUwWlhOME5DNWlkWGx3WVhOekxtNXZMMk55YkM5Q1VFTnNZWE56TTFRMFEwRXpMbU55YkRCMW9IT2djWVp2YkdSaGNEb3ZMMnhrWVhBdWRHVnpkRFF1WW5WNWNHRnpjeTV1Ynk5a1l6MUNkWGx3WVhOekxHUmpQVTVQTEVOT1BVSjFlWEJoYzNNbE1qQkRiR0Z6Y3lVeU1ETWxNakJVWlhOME5DVXlNRU5CSlRJd016OWpaWEowYVdacFkyRjBaVkpsZG05allYUnBiMjVNYVhOME1JR0tCZ2dyQmdFRkJRY0JBUVIrTUh3d093WUlLd1lCQlFVSE1BR0dMMmgwZEhBNkx5OXZZM053TG5SbGMzUTBMbUoxZVhCaGMzTXVibTh2YjJOemNDOUNVRU5zWVhOek0xUTBRMEV6TUQwR0NDc0dBUVVGQnpBQ2hqRm9kSFJ3T2k4dlkzSjBMblJsYzNRMExtSjFlWEJoYzNNdWJtOHZZM0owTDBKUVEyeGhjM016VkRSRFFUTXVZMlZ5TUEwR0NTcUdTSWIzRFFFQkN3VUFBNElCQVFBZUNDMFl3YnNScE5USjJJSHdJbUJsZzkyenE5Sjk3MFBYTGJYbkROTFVldFEreWFhMEJ5cUpMTThUQnNyZ0lnUHpheEZDZVRyKytDbXMzbllPbWZEVHM4UnJwdTRxam1ySDJScUtkVFJQK2E4L3pTZHkzU3lmcVY5dkcxNVZXOHZ3M3dVeVVSUW8vKzBFVk1Xcmx6K1l3ajdhUC8rNVVOdU9UVVBGcysyOFRYbFpWS0NKRGZZdDNhaWRvRGd4MTBKOG9oaXlUa0YvRnpCZjVla25taGJnYkZUSVM4UmQ0VGtMSTZMV2w1THJtcUlkdnRnWDJmZDE5RXpRSVNVOVJYZGhMdUlHSU90bHhxeHMwa2ZFVGcycW03amIyR2Z0SmNLbjdvQmlJdkdIaGJDeUxoWHpTYW5QR2crSVA3WGRzcHNzVnhjeDNBWHVXaWYrbGZxQzJ5VzciLCJNSUlFM3pDQ0FzZWdBd0lCQWdJQklUQU5CZ2txaGtpRzl3MEJBUXNGQURCVU1Rc3dDUVlEVlFRR0V3Sk9UekVkTUJzR0ExVUVDZ3dVUW5WNWNHRnpjeUJCVXkwNU9ETXhOak16TWpjeEpqQWtCZ05WQkFNTUhVSjFlWEJoYzNNZ1EyeGhjM01nTXlCVVpYTjBOQ0JTYjI5MElFTkJNQjRYRFRFeU1ESXhOakEzTURBd01Gb1hEVE15TURJeE5qQTNNREF3TUZvd1VURUxNQWtHQTFVRUJoTUNUazh4SFRBYkJnTlZCQW9NRkVKMWVYQmhjM01nUVZNdE9UZ3pNVFl6TXpJM01TTXdJUVlEVlFRRERCcENkWGx3WVhOeklFTnNZWE56SURNZ1ZHVnpkRFFnUTBFZ016Q0NBU0l3RFFZSktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQUwrTytzVkIwNEpXTFdPd0JxRFp3U2VOSzRxZ1hHMEpHMFZiRVZRWEY3RjlWWW9ET1ZWN3VyUG90a1ZrdEpyNm4zMEJmUnhzVHJXeWxoUkVGQW5wbEsxUkxZQTNPdUk2cFdPMkFTQzEvRWJMWlAxR3FIQVJHOGdNaVFiVUpIYzBMcGdqcmVONDk1UU4wTllmS1VQeVVLeGFUVzNmR3Biek5ueEw1ZGpQWnNEeUc1T25LNVhHWkxBN25YYUlMVlVqOGFLd05mT29uY2tIRjFWdmNLcFkyYUlJaDZpc0JhUVFuUnZyaWZBS2kxQ2VpWlZaRWN4eFh5S21USUMwVTZXUWlpT1M2RmdHTExWdGg4OHRRVmhSdUFjY2NKVjQxcDl6OTZ2bXpNcVJzOVlmSmVwaVZ4MXJpczRJNFlNUXpONXRhakdVOXFrdFgrbmdVUGF0b01MY2lvTUNBd0VBQWFPQnZqQ0J1ekFQQmdOVkhSTUJBZjhFQlRBREFRSC9NQjhHQTFVZEl3UVlNQmFBRk8yMHp6OTdJeGgyT3NhdHNtc0FYWHJHaU5ick1CMEdBMVVkRGdRV0JCUS9ydlY0QzVLamNDQTFYMXI2OXlTZ1VnSHdRVEFPQmdOVkhROEJBZjhFQkFNQ0FRWXdFUVlEVlIwZ0JBb3dDREFHQmdSVkhTQUFNRVVHQTFVZEh3UStNRHd3T3FBNG9EYUdOR2gwZEhBNkx5OWpjbXd1ZEdWemREUXVZblY1Y0dGemN5NXVieTlqY213dlFsQkRiR0Z6Y3pOVU5GSnZiM1JEUVM1amNtd3dEUVlKS29aSWh2Y05BUUVMQlFBRGdnSUJBQis2Y0ZtTEhCNW1wbUIrWHNqaFd4VGtoUFRuajNsbkxNejBMS1h5dmM1K1NlQjRJZDlOeWNEQko0YksvYXV2R0tUQXdlYklsMWxTM256VUFiMUFjV3lwaUpveHZFdmJESnloK241MzA1Y3NGdEJ4OXBVU2VVcldYN0Z2WE4wZU9uQnhVblRib0krc1VPRFhBMm80N08vUXdmSm5UeTYvTm5OQTUzNUJVN0oydHJ4VHVnVTJHUXhkTVRncUpHVnMrNUxZQUlSUklJRzF6ZGZzVVBpTFdjKzRkNEZ5L3RjQUQ4MnZnSW5BN1BUVy9WZERTeHV5cEw0QllBOFQ1aWljRWtncll4WlB3NkNJS3owRWFRc2ViMm9HSjlJaWx5dlBMei9JTlRqaUdJR2pra2RHTllMUE52aGZLcUhHMFYydlhJNDVlNTV2L2paTkxGY251ZHN1ekhkWUYxcENLdnNRSDdaaVdpZmxWd1YyeXI5Tm0rV3d3anJwZ1NwRnl5TnlxS2F3RzNqSWdrTGQ5VnhlY3MyNjJ1VDR2M0UxOUpJZ0JKN0ZpUkZKZ0dHY0piQXdsaGlBWTNpTmkrWW95a3o4ZFp1SFZNWjU5WWVDL1dqTzFuQ1hZK1BaUWJBc3VRQ3h1YjNaVzFyTldaeHhlenBzMlpzTmlHaFZXbFpWeTBSb0NYcHh2eXlld2pkRWdEZlJQa3RyM1RPSW50UWxtaDZCdlBuajBOSm01M2x4NVNaS3l5SDlYSHZkTE9GYXdXZUJrbTJ6RldOaEpoSWRJWFhDUVMrR0gyeEMxUEJ4dGVTTldjcmwxa010MUhKNk5MVitEc2kyRFFNUjdKbUxKSmp2MWp5akJTMVpMYTgxT2Y1WXlyM3JLYXdZRTlMbGNaT3BhZU1BSGtyUE5GRjgiLCJNSUlGWlRDQ0EwMmdBd0lCQWdJQkFqQU5CZ2txaGtpRzl3MEJBUXNGQURCVU1Rc3dDUVlEVlFRR0V3Sk9UekVkTUJzR0ExVUVDZ3dVUW5WNWNHRnpjeUJCVXkwNU9ETXhOak16TWpjeEpqQWtCZ05WQkFNTUhVSjFlWEJoYzNNZ1EyeGhjM01nTXlCVVpYTjBOQ0JTYjI5MElFTkJNQjRYRFRFd01UQXdOakl6TURBd01Gb1hEVFF3TVRBd05qSXpNREF3TUZvd1ZERUxNQWtHQTFVRUJoTUNUazh4SFRBYkJnTlZCQW9NRkVKMWVYQmhjM01nUVZNdE9UZ3pNVFl6TXpJM01TWXdKQVlEVlFRRERCMUNkWGx3WVhOeklFTnNZWE56SURNZ1ZHVnpkRFFnVW05dmRDQkRRVENDQWlJd0RRWUpLb1pJaHZjTkFRRUJCUUFEZ2dJUEFEQ0NBZ29DZ2dJQkFKZ242WXcvdVBzUWQzaFhwaDQ4SnFXTzU5RHVpMTVzMkowdWRndlNuYUF4OHlJdjZHS1psRG5ZVm1VYm1lSlF4a2NuWUZvQUdlc3NxUWZTQmkzV3UrSkFOVkh3UG51RUdDUFROQVRHcllUdzZEcTZXRTBkbE5mRmxnQmd0Y1QwNXlDUnRRREhjaEZkZTFHOXdsN21XSWRUVng3NGdJcjJ1dCtZYUhkK1hKWUV2ZXNyRVRZY3ArQkE3TjhKRkNXNkkzQ1NQb1o3TlJSeGQ5OTZvbitWZCtLbmYzN2xSM1krRnpTQVY2U3ZqWTdqdmRsWnNNb3BZeFhtM1U2bm9WdmM1NFErV085NHJ4b2U4cHQ3b3Uvc2l3QkgrZkh6SjdKTUlWTlpxUC9jSldlNm9DRUtsaCtPZDRjdGlDOWtOS2RJU0UyajdFYmhMeUhUOVN6VU9mdDlvSlRuQzBTN29Xc1BNQ1loRVVwWGZWVGJzN3ZXUlJGOHBTL0twbjhrSEd6ZS9SRm4vcmtLSUZPbG1yL2F0NG5Sd0IyamdYZUlOS0hOVUx6MmJaZVNKLzd3Q2dsWElPeGdzR2VxeVlFc0lMblVzWXFWMXk5QUtXc1pkS1MrR2xHUnNIaWJVWXByNzM5K3FOODZZYXlRbE1MRkQrYlZuZEgyMDBOVm95NWYxN1N1VnF0RzIzMVZqVjJtR0lPd1BScHE1ZUxQSHFQY05odWprT21YUzFITURKSk9MMWdPR3d2SU55MVNKVTdSVENEQTBvYzBqTktJcU9qSkx5aVRVd08ra3p1Nm5iWTNMcWF4aHdnQ3JaR1ZhZG8wRFFXRSt3SHUwZzJPN1F3aXVOUlg1d2tyYVd3UEFjNVEwd1NhQmwveXgxNThWOWhoNkdQdGpNN1FSYy9wTW41REFnTUJBQUdqUWpCQU1BOEdBMVVkRXdFQi93UUZNQU1CQWY4d0hRWURWUjBPQkJZRUZPMjB6ejk3SXhoMk9zYXRzbXNBWFhyR2lOYnJNQTRHQTFVZER3RUIvd1FFQXdJQkJqQU5CZ2txaGtpRzl3MEJBUXNGQUFPQ0FnRUFLTUtGWnFUVnA5aEVrcVVmUVp2eGVrekQxaEdZU2Y3N2FqOGx3VmsvNUUxQjJsSFZvdFR6a3E3a3V2Rkx0RnhVNnFSNkgxUG41bzBCdUVWaTlTN1JwRDNUTlhRUFlWZGEyWkJPcllnR1VLRDdOek5QeVdXK3ZEb2JPS05sTFVmd3NuanVDWEtRU28vdDU3ejVGWUxQZk94MWdtbHVJZWhjMm1sM1dmOVIrMFdUalRTTW4wazNIenkvQ3FpZFVKMnZwbS81dTV4MHdYeHJJWTNLZys5SWRvV2oxYWVzYkxMNXNPZHZnS0R6Y3pEcURtdDF1WTlDRGtYdVBEL3FHdFcwQWRWSWZ0bW54YjZKVG4yd1E2SlNNckdubFk2QnNVdWpxTlBvZVZxeTh1ZFErY2tVZ3pKemhaNmpwUkhxcTdhUDhKYnBWeDNLWnc3TG1TZE1yd1NaYVpaZk1hOU9LMkcxS2I5WFBWS1BCNS9LV1M5ZGUrSkpFWFZqcWdLLzJWTVU2NnkvUFJacVYyQzgzeFRTWkVFNDdQM2I4RVU5cXlNN0tpaS9lOFVFR1JRUjJRMmkydFJwMXlIQnp3ZWtLWmd0WXpBMHBvR1ZqN05XbkNBbTBPbENXeUJQNDdNeWlNUzRucHFOWWY4d01oUnk0ZlRZK3l5NU9SSnA1RmMwOC9WYlR0NjJDUHB3cWgwNlM3SDA1WitJelJyb0c5eXAwTkJ5NloxNy9kYjNKbm9zYmZuSGR5RmxacXplMklMak1kQndNL2VkUFFnR3FocGduMlVETlRFOFNtVkxEbW5xMGhHQlI5c0FBMFhlZEZqQTBDdnhpN0FsWDRCYjlNdUFRenJmVGNGaWt2Sm12bVVZNCs5MzVHNzBsclcxb1NYWnVnRnFnQU04RlZKUDFwWT0iXSwia3R5IjoiUlNBIiwidXNlIjoic2lnIiwiZSI6IkFRQUIiLCJuIjoicEc1WHdHeEVjSXRqQW1JOWFQaGpVOVJNVFZFVXBmMkVNa012OUFsNTM3MGt6aGMydzlfWHdKcTgzd29MVE1mUENSNjlGSzZtTXdTVjc3X0VQbkVlU3pUZVlWdzRmUlpZQmdCcWZQZVhpbVlXRHRnM1dTTGM5OFQxeTA1amZHN0lRbXBpMmNQSFlEdmlIQ0RWZ3h5UG1vNERqWFZqWXloa0FTYy1jZFZVZTdaY0xnZGVZSEpQMVMyMHA0ZDI0Rm1uZXRGT0hmQzBVaHYtRC1lU2k4dURRcmhuLW41dGFFRHRMeTRJMTNtS1c3dG5mZ2xfUVhwSFNGYk1NVmdBWV9aR1NfeUwzOUdNZFpmU0t0QWJsZW9mQ0Z6cEF1MDlWTWVDNmd0ZUx3aFNpV3VKc2xBb090cy1kUUd5aVdlOXpYanA1eGU2OXR0SnprSTFEOHNIZHR4c013In0.eyJzdWIiOiJiY2Y5NjczZS0yZTc3LTQ0ZGUtYWY1My1iN2Y0ZTU1MzViOTkiLCJqdGkiOiI1MWU3ZmZmMS1jMTQ2LTQxYzktYjFkMi0xYWFjOWNmMTA1NjMiLCJhdXRob3JpemF0aW9uX2RldGFpbHMiOnsidHlwZSI6ImhlbHNlaWRfYXV0aG9yaXphdGlvbiIsInByYWN0aXRpb25lcl9yb2xlIjp7Im9yZ2FuaXphdGlvbiI6eyJpZGVudGlmaWVyIjp7InN5c3RlbSI6InVybjpvaWQ6Mi4xNi41NzguMS4xMi40LjEuMi4xMDEiLCJ0eXBlIjoiRU5IIiwidmFsdWUiOiI5OTk5Nzc3NzUifX19fSwiYXVkIjoiaHR0cHM6Ly9oZWxzZWlkLXN0cy50ZXN0Lm5obi5ubyIsImV4cCI6MTYyODY4OTQ1MiwiaXNzIjoiYmNmOTY3M2UtMmU3Ny00NGRlLWFmNTMtYjdmNGU1NTM1Yjk5IiwiaWF0IjoxNjI4Njg4MjUyLCJuYmYiOjE2Mjg2ODgyNTJ9.bfIDk3SvnrHhPsK064592sZ02VY7aKP-4GzG6yXe2dfwIjxD5Jlwpyc2fxpX6-gN7wAaAbu0kN8gyjHPujumLv-xGO9NynHmFt4Rf9h_OkGI_rCotwWSVr4_ZN4RjDJS4YIsaPhMTGxfAQ6IsYnFcd1OYwanCIfOrdR2gGjc9qY_TZIZ4CuVqh05oBHOZ-yJ0lbX-7EGClDirzXKyvYDcRIxrX-n6x0sZNmPvJwvtAU09fC6JgbbzEetMReaDAE99eIkAKs48YY378KBC9JwVcrBpUXLxmfJuTakjMrVD0jFvnny93QyV92KgsM5cPUPcZ14n87FUjo_5deyyFvOgw"; 
            var request = new ClientCredentialsTokenRequest
            {
                Address = TokenEndpoint,
                ClientId = ClientId,
                Scope = Scope,
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = IdentityModel.OidcConstants.ClientAssertionTypes.JwtBearer },
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
        private static List<string> GenerateX5C(X509Certificate2 certificate)
        {
            var x5C = new List<string>();

            var certificateChain = X509Chain.Create();
            certificateChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            certificateChain.Build(certificate);

            if (certificateChain != null)
            {
                foreach (var cert in certificateChain.ChainElements)
                {
                    var x509Base64 = Convert.ToBase64String(cert.Certificate.RawData);
                    x5C.Add(x509Base64);
                }
            }
            return x5C;
        }     
    }
}
