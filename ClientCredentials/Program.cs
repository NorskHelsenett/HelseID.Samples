using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HelseID.Samples.Configuration;
using HelseID.Samples.Extensions;

namespace HelseId.Samples.ClientCredentials;

/// <summary>
/// This sample shows how to use the client credential grant to get an access token from HelseID.
/// </summary>
static class Program
{
    static async Task Main()
    {
        var configuration = HelseIdSamplesConfiguration.ConfigurationForClientCredentialsClient;

        // Two builder classes are used (each in its own file in this directory):
        //   * ClientCredentialsClientAssertionsBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        //   * ClientCredentialsClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        var clientCredentialsTokenRequestBuilder =
            new ClientCredentialsTokenRequestBuilder(
                new ClientCredentialsClientAssertionsBuilder(configuration),
                configuration);

        using var client = new HttpClient();

        var request = await clientCredentialsTokenRequestBuilder.CreateClientCredentialsTokenRequest(client);

        var result = await client.RequestClientCredentialsTokenAsync(request);

        if (result.IsError)
        {
            await result.WriteErrorToConsole();
        }
        else
        {
            result.WriteAccessTokenFromTokenResult();
        }
    }
}