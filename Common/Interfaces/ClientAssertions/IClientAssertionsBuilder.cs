using Duende.IdentityModel.Client;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.ClientAssertions;

public interface IClientAssertionsBuilder
{
    ClientAssertion BuildClientAssertion(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters);
}