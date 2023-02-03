using HelseId.Samples.Common.Interfaces.Endpoints;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;

namespace HelseId.Samples.Common.Endpoints;

// This class encapsulates the 'GetDiscoveryDocumentAsync' method from
// the IdentityModel library, and caches the results
public class DiscoveryDocumentGetter : IDiscoveryDocumentGetter
{
    private const string DiscoveryDocumentKey = "DiscoveryDocument";
    private const int DiscoveryDocumentCacheExpirationInHours = 24;

    private readonly IMemoryCache _memoryCache;
    private readonly string _stsUrl;

    public DiscoveryDocumentGetter(string stsUrl)
    {
        _stsUrl = stsUrl;
        _memoryCache = new MemoryCache(new MemoryCacheOptions
        {
            ExpirationScanFrequency = TimeSpan.FromHours(DiscoveryDocumentCacheExpirationInHours)
        });
    }

    public async Task<DiscoveryDocumentResponse> GetDiscoveryDocument()
    {
        if (_memoryCache.TryGetValue(DiscoveryDocumentKey, out DiscoveryDocumentResponse? result))
        {
            return result!;
        }

        return await UpdateCacheWithNewDocument();
    }

    private async Task<DiscoveryDocumentResponse> UpdateCacheWithNewDocument()
    {
        var discoveryDocument = await CallTheMetadataUrl();

        _memoryCache.Set(DiscoveryDocumentKey, discoveryDocument);

        return discoveryDocument;
    }

    private async Task<DiscoveryDocumentResponse> CallTheMetadataUrl()
    {
        using var httpClient = new HttpClient();
        // This extension from the IdentityModel library calls the discovery document on the HelseID server
        var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(_stsUrl);
        if (discoveryDocument.IsError)
        {
            throw new Exception(discoveryDocument.Error);
        }
        return discoveryDocument;
    }
}