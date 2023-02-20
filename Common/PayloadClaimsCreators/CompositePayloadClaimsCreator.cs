using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

// The double interface is a hack to make this class compatible as an instance of both
// IPayloadClaimsCreatorForClientAssertion and IPayloadClaimsCreatorForRequestObjects.
// This is done because the default ASP.NET Core injection framework does not have a
// pattern for injecting named types into services.
public class CompositePayloadClaimsCreator : IPayloadClaimsCreatorForClientAssertion, IPayloadClaimsCreatorForRequestObjects
{
    private readonly List<IPayloadClaimsCreator> _instances;

    public CompositePayloadClaimsCreator(List<IPayloadClaimsCreator> instances)
    {
        _instances = instances;
    }

    public IEnumerable<PayloadClaim> CreatePayloadClaims(PayloadClaimParameters payloadClaimParameters, HelseIdConfiguration configuration)
    {
        var result = new List<PayloadClaim>();
        foreach (var payloadClaimsCreator in _instances)
        {
            result.AddRange(payloadClaimsCreator.CreatePayloadClaims(payloadClaimParameters, configuration));
        }
        return result;
    }
}