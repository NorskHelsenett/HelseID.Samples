namespace HelseId.Samples.Common.Interfaces.Endpoints;

public interface IHelseIdEndpointsDiscoverer
{
    Task<string> GetTokenEndpointFromHelseId();
    
    Task<string> GetClientInfoEndpointFromHelseId();
}