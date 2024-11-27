using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface IJwtClaimsCreator
{
    Dictionary<string, object> CreateJwtClaims(
        IPayloadClaimsCreator payloadClaimsCreator,
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration);
}
