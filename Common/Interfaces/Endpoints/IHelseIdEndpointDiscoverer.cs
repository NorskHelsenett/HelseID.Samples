namespace HelseId.Samples.Common.Interfaces.Endpoints;

public interface IHelseIdEndpointDiscoverer
{
    string StsUrl { get; }

    Task<string> GetTokenEndpointFromHelseId();
    
    Task<string> GetClientInfoEndpointFromHelseId();
}