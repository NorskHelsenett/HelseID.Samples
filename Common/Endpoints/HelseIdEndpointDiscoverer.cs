using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;

namespace HelseId.Samples.Common.Endpoints;

// This class discovers endpoints on HelseID to use for (e.g. token) requests to HelseID
public class HelseIdEndpointDiscoverer : IHelseIdEndpointDiscoverer
{
    private const string TokenEndpoint = "TokenEndpoint";
    private const string ClientInfoEndpoint = "ClientInfoEndpoint";
    private const int EndpointCacheExpirationInHours = 24;
    private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions
    {
        ExpirationScanFrequency = TimeSpan.FromHours(EndpointCacheExpirationInHours)
    });

    public HelseIdEndpointDiscoverer(string stsUrl)
    {
        StsUrl = stsUrl;
    }

    public string StsUrl { get; }

    public async Task<string> GetTokenEndpointFromHelseId()
    {
        if (_memoryCache.TryGetValue(TokenEndpoint, out string? result))
        {
            return result!;
        }
       
        // The endpoint is not present in the cache; use the discovery document
        var disco = await GetDiscoveryDocument();
        _memoryCache.Set(TokenEndpoint, disco.TokenEndpoint);
        return disco.TokenEndpoint;
    }
    
    public async Task<string> GetClientInfoEndpointFromHelseId()
    {
        if (_memoryCache.TryGetValue(ClientInfoEndpoint, out string? result))
        {
            return result!;
        }

        var disco = await GetDiscoveryDocument();
        var clientInfoUrl = disco.TryGetString(HelseIdConstants.ClientInfoEndpointName);
        if (string.IsNullOrEmpty(clientInfoUrl))
        {
            throw new Exception($"Unable to get the URL of the clientInfo endpoint. Looking for '{HelseIdConstants.ClientInfoEndpointName}'");
        }
        _memoryCache.Set(TokenEndpoint, clientInfoUrl);
        return clientInfoUrl;
    }

    private async Task<DiscoveryDocumentResponse> GetDiscoveryDocument()
    {
        using var httpClient = new HttpClient();
        // This extension from the IdentityModel library calls the discovery document on the HelseID server
        var disco = await httpClient.GetDiscoveryDocumentAsync(StsUrl);
        if (disco.IsError)
        {
            throw new Exception(disco.Error);
        }
        return disco;
    }
}