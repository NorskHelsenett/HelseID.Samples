using System.IdentityModel.Tokens.Jwt;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface IJwtPayloadCreator
{
    JwtPayload CreateJwtPayload(
        IPayloadClaimsCreator payloadClaimsCreator,
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration);
}