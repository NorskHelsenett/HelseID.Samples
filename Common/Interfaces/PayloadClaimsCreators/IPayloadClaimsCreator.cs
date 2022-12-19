using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;

public interface IPayloadClaimsCreator
{
    IEnumerable<PayloadClaim> CreatePayloadClaims(
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration);
}

public interface IPayloadClaimsCreatorForClientAssertion : IPayloadClaimsCreator { }

public interface IPayloadClaimsCreatorForRequestObjects : IPayloadClaimsCreator { }