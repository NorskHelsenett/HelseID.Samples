using Microsoft.Extensions.Caching.Distributed;

namespace HelseId.Samples.Common.ApiDPoPValidation;

public class DummyReplayCache : IReplayCache
{
    private const string Prefix = "DummyReplayCache-";

    private readonly IDistributedCache _cache;

    public DummyReplayCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task AddAsync(string purpose, string handle, DateTimeOffset expiration)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expiration
        };

        await _cache.SetAsync(Prefix + purpose + handle, new byte[] { }, options);
    }

    public async Task<bool> ExistsAsync(string purpose, string handle)
    {
        return (await _cache.GetAsync(Prefix + purpose + handle)) != null;
    }
}