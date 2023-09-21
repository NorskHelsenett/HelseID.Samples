using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using IdentityModel;
using IdentityModel.Client;

namespace HelseId.Samples.Common.ClientAssertions;

/// <summary>
/// This class builds a client assertion. This assertion includes a JWT token that establishes the identity of this
/// client application for the STS. See https://www.rfc-editor.org/rfc/rfc7523#section-2.2 for more information
/// about this mechanism.
/// </summary>
public class ClientAssertionsBuilder : IClientAssertionsBuilder
{
    private readonly ISigningTokenCreator _signingTokenCreator;
    
    public ClientAssertionsBuilder(ISigningTokenCreator signingTokenCreator)
    {
        _signingTokenCreator = signingTokenCreator;
    }

    public ClientAssertion BuildClientAssertion(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        var token = _signingTokenCreator.CreateSigningToken(payloadClaimsCreator, payloadClaimParameters);
        
        // Uncomment if you'd like to inspect the token that is sent to HelseID as part of the client assertion
        // Console.WriteLine("This is the security token that is sent to HelseID as part of the client assertion:");
        // Console.WriteLine(token);
        // Console.WriteLine("-----");
        
        return new ClientAssertion
        {
            Value = token,
            Type = OidcConstants.ClientAssertionTypes.JwtBearer
        };
    }
}

