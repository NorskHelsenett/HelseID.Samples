using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

/// <summary>
/// This class creates an "authorization_details" claim that has an internal
/// structure which HelseID can recognize.
///
/// The purpose of this is to request a claim in the access token that
/// contains a specified underenhet (child organization) for our call to
/// the Sample API.
///
/// The underenhet (child organization) requested must be present in a
/// list connected to the client in HelseID.
/// </summary>
public class PayloadClaimsCreatorForMultiTenantClient : IPayloadClaimsCreator {

    public IEnumerable<PayloadClaim> CreatePayloadClaims(
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration)
    {
        if (payloadClaimParameters.IsAuthCodeRequest)
        {
            return new List<PayloadClaim>();
        }

        if (string.IsNullOrEmpty(payloadClaimParameters.ParentOrganizationNumber))
        {
            throw new Exception("Need payload claim parameters with a parent organization number");
        }
        
        // When the client is of the multitenancy type, it will require a parent organization number claim.
        // In this case, HelseID will require an authorization details claim with the following structure:
        //
        //  "authorization_details":{
        //      "type":"helseid_authorization",
        //      "practitioner_role":{
        //          "organization":{
        //              "identifier": {
        //                  "system":"urn:oid:1.0.6523",
        //                  "type":"ENH",
        //                  "value":"NO:ORGNR:[parent orgnumber]:[child orgnumber]"
        //              }
        //          }
        //      }
        //  }
        // 
        // We use anonymous types to insert the structured claim into the payload:

        var authorizationDetails = new
        {
            type = "helseid_authorization",
            practitioner_role = new
            {
                organization = new
                {
                    identifier = new
                    {
                        system =  "urn:oid:1.0.6523",
                        type = "ENH",
                        value = $"NO:ORGNR:{GetOrganizationNumberValue(payloadClaimParameters)}"
                    }
                }
            }
        };

        return new List<PayloadClaim> {
            new PayloadClaim("authorization_details", authorizationDetails),
        };
    }

    private string GetOrganizationNumberValue(PayloadClaimParameters payloadClaimParameters)
    {
        var organizationNumberValue = payloadClaimParameters.ParentOrganizationNumber;
        if (!string.IsNullOrEmpty(payloadClaimParameters.ChildOrganizationNumber))
        {
            organizationNumberValue += $":{payloadClaimParameters.ChildOrganizationNumber}";
        }
        else
        {
            organizationNumberValue += ":";
        }
        return organizationNumberValue;
    }
}