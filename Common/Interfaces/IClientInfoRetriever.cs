namespace HelseId.Samples.Common.Interfaces;

public interface IClientInfoRetriever
{
    Task ConsumeClientInfoEndpoint(HttpClient httpClient, string accessToken);
}