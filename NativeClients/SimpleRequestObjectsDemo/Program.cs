using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityModel;
using Duende.IdentityModel.Client;
using Duende.IdentityModel.OidcClient;
using Duende.IdentityModel.OidcClient.Browser;
using Duende.IdentityModel.OidcClient.DPoP;
using LoopbackListener;
using Microsoft.AspNetCore.WebUtilities;

namespace HelseId.Samples.SimpleRequestObjectsDemo;

class Program
{
    //
    // The values below should normally be configurable
    //

    // In a production environment you MUST use your own private key stored somewhere safe (a vault, a certificate store, a secure database or similar).
    private const string JwkPrivateKey = """
                                         {
                                           "alg": "RS256",
                                           "d": "wUJFbzCvRwDlDyVyCmLmXQy0Xod81R5Cwk8U1vW2cJg1E88dQurAkgGAYcISUJKGW1haCVn-WZqmJm2WXLTjNHvGIH-sZapWqINVuwrl1FF_hQ-Cf2hRCyV8P-eU0tn_GH0gCRS2_ER5AbtDw26JfkHy9Y3ZRrL9EXjH_ZEd-7fNM_g_UelGe1a0xBLrCf80HkZaO-U10MV-iu88kqsUFb3RdIsbykGgrYVHmr87Y6K1DLYzJESQU3z_rJmubSELE6-HWw_gf1zb-FXhb1M3i1fbFlB0muom_BnbyOvOFRhGV5ngi8tyRBaz9BMbtyWLEEJzLorjz7C4iKfelefjjaUh-BinhpZ35j_Ki0aY5rwXjcyxQiciUHdfDcntzu815Rq5vu2lcL7VHz4mIp-X7Er4PfKqlrIgBp52SVJpWI1JEL8c7vA2ABGM9hqqY_Akh6YJmdMwNUpqE_Madr_cI2X3R9D0AxeGYrhwwYx41izST9X8dPrJ9X9w2UGlOCweHBi3Ok8gGIYvZzbi6cmXMkzo5J0-qCTQYDS2Lb6h4YKEVN7TQpp3PjXOfeZrc4AW8pHVjnirMoI7GGioGDMMEMA2n60I2qMmX-nyb5K5OsbWPDqMKZLBdTk5mfNfQvQy6cF5BR25QjxFGvXH0ThWjFaUWCgSBa0O3azzkcG1-b0",
                                           "dp": "Y9CvDg6yfbZGyE-ycLasng1NLT1_cfiYkMLXa4c9TN2L7Ta1R4sBc4vX4IvZ_kSD3ubHD4q3vKSGxEawD7x-3odyrkamLZPHmkvafrkIZPZWcNqGTZAwqCpHxmodJsJIOkPWI_xn2uD2yU9aAaZEoxTqA7Oac-kLdDPjLKsvCRJcilQyD4kvodoK0n4YKINJM-taLIgsxwA7ZkH2GBlV49ZCvKLRZGERhEiunxncx4LNKUd5CBbKCBTZse8EpdhFxbzRgUxOQZujXaVOzdZB_IUkfDPFsN5Fjf6rYQmbyp1W7rUlqJQ5sqRgZngjrCMUdKCzMhVN7X34IMzUDv8ppQ",
                                           "dq": "zcQhwDxvPDIPUMbIyyWw5G-tnMueZS8KlSmSTzUaLC9QDOU9RbSK6SbNSP-zahG13IJwtAJ9TqYlmj9AbVpdrXtctdcZkUn_wX5P-Qz6J_w-0xODjM_YB3ma_qYsh4yYoEm4lgS0JZN2F7fnc1rOXpzBjm-jsJyRbulD1K908dy6ui9kuO4FfoWpXwX7Jixqfz55koIm1umSVjwI6otz7NOLd8gZcapeBFPrCZnCMIVrGBU0DDVAolEF2GMps4a3259VYPYPkDDs7lwSY8Jk1jFUmdkqwXLwaDzlgnpJJI2boHMOxt1QAuMdToLphah5R3HNLh1NTrLQn0Uehiazhw",
                                           "e": "AQAB",
                                           "kty": "RSA",
                                           "n": "4e76k7QF01kw3hhdHyc3iyUn6c465yrLD9KV7m4gNIc3Tm66Iq3P72xq9w5abAXJG9GJUkbhtTG5isFmXLCU4MOlU5T2a7iBqx2dmao72LSsQ7WZQVtIv5JvWYAXpd4rlgTaJfO3Unv9Tn8v5wWgL9ZbplVmR_GWMY5l-7i44PWwLwGZge_KUVGQmKtx7XXsnezG4JfEAPJbO9zfD4CH6AGtRdcAn2r-2-jqk_-uU1BVoWwDdrCJ_DKOyYNDUfkRneTATY5RDdH5flNd-19XW31L1q3dTMqHbcMzFMfiqwyBYX5kFJrDT-W7poIex7jhZfA5by8K1tfwJqdYRGH4Qp5QtBTs76iuANNBUz3tO0vmV8bYez2AWegvRqrZGHPsMPtA5pCjXw1rueJdH5WqpCHrBsnNkOHNVcd8yHLPmXRokwi3cSanOuquLOI5Qh-pqeuTTJAv8QQ3X5aQRnHmZoyVuOP9Qqq05MGRPp6W-7Vdbi6mslDP-FwUnkHb2C-XenxHesqfcbqBOELa-PD6Fj_usVKPcL9HR_J4IK38XFFOT669_Xhpyaq6iRtvlmj1n_fQNvRcGpZIfIAFgf64cIwLAz2vimj5ywXneyDIRv5Wge8VyhfsAe9S01x0dNq-aR16clayKDn48e6fETeTWJJaPK7lvi1-Oc-tlaA7Pfk",
                                           "p": "8fDDtrup_sHpmpnAQ6arzA6S2zG23OlRwsrPQu1bByDJSAB6Y0RQDIxQB06NjNCyyoetpiUlblgpcLQpy-R9xAgdvw-gcxNYW4hxkQdbnSP0U9vv8V3tGtxN8szjBEsNz0vhs0Gc4Wz01IYGVY6OJhOg3qshAFrjZie49MsIRw_w0dJA6UAXbx19fvoFtKDSyp-w2FnFQHMnzwsoJULiCPB6R77XJZLx3gyQN4ad-s63E7f89055JNIO8rt-cigkL_ilQh-x9m-oX_nBWb_N4gjVdnyoH2Vjlq2os2l209URLPl4bq6AMCfe7SAnuyLA9U_aTiLOF2eAlbgznmUWjw",
                                           "q": "7xAWBXurm-Qv60KwKF2dqkGbCyNTrQv1Ep8L2ArTVeTMcQb54dQiwLZb-NDaiRIAyBXOsrkWEyh8tru5fvicn-EdAnajGTxEwwKktaUA-ufDw8vy4HOfATXGuA0DFt1L6nZ_n9lFYxd09iEUj0GwvaRM21A7nbOz3Qf6JThCaP8JOK3doOYsr7fufD8E9gcE7CiTcbbpm8BOa6-h2fY3tENCOV76a9_G2RfPOrvIu-tgVPU_2K-r8vSWQobXhGELOYc0XAipUxEAPxGLOq3T-OTQk2SvPnom2mEG_-5V1PG1sxH0WGf53XpUjjsUW1Zj9bidmuhLPqbMBf-ZhbLm9w",
                                           "qi": "3h-0vyBkKf0WsPhCiIWXm8yPU-J_qgM4JjkrBzwWOU6CpnQyIdoSfStXhas7ehoODnb7sAQ2PEh4RBls2kJrXmzqC_0wsUEFdnfSERsf8eb8Sgu1NF5JgqRU9UBe9-4bvLZAcFpBG_PJyMg8QhCEuqFbjrjNjg9r3EVfeRHyjl7dPEGF5RwIUiGbkyLmXmcqf0L4GhuvqGdd56krht_kvwI-lppMB4OoZDgwfOH0fcKuHJUyc0RyfZ-iS9YZTQI-1AO2gA-RsjCjvtZB2QBdhvqPp2OujkYlcGXukSBtoBsU3elGBqFPRqlPsDMN8dj0bw_xjNxue7fKh9a68CPedA",
                                           "kid": "B2C61A07EE0661237D19BEE1E0A1463C"}
                                         """;

