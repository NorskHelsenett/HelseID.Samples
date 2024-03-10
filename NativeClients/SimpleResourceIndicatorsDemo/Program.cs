using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient.DPoP;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.SimpleResourceIndicatorsDemo;

/*
 * This sample application shows how Resource Indicators
 * (https://datatracker.ietf.org/doc/html/rfc8707) work in order
 * to download multiple Access Tokens without performing multiple
 * calls to the Authorize endpoint.
 * This is useful when calling national health APIs, as most
 * of these APIs require Access Tokens with a single audience value.
 *
 * This client for this sample application has access to two APIs:
 * nhn:helseid-public-sample-api-1 and nhn:helseid-public-sample-api-2.
 * The application requests scopes from both APIs, but the first
 * call to the /token endpoint only requests the first resource.
 * The second call (using a Refresh Token) requests the second
 * resource.
 */
public class Program
{
    //
    // The values below should normally be configurable
    //

    // In a production environment you MUST use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
    private const string JwkPrivateKey = """
                                         {
                                           "alg": "RS256",
                                           "d": "sck9fMkA2qXTPH11k2BHd6q7sNWglTG7B_QJ7ZlhAGxJavhcnb_qnDOJi_UcjvMHoLNqO02Tnvf6Sat5jxMVuPZRXsM494QbTKrDT_I6GnH0APL5f_ThuqL1g6bdK-w_PTNoy7VGciyGO-ROi-y94Sano_kDMSFNrgZbXPYjZ6PhP-frbWSgLLoNBoXqp6Or8hXxtLWX_eHzyLM9TDEOEKMnArOTSmmbzUjmAJOwHVjz8w-X5iATNsPM4rM_lZ4QX-oX-I-YsbuAqs6xvnTBv5GgRNVr7czx7gnMACsvBTSXhgKDREPf3rsocAmQWPOZG_ldZIvupPV_hx5pwAL_WBtPdMbVYcTajwR0iKHdcxuwAOasG8aGhthufWFNLw23nAxTxExpSO-azWbBYqC23oDPStkSpDAxN6uo4x038f2pPgFmSB5pOmu8knSjNuTTk6b63o71weK9-rqtLFFyh3VpsfKUas2ID8I76qfV-QfGwW61NF_1i8Gkca9hHZDdFleMEEIEk4Ab-tLRLvBlt3JuLCDgG8ngZbPNhrpXYkGMpUEOHngfZ-P14qhAh1pwjxs-r6cRmKJiXkMJNuyxKeqWCuh63zEtYvaHfyKtXQpTntw9rkbjeKG0esmVX3zL0Vq4410VTMpNIVhYwIrLXuPkkVnIINTkuQ8NSyP2eek",
                                           "dp": "7lTTnc4-WIUDT0Jf3iyn_M0F4mkPQf_it7ohPvSHp8skO8W2bYaUvNwpP7tEu4KftJ5PEiTuP5mF_wKtsumoA1VJOQlmDCLL5JfjNiVh31OuhM6zGIlCjuZ2_w_VRumpDnPSOzieDj2v5nvv3TkDR3zx4LnJbVGe0DWEmEoCukotgRWEKOO5MKZla_-IeISVHqSRwBHaqTfE4duFVNJdUP_W-nI_xUIHx7NEErSAf9bkWJMHJZlqX8sNuyBM4W3A6kyApT5uRXxoNeYkO3ODPYjD57dv6DsbM5EtoB09C-OpafFW_LYgnaY30z4WF9BSTclGzwLOhzLrJH04bA3lIw",
                                           "dq": "re5d1vNXA4kMmWm3Rb4UCxKyPyjtH1_xFKo_Lklr3ahboQJtwoTaw9BYDq4PkeaYxP6_vv8zlYMDJtzCKi0UaRbX-rtqAmddXbGVzmZ0-KwHV-mNS3DJGoTr-yUdbODyHqPUwcfOC8Hg3EGZPM8Mhge4W_Eh3g1j3bp2YZaxCBodwNUG2NT-N-J-Zbk5ujAgnxdv_GfhTU4BmiCvx7FpfKv-IbEgv9Zaw2Un5BL2H3SlloVfMY_DfWEUKzL_NgyBEoyfEgTO1hfjC49kQzysRSRu8C_n3LJ4rmNjyP7zL_oyKpmutyBWQ9nTQzTR-lkZX_uzmEIn6LQ9rvS8aVNiPw",
                                           "e": "AQAB",
                                           "kty": "RSA",
                                           "n": "z7cMpqAavfl5KQy5XaXZ5sNmPE1xyaDyZgX6Xd1KbM5dHaCBfiwfyFmfQ5n-NhaAg7S6vlhCI_9GiTaR7KcOSOkzcEfQcJV8QbWcSO-gG_Liwm4bhhXSj5j4FICOVEpGrAeVwF9NImD0XUItOAfboFPeDrwdXSrBz5_NVszzjI05rQk5kse3t1GGqUGWM2FhObQq4AE57hnD-J7YtA99l-7yuldgu8tFbZj3EiTli0OBCTnRdRjXlkxzXX50RT-_JhUMLdrR9ShKb7yDdcQIdB4ewLVQFzAcKbYq49_Us2QM3QqtCMSENxI0pNo5Dv8NDz7CD2Ujduxeb7GB1P6VEusxELIuIsHaCPpg2Xpn-WMxDUCZsZzOGLGXbsImIxphohON4JWA6dwQsRJp5dQwF9iBGsCdyLHnENKexHuHOJBxTSKDJoRu-1t1w_fjc1wyVGliPtKRmS7aVj1fW_-i89k_G1VFgQ9A2GFas1AA9LYiZQxtejTqCWKE-BezqRIjfFsI3jMIjR4X5Tx7X6sSqEItcialVwMrc29N9FXprccTMAYDY1Q3JVkcJz8Y0E35QfA1xloEBInfUZ-Yz_nD39_yZ8BUlFvrgc4OAkv8A-xbdle4k5L8r9DhNuBRxiKR47-oGfGlxYQ7TfOydXwPTsHvZ5YCMwmgc31P7dAhD-U",
                                           "p": "_xd73ImAb5f2SOWLOuX7pkyQNHDOrrc8ELGTJH55dMecypSfgY6y8rRQVuobS80OiKXyUExT9Fe9CoavpVJ68oWmC5vWu5EKEbe6dIpjMuAYUW6dBcrBvBThAU3A3yeY2X5WNttX1wJt__lnbXA88Var3dlehaAmRWmZnhpTvpTMBjeRt9zJ9IjGHgOwg14wkgDyT21HaO333uwohcmD5nOEkq_7ncpDYMmrHHHq40_3-HA2wWsrKhQIZ4K1CAaZUDumBJHtfrrRS-wXVgVN4-EeG8zXUHtvJtiiMMC1202ZO2ryZDQu28SJhh5M4LUxHQSOM5t25FdSzvJ4Y8yUfw",
                                           "q": "0HRhuBG0k0f6p-ZZMwVb_iPlD9MKOJ-WCCIjETBPNQdZ8XgDux5uZyYL0isb0nfedanMPWXjBlFzzAjcvQlccmm983XEbm8vai-qO_2GdD88956cc7Mc9hupKtYgW9NCNXtGnnjsTYHMWqCOKUOrGdHcVaQ48Tnu-7L_rV3fSgjM887q1_B9hnYk9P3xNoZreZDFna2MLNiGYRPYNXMtAQEMONSNhIs9pl35QUdOJpRTNwbIPiDcIXwltzeRJXSgdaoCBoG6qVMsNIAt7cF7BmcnNfCZFrr6Mbo-_LGP1b3cK9sY1HZH1_bIWJzDB1AatWB1_jhIOKWuXBjT7t1Zmw",
                                           "qi": "u6IPj2-r45lcxbpQdqHAbXrY0dPgaeokw56Ai4hEwSZ5TUvgkTDfyuoGcO0Jz9vQRC_NDY0HLt8zKgDze-wKKSE3WSrLOevRk0fts08khl5PuTum4yFpgmkhhGdcbuwIP6VpxOaq79dW0-0sQGu2x-7zWbedKIn8FJCaMB6z8v2ouvIJIDouUkWSVckyFWmZPcwdCabajnA8mxtk9a6SZvQC5decufLxChJTCHuAgP5IctpG5686jBp0lF0Ly8WYXmehPdfZ8aYV4jhdQ-PkpgexpwAGaQO7QUHaEyFPAe6XCvj39MF9oLnQzEmvrvVwM9dzcdd6Ie4dSZpA8RZySQ",
                                           "kid": "7F0F6BF5DBFA20EE33F8736A995471E9"
                                         }
                                         """;

