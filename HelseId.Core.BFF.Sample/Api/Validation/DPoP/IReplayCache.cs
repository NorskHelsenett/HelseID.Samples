using System;
using System.Threading.Tasks;

namespace HelseId.Core.BFF.Sample.Api.DPoP;

public interface IReplayCache
{
    Task AddAsync(string purpose, string handle, DateTimeOffset expiration);

    Task<bool> ExistsAsync(string purpose, string handle);
}
