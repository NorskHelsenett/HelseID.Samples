using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

public class NullPayloadClaimsCreatorForRequestObjects : IPayloadClaimsCreatorForRequestObjects
{
    public IEnumerable<PayloadClaim> CreatePayloadClaims(PayloadClaimParameters payloadClaimParameters)
    {
        return new List<PayloadClaim>();
    }
}