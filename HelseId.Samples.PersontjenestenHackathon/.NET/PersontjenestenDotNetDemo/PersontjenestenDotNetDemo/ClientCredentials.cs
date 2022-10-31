using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = Microsoft.IdentityModel.Tokens.JsonWebKey;

namespace PersontjenestenDotNetDemo
{
    internal class ClientCredentials
    {
        private const string _tokenEndpoint = "https://helseid-sts.test.nhn.no/connect/token";
        private string _clientId = "fe5d6d6d-fa87-4e8c-b943-0dab27514200";
        private string _scope = "nhn:hgd-persontjenesten-api/read-with-legal-basis";
        private string _jwk =
            "{\u0022d\u0022:\u0022G_pqfUcnJcvuJ6ZhleGXvoqLcqz35lnmu8X-4avioPeorSDYYufPyV2gIpf-seWrDzc7MjhrF4zLyBmmkvPLkSCwhSjafsFNmy146KRhc1ynTceroIn84iOJ6_UPH5FhwB2qNjWpvzyKGd2pr4FhL5eEyAjaygLjGrs5AOe97XkPnH9mtnRBjI5m0tVttSxMramAONvF7ON6MoafeGhzbsz1EJbXU53yc9XIe8aZhWTAo_l9hesqn-BdnjXUjIBbB5tHSrs3zmk4wzGDc6L9fSVJEG12EudJ2iu5_PJ_qEuqhihDChgZ7Kf39IlhF7QVzNtSJ7khHPzQOuJiowiS-vLO7F5-xLYkCi6-7277siWeNqXIbT_xJX0ZY7mMrCMtF2VOVTn2mRLTmu2IYefvUcL2nzcalUGxAcusmfLYAXmElcmzgcBqRWzuq3byy8K7cDz8E4pQ3pV2kMiekYJ2IH5AhCninWH8gEFC2w8_MdUk2-GHrYDxwclXsbIIArKX-tX5wuJbgFpqPFnlj238i9K6UUlV-GP-fRb8UxZiSja8GHTK5022a9UhctcIicNcPW86aqP6N6LPgQ38cIXOm-0gcL-nWy2fz_CpXHwgn236eQhpHEM7QM_YuZsBJOygAB3jsU6nJqpJFoZDHUl1tJSG7w4OwIsZ5YzpMTB-R-0\u0022,\u0022dp\u0022:\u0022G0EMJ6BJ7kay5xtDTbH6S-ENylOT1FZkpIUNyrn7kxPFIgmKXOAOGo5ybDP2BF3J2Fc2U9dw0Mx7-CdTN-ltN-P9bhOZEhUgQDA85OXWVLEEWbLa-tABLiCn6HvO1ulvsgx13CmImvvnKd-KQVZQWq1_UHb30V3Ow6UqHx2VqhR4NHgNLPfgAf9voGV_QMkHuqpDYVsu-nmrCjKc6UEb1vfyPkALHmmwYCHf4DDKbE0ZE70kN787GWxvu4ASiOv-1I_nNLyZVuePu7Y5xsYKn6rY_q8fFyn6vv7weKpBEigDfzrSo9boKOF_6BtbpBmMzRBbA4UbCzjVXzmXTwDP6w\u0022,\u0022dq\u0022:\u0022pMn17-AFH0-S_ZPEqY5y2w-iGz0RNTkm3vxYyW2ZnCk9nyhYwvKg0vjLzJI3nuRiRaGzEE_Ig3XcrPKi4wuQ8N7_O-7ZxaOPzCQ3sa8TMO7U7LkmnOLg6fwg8mfkWSMiFRxSJKAMkpjgnbuyDfXIG3PVAw_M6ckdmG3BAQyaxGM_8N7aEkAuAi-CG8uXawokEIEP5a1vE5Z4gHJo-Q_7gdtI4ROsM4-fY_bY0pSmcxaUJZg99PnJRhJnxHM6udvgN2SdiEpCx6BqlsJ_bgyudoBDjnURTKtk0DghQPPCUfdh9G63NnLVlZrtUDjgHRw6PzAU_0bQllVjvoBel3TCMQ\u0022,\u0022e\u0022:\u0022AQAB\u0022,\u0022kty\u0022:\u0022RSA\u0022,\u0022n\u0022:\u0022ywjixAlPUBw7ZytSEknq-HCJWLV-GHxqoad2kLQuSUGc3X0GJOnEcsmzo9HjT1peTnjh6JMZtavniKTT0w1sOdWyU1Jk9dFZ9UI_uhOeXcYw_t2KtPSAz7zDWhXqE8zmVPeXHP7HpMnA99uXCVs2FbvO5ALqK04DgRKAwTujDpHnROISXVhbacKyVh_vEM9TBtYvZ_KV-S4Jo1RGYtCSP5CJWxNbxYc-nKawMPURCUbz-STO6jiJ9xIK5si3htCQAC3G92zlM6YPfTc1ouvvG_GMkEzVhLriMTK3iUwg2R4K4VKbsNTUDjog1wUeyXc9rlYW7w6CSQTwQkrSbiIhRciyrM3BxZW1epBkElbJ7cytmw8EeThCI2FGMRKDF6YndJcNu6lB2em99TwDK794889-xS4LM0a_UBXAJlAWtmK464WFPHCy2PtzH43xH-wcISscg-KkKaBw5tl8aia2CayeLbCICjElTZocZTLL8-7w8t5sqcJ6MV4CLFl0env1LV1Bh_8wOHD67yqy_kIISa2OfCvdIb8QZhEAzXpph6H379Kk1INAeLQplyB79rjkoRKyOzaTst3Mdo7pfKrnBZMNHgLHuYeaYyO5R-uefNx3qfAdWXl1vstl_oloQ0mFkBgwZE4ev9-7ulL624in9EGRF8Jg8yUnphiHPOMPPtE\u0022,\u0022p\u0022:\u00229XOdGXNEX5G4Fis28tpSmJXILww2dGGNNO8o-efyq0zfzdujfjP1ib8X6N2RMlW0THJbOM50l70vsMBWp6_4uqI5IjH-uOjUG3UCZZ-m38m4QmQA5ymv773LM2MTjMMgPcfHvCcle_RlYQOgw-YG3BOhOb09VE_KlBo8TqaQ_1YYBntlhzsruTMaM-TUSDKtJFcJBxdD61ZZI8Ph4b8Y8eKz-__oTiBkiUdPDN0oq6iFtf2JMgpxXu7EgnAnJ4ZpWLaAkVqo6edlm4bBB2qbe378SimA8Tol9SdY0FuG_16nx19d2LlTHw0mS-QhDKvY-Pg6ZK_8omIr1eOYB-JpUw\u0022,\u0022q\u0022:\u002208KdJJzgtuXekz-9fJ9TDU3uJQU-IliRAdK9-JAZG1xzNkgpVoPQUl6vB9IU4xtu19KqmLDQl02LKFqsYbvp-eVWHGvj1Rldw8jsjbUXfebkJiOtEPQkhumG4Snq4fXGsoeGkgV3o_ziRuYEdzmTQuvNrYq96OZgduvZM9ScDfbyPzNqYY1kw_P5eJRbB1tFpTDfd3QJe24M4oobSLBPwsE1Wacoy6eW4yiVHA6VBVNxLthNfSCjhJ3OPWo19MoKIszTIRaVZJvmans7eZ2ZIlPl5EE2qYaStg5NoO8Hst4BvvMB6c20JVkz-pHZ9hphPKYHi9hnlyPpIU-JQVoeyw\u0022,\u0022qi\u0022:\u0022g1TrEdNtg4pIKIO9b3KUWk3MHGG_KnhVjxlF3G_7Lef2yu1p4nKK1IL4owfPRz_4pKdrQY214x-faxtVL59-yGiRvG0Dp_U0u_hTkd3IzqIRQLGh4CSSm3KY5nslZF3TN3jY9rEGYagAH9uARcdv8ESVbHNGleT21xGu69v17w01_TglobZjAEgEyj273k-DEyrgbgM0Fil4j8YlisD-5T1xv9H7J9vBa2GvyrUt2iyasYUeIwH_ooiNZ6O1ZA8-tOp3qiFwq2z8K0Gm6LFgRaNH0Ne4z2kB6xMFWnthZc3CUNvVxwmSvmqszEKIPEbs_grB2NTpxbCEJ0fBVf8O6g\u0022}";

        public async Task<string> GetJwt()
        {
            var securityKey = new JsonWebKey(_jwk);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, _clientId),
                new Claim(JwtClaimTypes.IssuedAt, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N"))
            };

            var token = new JwtSecurityToken(_clientId, _tokenEndpoint, claims, DateTime.Now, DateTime.Now.AddMinutes(1), signingCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var clientAssertion = tokenHandler.WriteToken(token);

            var request = new ClientCredentialsTokenRequest
            {
                Address = _tokenEndpoint,
                ClientId = _clientId,
                Scope = _scope,
                ClientAssertion = new ClientAssertion { Value = clientAssertion, Type = "urn:ietf:params:oauth:client-assertion-type:jwt-bearer" },
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };

            var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);
            return result.AccessToken;
        }
    }
}
