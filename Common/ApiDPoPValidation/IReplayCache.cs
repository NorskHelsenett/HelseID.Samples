namespace HelseId.Samples.Common.ApiDPoPValidation;

public interface IReplayCache
{
    Task AddAsync(string purpose, string handle, DateTimeOffset expiration);

    Task<bool> ExistsAsync(string purpose, string handle);
}