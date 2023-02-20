namespace HelseId.Samples.Common.Interfaces;

public interface IClientInfoRetriever
{
    Task ConsumeClientinfoEndpoint(HttpClient httpClient, string accessToken);
}