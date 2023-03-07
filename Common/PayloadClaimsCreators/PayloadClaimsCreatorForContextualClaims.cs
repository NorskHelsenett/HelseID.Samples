using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

public class PayloadClaimsCreatorForContextualClaims : IPayloadClaimsCreator
{
    // See https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/45973505/How+to+submit+organizational+information+to+HelseID+machine-to-machine
    public IEnumerable<PayloadClaim> CreatePayloadClaims(PayloadClaimParameters payloadClaimParameters, HelseIdConfiguration configuration)
    {
        var authorizationDetails = new
        {
            type = payloadClaimParameters.ContextualClaimType,
            value = new
            {
                user_details = new
                {
                    logon_identity = "e2d0bc68-028b-4f9f-8c28-f534cc3baca1",
                }
            }
        };

        return new List<PayloadClaim> {
            new PayloadClaim("authorization_details", authorizationDetails),
        };    
    }
}