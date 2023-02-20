using HelseId.Samples.Common.Interfaces;
using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using IdentityModel.Client;

namespace HelseId.Samples.ClientCredentials.Client;

public class Machine2MachineClient
{
    private ITokenRequestBuilder _tokenRequestBuilder;
    private IApiConsumer _apiConsumer;
    private ClientCredentialsTokenRequestParameters _tokenRequestParameters;
    private IExpirationTimeCalculator _expirationTimeCalculator;
    private DateTime _persistedAccessTokenExpiresAt = DateTime.MinValue;
    private string _persistedAccessToken = string.Empty;
    private readonly IPayloadClaimsCreatorForClientAssertion _payloadClaimsCreatorForClientAssertion;
    // Can be used for debugging purposes:
    private IClientInfoRetriever _clientInfoRetriever;
    
    public Machine2MachineClient(
        IApiConsumer apiConsumer,
        ITokenRequestBuilder tokenRequestBuilder,
        IClientInfoRetriever clientInfoRetriever,
        IExpirationTimeCalculator expirationTimeCalculator,
        IPayloadClaimsCreatorForClientAssertion payloadClaimsCreatorForClientAssertion,
        ClientCredentialsTokenRequestParameters tokenRequestParameters)
    {
        _tokenRequestBuilder = tokenRequestBuilder;
        _apiConsumer = apiConsumer;
        // The client info retriever can be used for debugging purposes.
        // When activated, it accesses the client info endpoint on the HelseID service,
        // which returns info about the client that was used to get an access token.
        _clientInfoRetriever = clientInfoRetriever;
        _expirationTimeCalculator = expirationTimeCalculator;
        _payloadClaimsCreatorForClientAssertion = payloadClaimsCreatorForClientAssertion;
        _tokenRequestParameters = tokenRequestParameters;
    }

    public async Task CallApiWithToken()
    {
        using var httpClient = new HttpClient();

        // 1: get the token
        var accessToken = await GetAccessToken(httpClient);

        // 2: (optional) get information on the client
        await _clientInfoRetriever.ConsumeClientinfoEndpoint(httpClient, accessToken);

        // 3: consume the API
        await CallApi(httpClient, accessToken);
    }

    private async Task<string> GetAccessToken(HttpClient httpClient)
    {
        if (DateTime.UtcNow > _persistedAccessTokenExpiresAt)
        {
            var tokenResponse = await GetAccessTokenFromHelseId(httpClient);
            _persistedAccessTokenExpiresAt = _expirationTimeCalculator.CalculateTokenExpirationTimeUtc(tokenResponse.ExpiresIn);
            _persistedAccessToken = tokenResponse.AccessToken;
        }
        else
        {
            Console.WriteLine("The access token has not yet expired, no call was made to HelseID for a new token.");
        }
        return _persistedAccessToken;
    }

    private async Task<TokenResponse> GetAccessTokenFromHelseId(HttpClient httpClient)
    {
        // The request to HelseID is created:
        var request = await _tokenRequestBuilder.CreateClientCredentialsTokenRequest(_payloadClaimsCreatorForClientAssertion, _tokenRequestParameters);

        // We use the HTTP client to retrieve the response from HelseID:
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(request);

        if (tokenResponse.IsError)
        {
            await WriteErrorToConsole(tokenResponse);
            throw new Exception();
        }
        
        WriteAccessTokenFromTokenResult(tokenResponse);

        return tokenResponse;
    }

    private async Task WriteErrorToConsole(TokenResponse tokenResponse) {
        await Console.Error.WriteLineAsync("An error occured:");
        await Console.Error.WriteLineAsync(tokenResponse.Error);
    }

    private void WriteAccessTokenFromTokenResult(TokenResponse tokenResponse) {
        Console.WriteLine("The application received this access token from HelseID:");
        Console.WriteLine(tokenResponse.AccessToken);
        Console.WriteLine("Copy/paste the access token string at https://jwt.ms to see the contents");
        Console.WriteLine("------");
    }

    private async Task CallApi(HttpClient httpClient, string accessToken)
    {
        try
        {
            Console.WriteLine("Using the access token to call the sample API");
            var response = await _apiConsumer.CallApi(httpClient, ConfigurationValues.SampleApiUrlForM2M, accessToken);
            var notPresent = "<not present>";
            Console.WriteLine($"Response from the sample API: {response?.Greeting}, org.nr: {response?.OrganizationNumber}, underenhet: {response?.ChildOrganizationNumber ?? notPresent}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"The sample API did not accept the request at address '{ConfigurationValues.SampleApiUrlForM2M}'. (Have you started the sample API application (in the 'SampleAPI' folder)?");
            Console.WriteLine($"Error message: '{e.Message}'");
        }
    }
}