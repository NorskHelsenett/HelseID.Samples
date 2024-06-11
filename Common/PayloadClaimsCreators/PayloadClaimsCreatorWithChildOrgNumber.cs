using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.Common.PayloadClaimsCreators;

/// <summary>
/// This class creates an "authorization_details" claim that has an internal
/// structure which HelseID can recognize.
///
/// The purpose of this is to request a claim in the access token that
/// contains a specified child organization (underenhet) for our call to
/// the Sample API.
///
/// The child organization requested must be present in a
/// list connected to the client in HelseID.
/// </summary>
public class PayloadClaimsCreatorWithChildOrgNumber : IPayloadClaimsCreator {

    public IEnumerable<PayloadClaim> CreatePayloadClaims(
        PayloadClaimParameters payloadClaimParameters,
        HelseIdConfiguration configuration)
    {
        if (string.IsNullOrEmpty(payloadClaimParameters.ChildOrganizationNumber))
        {
            throw new Exception("Need payload claim parameters with child organization number");
        }

        // When the client requires a child organization claim, HelseID will
        // require an authorization details claim with the following structure:
        //
        //  "authorization_details":{
        //      "type":"helseid_authorization",
        //      "practitioner_role":{
        //          "organization":{
        //              "identifier": {
        //                  "system":"urn:oid:2.16.578.1.12.4.1.2.101",
        //                  "type":"ENH",
        //                  "value":"[orgnummer]"
        //              }
        //          }
        //      }
        //  }

        var orgNumberDetails = new Dictionary<string, string>
        {
            { "system", "urn:oid:2.16.578.1.12.4.1.2.101" },
            { "type", "ENH" },
            { "value", $"{payloadClaimParameters.ChildOrganizationNumber}" }
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
}
