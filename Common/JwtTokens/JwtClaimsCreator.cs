using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.JwtTokens;

// This class creates a payload with claims for use in a token request or a request object
public class JwtClaimsCreator : IJwtClaimsCreator
{
    public const int TokenExpirationTimeInSeconds = 30;

    public Dictionary<string, object> CreateJwtClaims(
        IPayloadClaimsCreator payloadClaimsCreator,
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration)
    {
        // The payload for the jwt gets set up with the required claims:
        var claims = new Dictionary<string, object>();

        // The PayloadClaimsCreator creates any extra claims we might need to send to HelseID.
        foreach (var payloadClaim in payloadClaimsCreator.CreatePayloadClaims(payloadClaimParameters, configuration))
        {
            claims[payloadClaim.Name] = payloadClaim.Value;
        }

        return claims;
    }
}
