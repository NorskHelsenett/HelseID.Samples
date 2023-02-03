using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface ISigningJwtTokenCreator
{
    string CreateSigningToken(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters);
}