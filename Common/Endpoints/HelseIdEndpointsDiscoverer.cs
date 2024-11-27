using HelseId.Samples.Common.Interfaces.Endpoints;

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
}