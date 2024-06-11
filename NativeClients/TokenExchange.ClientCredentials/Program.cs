using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;

namespace HelseID.Samples.TokenExchange.ClientCredentials;

public class Program
{
    private const string SubjectClientId = "token_exchange_subject_client";
    private const string ActorClientId = "token_exchange_actor_client";
    private const string StsUrl = "https://helseid-sts.test.nhn.no/";

    private static DiscoveryDocumentResponse _discoveryDocument = default!;

    static async Task Main()
    {
        try
        {
            _discoveryDocument = await new HttpClient().GetDiscoveryDocumentAsync(StsUrl);
            if (_discoveryDocument.IsError)
            {
                throw new Exception(_discoveryDocument.Error);
            }

            // Subject Client system requests a initial access token
            // Here the client system calls the api and the api picks up the token
            var subjectAccessToken = await GetSubjectAccessTokenUsingClientCredentials();
            if (string.IsNullOrEmpty(subjectAccessToken))
            {
                return;
            }

            // At this point, the client would use the subjectAccessToken as a bearer token to call the API.

            ///////////////////////////////////////////////////////////////////
            // Everything below this line would be done by the API (the actor)
            // that receives the call from the client (our subject)
            ///////////////////////////////////////////////////////////////////

            // Perform the token exchange process as a separate client
            var exchangeResponse = await PerformTokenExchange(subjectAccessToken);
            if (exchangeResponse.IsError)
            {
                Console.WriteLine($"Error exchanging subject token: {@exchangeResponse.Error}");
                return;
            }

            var exchangedAccessToken = exchangeResponse.AccessToken;

            Console.WriteLine("Original (subject) access token:");
            Console.WriteLine(subjectAccessToken);
            Console.WriteLine();
            Console.WriteLine("Exchanged access token:");
            Console.WriteLine(exchangedAccessToken);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine("Error:");
            Console.Error.WriteLine(e.ToString());
        }
    }

    private static async Task<string> GetSubjectAccessTokenUsingClientCredentials()
    {
        // Get an access tokens using client credentials
        // This is the token we want to exchange later on
        // udelt:token_exchange_actor_api/scope

        var clientAssertion = BuildClientAssertion(SubjectClientId, _discoveryDocument, GetClientAssertionSecurityKey());

        var request = new ClientCredentialsTokenRequest
        {
            Address = _discoveryDocument.TokenEndpoint,
            ClientId = SubjectClientId,
            Scope = "nhn:helseid-public-samplecode-for-token-exchange/token-exhange-endpoint",
            ClientAssertion = new ClientAssertion
            {
                Value = clientAssertion,
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
            },
            ClientCredentialStyle = ClientCredentialStyle.PostBody
        };

        var result = await new HttpClient().RequestClientCredentialsTokenAsync(request);

        if (result.IsError)
        {
            Console.Error.WriteLine("Error retrieving subject access token:");
            Console.Error.WriteLine(result.Error);
            return String.Empty;
        }

        return result.AccessToken!;
    }

    private static async Task<TokenResponse> PerformTokenExchange(string subjectToken)
    {
        // Perform the token exchange
        // To do a token exchange HelseID requires that an enterprise certificate is used as the client secret
        // Also note the SubjectToken and SubjectTokenType parameters
        var request = new TokenExchangeTokenRequest
        {
            Address = _discoveryDocument.TokenEndpoint,
            ClientAssertion = new ClientAssertion
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = BuildClientAssertion(ActorClientId, _discoveryDocument, GetClientAssertionSecurityKey())
            },
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            ClientId = ActorClientId,
            Scope = "nhn:helseid-public-sample-api-1/some-scope",
            SubjectToken = subjectToken,
            SubjectTokenType = OidcConstants.TokenTypeIdentifiers.AccessToken
        };

        return await new HttpClient().RequestTokenExchangeTokenAsync(request);
    }

    private static string BuildClientAssertion(string clientId, DiscoveryDocumentResponse disco,
        SecurityKey securityKey)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtClaimTypes.Subject, clientId),
            new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
            new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        var token = new JwtSecurityToken(clientId, disco.TokenEndpoint, claims, DateTime.UtcNow,
            DateTime.UtcNow.AddSeconds(60), signingCredentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static SecurityKey GetClientAssertionSecurityKey()
    {
        var jwk = File.ReadAllText("jwk.json");
        return new JsonWebKey(jwk);
    }
}
