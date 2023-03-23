using System.IdentityModel.Tokens.Jwt;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.JwtTokens;

// This class creates a JwtPayload instance for use in a token request or a request object
public class JwtPayloadCreator : IJwtPayloadCreator
{
    public const int TokenExpirationTimeInSeconds = 30;

    public JwtPayload CreateJwtPayload(
        IPayloadClaimsCreator payloadClaimsCreator,
        PayloadClaimParameters payloadClaimParameters)
    {
        // The payload for the jwt gets set up with the required claims:
        var payload = new JwtPayload();

        // The PayloadClaimsCreator creates any extra claims we might need to send to HelseID.
        foreach (var payloadClaim in payloadClaimsCreator.CreatePayloadClaims(payloadClaimParameters))
        {
            payload[payloadClaim.Name] = payloadClaim.Value;
        }

        return payload;
    }
}
