using HelseId.Samples.Common.Interfaces;

namespace HelseId.Samples.ClientCredentials.ClientInfoEndpoint;

/// <summary>
/// A 'null-value' instance of an IClientInfoRetriever. Does nothing; used in the basic scenario.
/// </summary>
public class NullClientInfoRetriever : IClientInfoRetriever
{
    public Task ConsumeClientinfoEndpoint(HttpClient httpClient, string accessToken)
    {
        return Task.CompletedTask;
    }
}