    // This client_id is only to be used for this particular sample. Your application will use it's own client_id.
    private const string ClientId = "helseid-sample-resource-indicators";

    // The client is configured in the HelseID test environment, so we will point to that
    private const string StsUrl = "https://helseid-sts.test.nhn.no";

    private const int LocalhostPort = 8089;

    // In a test environment, the port does not need to be pre-registered in HelseID Selvbetjening;
    // this means that you can allocate any available port when launching the application:
    private static readonly string RedirectUrl = $"http://localhost:{LocalhostPort.ToString()}/callback";

    // This is the scope of the API you want to call (get an access token for)
    private const string ApiScopes = $"{FirstResource}/some-scope {SecondResource}/some-scope";

    // These scopes indicate that you want an ID-token ("openid"), and what information about the user you want the ID-token to contain
    private const string IdentityScopes = "openid profile offline_access helseid://scopes/identity/security_level";

    private const string FirstResource = "nhn:helseid-public-sample-api-1";
    private const string SecondResource = "nhn:helseid-public-sample-api-2";

    static async Task Main()
    {
        try
        {
            using var httpClient = new HttpClient();

            // 1. Authenticating the client
            // ///////////////////////
            // Perform client authentication; uses the /par endpoint in HelseID
            // Use the Resource-parameter to indicate which APIs you want tokens for
            // Use the Scope-parameter to indicate which scopes you want for these APIs

            var options = new OidcClientOptions
            {
                Authority = StsUrl,
                ClientId = ClientId,
                RedirectUri = RedirectUrl,
                LoadProfile = false,
                // This validates the identity token (important!):
                IdentityTokenValidator = new JwtHandlerIdentityTokenValidator(),
            };

            // Set the DPoP proof, we can use the same key for this as for the client assertion:
            options.ConfigureDPoP(JwkPrivateKey);

            var oidcClient = new OidcClient(options);

            // The authorizeState object contains the state that needs to be held between starting the authorize request and the response
            var authorizeState = await oidcClient.PrepareLoginAsync();

            // Download the HelseID metadata from https://helseid-sts.test.nhn.no/.well-known/openid-configuration to determine endpoints and public keys used by HelseID:
            // In a production environment, this document must be cached for better efficiency (both for this client and for HelseID)
            var disco = await httpClient.GetDiscoveryDocumentAsync(StsUrl);

            var pushedAuthorizationResponse = await GetPushedAuthorizationResponse(
                httpClient,
                disco,
                authorizeState);

            if (pushedAuthorizationResponse.IsError)
            {
                throw new Exception($"{pushedAuthorizationResponse.Error}: JSON: {pushedAuthorizationResponse.Json}");
            }

            var startUrl = $"{disco.AuthorizeEndpoint}?client_id={ClientId}&request_uri={pushedAuthorizationResponse.RequestUri}";

            // 2. Logging in the user
            // ///////////////////////
            // Perform user login, uses the /authorize endpoint in HelseID
            var browserOptions = new BrowserOptions(startUrl, RedirectUrl);

            // Create a redirect URI using an available port on the loopback address.
            var browser = new SystemBrowser(port: LocalhostPort);

            var browserResult = await browser.InvokeAsync(browserOptions, default);

            // 3. Retrieving an access token for API 1, and a refresh token
            ///////////////////////////////////////////////////////////////////////
            // User login has finished, now we want to request tokens from the /token endpoint
            // We add a Resource parameter that indicates that we want scopes for API 1

            var parameters = new Parameters
            {
                {"resource", FirstResource}
            };

            // If the result type is success, the browser result should contain the authorization code.
            // We can now call the /token endpoint with the authorization code in order to get tokens:

            // We need a client assertion on the request in order to authenticate the client:
            oidcClient.Options.ClientAssertion = GetClientAssertionPayload(disco);
            var loginResult = await oidcClient.ProcessResponseAsync(browserResult.Response, authorizeState, parameters);

            if (loginResult.IsError == false)
            {
                loginResult = ValidateIdentityClaims(loginResult);
            }

            if (loginResult.IsError)
            {
                throw new Exception($"{loginResult.Error}: Description: {loginResult.ErrorDescription}");
            }

            var accessToken1 = loginResult.AccessToken;
            var refreshToken = loginResult.RefreshToken;

            Console.WriteLine("First request, resource: " + FirstResource);
            Console.WriteLine("Access Token: " + accessToken1);
            Console.WriteLine("Refresh Token: " + refreshToken);
            Console.WriteLine();

            // 4. Using the refresh token to get an access token for API 2
            //////////////////////////////////////////////////////////////
            // Now we want a second access token to be used for API 2
            // Again we use the /token-endpoint, but now we use the refresh token
            // The Resource parameter indicates that we want a token for API 2.

            // We can now call the /token endpoint with the refresh token in order to get a new access token:
            // Client assertions cannot be used twice, so we need a new payload:
            oidcClient.Options.ClientAssertion = GetClientAssertionPayload(disco);

            // User login has finished, now we want to request tokens from the /token endpoint
            // We add a Resource parameter that indicates that we want scopes for API 2
            parameters = new Parameters
            {
                {"resource", SecondResource}
            };

            var refreshTokenResult = await oidcClient.RefreshTokenAsync(refreshToken, parameters);

            if (refreshTokenResult.IsError)
            {
                throw new Exception($"{refreshTokenResult.Error}: Description: {refreshTokenResult.ErrorDescription}");
            }

            Console.WriteLine("Second request, resource: " + SecondResource);
            Console.WriteLine("Access Token: " + refreshTokenResult.AccessToken);
            Console.WriteLine("Refresh Token: " + refreshTokenResult.RefreshToken);
            Console.WriteLine();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("Error:");
            await Console.Error.WriteLineAsync(e.ToString());
        }
    }

