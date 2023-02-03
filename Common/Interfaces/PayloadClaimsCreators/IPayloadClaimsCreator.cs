using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;

public interface IPayloadClaimsCreator
{
    IEnumerable<PayloadClaim> CreatePayloadClaims(
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration);
}

// Instances of this interface create a payload for use in a client assertion
public interface IPayloadClaimsCreatorForClientAssertion : IPayloadClaimsCreator { }

// Instances of this interface create a payload for use in a request object
public interface IPayloadClaimsCreatorForRequestObjects : IPayloadClaimsCreator { }