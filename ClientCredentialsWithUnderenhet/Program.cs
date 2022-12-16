using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using HelseId.Samples.ClientCredentials;
using HelseID.Samples.Configuration;
using IdentityModel.Client;
using HelseID.Samples.Extensions;

namespace HelseId.Samples.ClientCredentialsWithUnderenhet;

/// <summary>
/// This sample expands upon the client credentials grant example in order to demonstrate how to get
/// an access token for a client that will represent a underenhet (child organization).  
/// </summary>
static class Program
{
    static async Task Main()
    {
        var configuration = HelseIdSamplesConfiguration.ConfigurationForClientCredentialsWithUnderenhetClient;
        
        // Two builder classes are used (see the ClientCredentials directory for details):
        //   * ClientCredentialsClientAssertionsBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        //   * ClientCredentialsClientAssertionsBuilderForUnderenhet, which creates a client assertion
        //     that will be used inside the token request to HelseID in order to authenticate this client.
        //     This particular instance also expands the signing token with an underenhet claim (see the class below)
        var clientCredentialsTokenRequestBuilder =
            new ClientCredentialsTokenRequestBuilder(
                new ClientCredentialsClientAssertionsBuilderForUnderenhet(
                    HelseIdSamplesConfiguration.ClientCredentialsWithUnderenhetOrgNo, configuration),
                configuration);

        using var client = new HttpClient();

        var request = await clientCredentialsTokenRequestBuilder.CreateClientCredentialsTokenRequest(client);

        var result = await client.RequestClientCredentialsTokenAsync(request);

        if (result.IsError)
        {
            await result.WriteErrorToConsole();
        }
        else
        {
            result.WriteAccessTokenFromTokenResult();
        }
    }
}

/// <summary>
/// This class inherits ClientCredentialsClientAssertionsBuilder and expands the signing token with
/// a structured claim that tells HelseID what underenhet (child organization) the client wants
/// to represent.
/// </summary>
public class ClientCredentialsClientAssertionsBuilderForUnderenhet : ClientCredentialsClientAssertionsBuilder
{
    private readonly string _underenhetOrgNo;

    public ClientCredentialsClientAssertionsBuilderForUnderenhet(
        string underenhetOrgNo,
        HelseIdSamplesConfiguration configuration) : base(configuration)
    {
        _underenhetOrgNo = underenhetOrgNo;
    }
    
    /// <summary>
    /// This method overrides the ExpandSigningToken method and creates an extra claim for the signing token.
    /// This claim informs HelseID of the organization code for the underenhet (child organization)
    /// that this client wants to get an access token for. If the organization code is present as a
    /// property of the client ID that this client puts in its request, a token with a claim
    /// of type "helseid://claims/client/claims/orgnr_child" and the corresponding organization
    /// code for the requested underenhet will be issued. 
    /// </summary>
    protected override JwtSecurityToken ExpandSigningToken(JwtSecurityToken jwtSecurityToken)
    {
        // When the client requires an underenhet (child organization) claim, HelseID will 
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
        // 
        // We can insert the structured claim into the payload in the following manner, using anonymous types:

        var authorizationDetails = new
        {
            type = "helseid_authorization",
            practitioner_role = new
            {
                organization = new
                {
                    identifier = new
                    {
                        system = "urn:oid:2.16.578.1.12.4.1.2.101",
                        type = "ENH",
                        value = $"{_underenhetOrgNo}"
                    }
                }
            }
        };

        jwtSecurityToken.Payload["authorization_details"] = authorizationDetails;

        return jwtSecurityToken;
    }
}


