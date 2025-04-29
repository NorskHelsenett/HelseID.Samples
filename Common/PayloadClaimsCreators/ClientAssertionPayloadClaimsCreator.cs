using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.Common.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

// This class generates the (general) claims that are required for the token that is sent
// to HelseID as part of a client assertion (to the PAR endpoint, or to the Token endpoint)
public class ClientAssertionPayloadClaimsCreator : IPayloadClaimsCreatorForClientAssertion
{
    private readonly IDateTimeService _dateTimeService;

    public ClientAssertionPayloadClaimsCreator(IDateTimeService dateTimeService)
    {
        _dateTimeService = dateTimeService;
    }

    public IEnumerable<PayloadClaim> CreatePayloadClaims(
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration)
    {
        // Time values are converted to epoch (UNIX) time format
        var tokenIssuedAtEpochTime = EpochTime.GetIntDate(_dateTimeService.UtcNow);
        // This class contains JSON objects representing the claims contained in the JWT.
        return new List<PayloadClaim>
        {
            // See https://www.rfc-editor.org/rfc/rfc7523#section-3 for a further description of these claims
            // "iss": the issuer claim; in our case, the value is the client ID
            new(JwtRegisteredClaimNames.Iss, configuration.ClientId),
            // "sub" (subject): a unique identifier for the End-User at the Issuer. HelseID expects this to be the client ID.
            new(JwtRegisteredClaimNames.Sub, configuration.ClientId),
            // "aud" (audience): the audience for our client assertion is the HelseID server
            new(JwtRegisteredClaimNames.Aud, configuration.StsUrl),
            // "exp" (expires at): this describes the end of the token usage period
            new(JwtRegisteredClaimNames.Exp, tokenIssuedAtEpochTime + JwtClaimsCreator.TokenExpirationTimeInSeconds),
            // "iat" (issued at time): this describes the time when the token was issued
            new(JwtRegisteredClaimNames.Iat, tokenIssuedAtEpochTime),
            // "nbf" (not before): this describes the time when the token can be used
            new(JwtRegisteredClaimNames.Nbf, tokenIssuedAtEpochTime),
            // "jti" a unique identifier for the token, which can be used to prevent reuse of the token.
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };
    }
}
