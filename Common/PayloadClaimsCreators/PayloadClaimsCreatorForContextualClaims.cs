using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

public class PayloadClaimsCreatorForContextualClaims : IPayloadClaimsCreator
{
    // See https://helseid.atlassian.net/wiki/spaces/HELSEID/pages/491356176/Passing+extended+context+information+as+a+Client
    public IEnumerable<PayloadClaim> CreatePayloadClaims(PayloadClaimParameters payloadClaimParameters, HelseIdConfiguration configuration)
    {
          var authorizationDetails = new
          {
            type = "nhn:tillitsrammeverk:parameters",
            practitioner = new
            {
              authorization = new
              {
                code = "AA",
                system = "urn:oid:2.16.578.1.12.4.1.1.9060",
              },
              legal_entity = new
              {
                id = "111222333",
                system = "urn:oid:2.16.578.1.12.4.1.4.101"
              },
              point_of_care = new
              {
                id = "111222333",
                system = "urn:oid:2.16.578.1.12.4.1.4.101",
              },
              department = new
              {
                id = "111222333",
                system = "urn:oid:2.16.578.1.12.4.1.4.102",
              },
            },
            care_relationship = new
            {
              healthcare_service = new
              {
                code = "S03",
                system = "urn:oid:2.16.578.1.12.4.1.1.8655",
              },
              purpose_of_use = new
              {
                code = "TREAT",
                system = "urn:oid:2.16.840.1.113883.1.11.20448"
              },
              purpose_of_use_details = new
              {
                code = "15",
                system = "urn:oid:2.16.578.1.12.4.1.1.9151"
              },
              decision_ref = new
              {
                ref_id = "30F4AB40-DBC2-41A7-8AC4-181AD3FDC25B",
                user_selected = true
              }

            },
          };

        return new List<PayloadClaim> {
            new PayloadClaim("authorization_details", authorizationDetails),
        };
    }
}