    // This client_id is only to be used for this particular sample. Your application will use it's own client_id.
    private const string ClientId = "f7cd1256-0526-4b5a-b4c3-f054c984ace8";

    // The client is configured in the HelseID test environment, so we will point to that
    private const string StsUrl = "https://helseid-sts.test.nhn.no";

    private const int LocalhostPort = 8080;

    // In a test environment, the port does not need to be pre-registered in HelseID Selvbetjening;
    // this means that you can allocate any available port when launching the application:
    private static readonly string RedirectUrl = $"http://localhost:{LocalhostPort.ToString()}/callback";

    // This is the scope of the API you want to call (get an access token for)
    private const string ApiScopes = "nhn:test-public-samplecode/authorization-code";

    // These scopes indicate that you want an ID-token ("openid"), and what information about the user you want the ID-token to contain
    private const string IdentityScopes = "openid profile offline_access helseid://scopes/identity/pid helseid://scopes/identity/security_level";

    static async Task Main(string[] args)
    {
        try
        {
            using var httpClient = new HttpClient();

            // Download the HelseID metadata from https://helseid-sts.test.nhn.no/.well-known/openid-configuration to determine endpoints and public keys used by HelseID:
            // In a production environment, this document must be cached for better efficiency (both for this client and for HelseID)
            var disco = await httpClient.GetDiscoveryDocumentAsync(StsUrl);

            var options = new OidcClientOptions
            {
                Authority = StsUrl,
                ClientId = ClientId,
                RedirectUri = RedirectUrl,
                LoadProfile = false,
            };

            // Set the DPoP proof, we can use the same key for this as for the client assertion:
            options.ConfigureDPoP(JwkPrivateKey);

            // Setup the oidc client for authentication against HelseID
            var oidcClient = new OidcClient(options);

            // The authorizeState object contains the state that needs to be held between starting the authorize request and the response
            var authorizeState = await oidcClient.PrepareLoginAsync();

            var pushedAuthorizationResponse = await GetPushedAuthorizationResponse(
                httpClient,
                authorizeState,
                disco);

            if (pushedAuthorizationResponse.IsError)
            {
                throw new Exception($"{pushedAuthorizationResponse.Error}: JSON: {pushedAuthorizationResponse.Json}");
            }

            var startUrl = $"{disco.AuthorizeEndpoint}?client_id={ClientId}&request_uri={pushedAuthorizationResponse.RequestUri}";

            var browserOptions = new BrowserOptions(startUrl, RedirectUrl);

            // Create a redirect URI using an available port on the loopback address.
            var browser = new SystemBrowser(port: LocalhostPort);

            var browserResult = await browser.InvokeAsync(browserOptions, CancellationToken.None);

            // If the result type is success, the browser result should contain the authorization code.
            // We can now call the /token endpoint with the authorization code in order to get tokens:

            // We need a client assertion on the request in order to authenticate the client:
            oidcClient.Options.ClientAssertion = GetClientAssertionPayload(disco);

            var loginResult = await oidcClient.ProcessResponseAsync(browserResult.Response, authorizeState);

            if (loginResult.IsError == false)
            {
                loginResult = ValidateIdentityClaims(loginResult);
            }

            if (loginResult.IsError)
            {
                throw new Exception($"{loginResult.Error}: Description: {loginResult.ErrorDescription}");
            }

            // The access token can now be used when calling an api
            // It contains the user id, the security level, the organization number
            // and the child organization
            // Copy the access token and paste it at https://jwt.ms to decode it

            Console.WriteLine($"Identity token from login: {loginResult.IdentityToken}");
            Console.WriteLine($"Access token from login: {loginResult.AccessToken}");

            // We can now call the /token endpoint again with the refresh token in order to get a new access token:
            // Client assertions cannot be used twice, so we need a new payload:
            oidcClient.Options.ClientAssertion = GetClientAssertionPayload(disco);

            var refreshTokenResult = await oidcClient.RefreshTokenAsync(loginResult.RefreshToken);

            if (refreshTokenResult.IsError)
            {
                throw new Exception($"{refreshTokenResult.Error}: Description: {refreshTokenResult.ErrorDescription}");
            }

            Console.WriteLine();
            Console.WriteLine("New Access Token after using Refresh Token:");
            Console.WriteLine(refreshTokenResult.AccessToken);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync("Error:");
            await Console.Error.WriteLineAsync(ex.ToString());
        }
    }

