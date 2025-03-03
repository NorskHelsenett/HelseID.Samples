using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.JwtTokens;

/// <summary>
/// This class creates a JWT token that can be used in client assertions or in request objects.
/// This class requires an IPayloadClaimsCreator instance that will create the claims that are needed in the token request.
/// </summary>
public class SigningTokenCreator : ISigningTokenCreator
{
    private readonly IJwtClaimsCreator _jwtClaimsCreator;
    private readonly HelseIdConfiguration _configuration;

    public SigningTokenCreator(
        IJwtClaimsCreator jwtClaimsCreator,
        HelseIdConfiguration configuration)
    {
        _jwtClaimsCreator = jwtClaimsCreator;
        _configuration = configuration;
    }

    public string CreateSigningToken(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        var claims = _jwtClaimsCreator.CreateJwtClaims(payloadClaimsCreator, payloadClaimParameters, _configuration);
        var signingCredentials = GetClientAssertionSigningCredentials();

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims,
            SigningCredentials = signingCredentials,
            TokenType = "client-authentication+jwt",
        };

        // This creates a (signed) jwt token which is used for the client assertion.
        var tokenHandler = new JsonWebTokenHandler
        {
            SetDefaultTimesOnTokenCreation = false,
        };

        return tokenHandler.CreateToken(securityTokenDescriptor);
    }

    /// <summary>
    /// This method uses our (secure) private key to sign the token which is used for the client assertion
    /// </summary>
    private SigningCredentials GetClientAssertionSigningCredentials()
    {
        var securityKey = new JsonWebKey(_configuration.PrivateKeyJwk.JwkValue);
        return new SigningCredentials(securityKey, _configuration.PrivateKeyJwk.Algorithm);
    }
}
