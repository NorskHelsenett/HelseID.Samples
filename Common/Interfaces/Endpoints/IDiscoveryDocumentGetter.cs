using IdentityModel.Client;

namespace HelseId.Samples.Common.Interfaces.Endpoints;

public interface IDiscoveryDocumentGetter
{
    Task<DiscoveryDocumentResponse> GetDiscoveryDocument();
}