    private static async Task<PushedAuthorizationResponse> GetPushedAuthorizationResponse(
        HttpClient httpClient,
        AuthorizeState authorizeState,
        DiscoveryDocumentResponse disco)
    {
        // Setup a client assertion - this will authenticate the client (this application)
        var clientAssertionPayload = GetClientAssertionPayload(disco);
        var pushedAuthorizationRequest = new PushedAuthorizationRequest
        {
            Address = disco.PushedAuthorizationRequestEndpoint,
            Parameters = GetParametersForPushedAuthorizationRequest(
                clientAssertionPayload,
                authorizeState,
                disco),
        };

        // Calls the /par endpoint in order to get a request URI for the /authorize endpoint
        return await httpClient.PushAuthorizationAsync(pushedAuthorizationRequest);
    }

    // This is necessary in order to please the IdentityModel library:
    private static Parameters GetParametersForPushedAuthorizationRequest(
        ClientAssertion clientAssertionPayload,
        AuthorizeState authorizeState,
        DiscoveryDocumentResponse disco)
    {
        // Sets the pushed authorization request parameters:
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(authorizeState.CodeVerifier));
        var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);
        var requestObject = BuildRequestObject(disco);

        var dictionary = new Dictionary<string, string>
        {
            { "client_id", ClientId },
            { "client_assertion", clientAssertionPayload.Value },
            { "client_assertion_type", clientAssertionPayload.Type },
            { "request", requestObject },
            { "scope", ApiScopes + " " + IdentityScopes },
            { "redirect_uri", RedirectUrl},
            { "response_type", OidcConstants.ResponseTypes.Code },
            { "code_challenge", codeChallenge },
            { "code_challenge_method", OidcConstants.CodeChallengeMethods.Sha256 },
            { "state", authorizeState.State },
        };
        return new Parameters(dictionary);
    }

    private static string BuildRequestObject(DiscoveryDocumentResponse disco)
    {
        // Builds a request object containing the authorization details claim with the following structure:
        // (This type of structure indicates a multi-tenant client)
        //        "authorization_details":{
        //            "type":"helseid_authorization",
        //            "practitioner_role":{
        //                "organization":{
        //                    "identifier": {
        //                        "system":"urn:oid:1.0.652301",
        //                        "type":"ENH",
        //                        "value":"NO:ORGNR:[parent organization number]:[child organization number]"
        //                    }
        //                }
        //            }
        //        }


        var orgNumberDetails = new Dictionary<string, string>
        {
            {"system", "urn:oid:1.0.6523"},
            {"type", "ENH"},
            {
                // The client configuration in HelseID contains a delegation from the supplier of this system to the
                // requested organization number. If this delegation is not set, the request will be invalidated.
                "value", "NO:ORGNR:994598759:123456789"
            }
        };

        var identifier = new Dictionary<string, object>
        {
            {"identifier", orgNumberDetails}
        };

        var organization = new Dictionary<string, object>
        {
            {"organization", identifier}
        };

        var authorizationDetails = new Dictionary<string, object>
        {
            {"type", "helseid_authorization"},
            {"practitioner_role", organization}
        };

        var serialized = JsonSerializer.Serialize(authorizationDetails);

        var claims = new List<Claim>
        {
            new Claim("authorization_details", serialized, "json"),
            new Claim(JwtClaimTypes.Subject, ClientId),
            new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };

        return BuildHelseIdJwt(disco, claims);
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

        return BuildHelseIdJwt(disco, claims);
    }

    private static string BuildHelseIdJwt(DiscoveryDocumentResponse disco, List<Claim> extraClaims)
    {
        var payload = new JwtPayload(ClientId, disco.Issuer, extraClaims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(30));
        var header = new JwtHeader(GetClientAssertionSigningCredentials(), null,  tokenType: "client-authentication+jwt");
            
        var credentials = new JwtSecurityToken(header, payload);
        
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