    private static async Task<PushedAuthorizationResponse> GetPushedAuthorizationResponse(
        HttpClient httpClient,
        DiscoveryDocumentResponse disco,
        AuthorizeState authorizeState)
    {
        // Sets the pushed authorization request parameters:
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(authorizeState.CodeVerifier));
        var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);
        // Setup a client assertion - this will authenticate the client (this application)
        var clientAssertionPayload = GetClientAssertionPayload(disco);

        var pushedAuthorizationRequest = new PushedAuthorizationRequest
        {
            Resource = new List<string> {FirstResource, SecondResource},
            Address = disco.PushedAuthorizationRequestEndpoint,
            ClientId = ClientId,
            ClientAssertion = clientAssertionPayload,
            RedirectUri = RedirectUrl,
            Scope = $"{IdentityScopes} {ApiScopes}",
            ResponseType = OidcConstants.ResponseTypes.Code,
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            CodeChallenge = codeChallenge,
            CodeChallengeMethod = OidcConstants.CodeChallengeMethods.Sha256,
            State = authorizeState.State,
        };

        // Calls the /par endpoint in order to get a request URI for the /authorize endpoint
        return await httpClient.PushAuthorizationAsync(pushedAuthorizationRequest);
    }

    private static ClientAssertion GetClientAssertionPayload(DiscoveryDocumentResponse disco)
    {
        var clientAssertion = BuildClientAssertion(disco);

        return new ClientAssertion
        {
            Type = OidcConstants.ClientAssertionTypes.JwtBearer,
            Value = clientAssertion,
        };
    }

    private static string BuildClientAssertion(DiscoveryDocumentResponse disco)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, ClientId),
            new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };

        var credentials = new JwtSecurityToken(
            ClientId,
            disco.Issuer,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddSeconds(30),
            GetClientAssertionSigningCredentials());

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(credentials);
    }

    private static SigningCredentials GetClientAssertionSigningCredentials()
    {
        var securityKey = new JsonWebKey(JwkPrivateKey);
        return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
    }

    private static LoginResult ValidateIdentityClaims(LoginResult loginResult)
    {
        // The claims from the identity token has ben set on the User object;
        // We validate that the user claims match the required security level:
        if (loginResult.User.Claims.Any(c => c is
            {
                Type: "helseid://claims/identity/security_level",
                Value: "4",
            }))
        {
            return loginResult;
        }

        return new LoginResult("Invalid security level", "The security level is not at the required value");
    }
}
