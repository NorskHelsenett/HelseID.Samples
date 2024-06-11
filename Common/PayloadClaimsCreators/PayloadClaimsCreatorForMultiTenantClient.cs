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
        if (string.IsNullOrEmpty(payloadClaimParameters.ParentOrganizationNumber))
        {
            return new List<PayloadClaim>();
        }

        // When the client is of the multi-tenancy type, it will require a parent organization number claim.
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

        var orgNumberDetails = new Dictionary<string, string>
        {
            { "system", "urn:oid:1.0.6523" },
            { "type", "ENH" },
            { "value", $"NO:ORGNR:{GetOrganizationNumberValue(payloadClaimParameters)}" }
        };

        var identifier = new Dictionary<string, object>
        {
            { "identifier", orgNumberDetails }
        };

        var organization = new Dictionary<string, object>
        {
            { "organization", identifier }
        };

        var authorizationDetails = new Dictionary<string, object>
        {
            { "type", "helseid_authorization" },
            { "practitioner_role", organization }
        };

        return new List<PayloadClaim> {
            new("authorization_details", authorizationDetails),
        };
    }

    private static string GetOrganizationNumberValue(PayloadClaimParameters payloadClaimParameters)
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
