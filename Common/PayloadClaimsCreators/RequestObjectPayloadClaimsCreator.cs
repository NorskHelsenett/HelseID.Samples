using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.Common.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

// This class generates the claims that are required for the token that is sent
// to HelseID as a request object (to the PAR endpoint)
public class RequestObjectPayloadClaimsCreator : IPayloadClaimsCreatorForRequestObjects
{
    private readonly IDateTimeService _dateTimeService;

    public RequestObjectPayloadClaimsCreator(IDateTimeService dateTimeService)
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
            // See https://utviklerportal.nhn.no/informasjonstjenester/helseid/bruksmoenstre-og-eksempelkode/bruk-av-helseid/docs/tekniske-mekanismer/organisasjonsnumre_no_nbmd/,
            // and also https://openid.net/specs/openid-connect-core-1_0.html#RequestObject for a further description of these claims
            // "iss": the issuer claim; in our case, the value is the client ID
            new(JwtRegisteredClaimNames.Iss, configuration.ClientId),
            // "aud" (audience): the audience for our client assertion is the HelseID server
            new(JwtRegisteredClaimNames.Aud, configuration.StsUrl),
            // "exp" (expires at): this describes the end of the token usage period
            new(JwtRegisteredClaimNames.Exp, tokenIssuedAtEpochTime + JwtClaimsCreator.TokenExpirationTimeInSeconds),
            // "nbf" (not before): this describes the start of the token usage period
            new(JwtRegisteredClaimNames.Nbf, tokenIssuedAtEpochTime),
            // "client_id": the value of the current client ID
            new(HelseIdConstants.ClientIdClaimName, configuration.ClientId),
            // "jti" a unique identifier for the token, which can be used to prevent reuse of the token.
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        };
    }
}
