using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Endpoints;

// This class discovers endpoints on HelseID to use for (e.g. token) requests to HelseID
public class HelseIdEndpointsDiscoverer : IHelseIdEndpointsDiscoverer
{
    //TODO: Lag som extension metoder i stedet
    public HelseIdEndpointsDiscoverer(IDiscoveryDocumentGetter discoveryDocumentGetter)
    {
        _discoveryDocumentGetter = discoveryDocumentGetter;
    }

    private readonly IDiscoveryDocumentGetter _discoveryDocumentGetter;

    public async Task<string> GetTokenEndpointFromHelseId()
    {
        var disco = await _discoveryDocumentGetter.GetDiscoveryDocument();
        return disco.TokenEndpoint!;
    }
    
    public async Task<string> GetClientInfoEndpointFromHelseId()
    {
        var disco = await _discoveryDocumentGetter.GetDiscoveryDocument();
        var clientInfoUrl = disco.TryGetString(HelseIdConstants.ClientInfoEndpointName);
        if (string.IsNullOrEmpty(clientInfoUrl))
        {
            throw new Exception($"Unable to get the URL of the clientInfo endpoint. Looking for '{HelseIdConstants.ClientInfoEndpointName}'");
        }
        return clientInfoUrl;
    }
}