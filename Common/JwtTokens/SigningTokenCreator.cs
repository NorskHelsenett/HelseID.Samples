using System.IdentityModel.Tokens.Jwt;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.JwtTokens;

/// <summary>
/// This class creates a JWT token that can be used in client assertions or in request objects.
/// This class requires an IPayloadClaimsCreator instance that will create the claims that are needed in the token request.
/// </summary>
public class SigningTokenCreator : ISigningTokenCreator
{
    private readonly IJwtPayloadCreator _jwtPayloadCreator;
    private readonly HelseIdConfiguration _configuration;
    
    public SigningTokenCreator(
        IJwtPayloadCreator jwtPayloadCreator,
        HelseIdConfiguration configuration)
    {
        _jwtPayloadCreator = jwtPayloadCreator;
        _configuration = configuration;
    }
    
    public string CreateSigningToken(IPayloadClaimsCreator payloadClaimsCreator, PayloadClaimParameters payloadClaimParameters)
    {
        var header = CreateJwtHeaderWithSigningCredentials();
        var payload = _jwtPayloadCreator.CreateJwtPayload(payloadClaimsCreator, payloadClaimParameters, _configuration);

        // The JwtSecurityToken class is part of the System.IdentityModel library.
        var jwtSecurityToken = new JwtSecurityToken(header, payload);

        // This creates a (signed) jwt token which is used for the client assertion.
        // The JwtSecurityTokenHandler class is part of the System.IdentityModel library.
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    /// <summary>
    /// This method uses our (secure) private key to sign the token which is used for the client assertion 
    /// </summary>
    private JwtHeader CreateJwtHeaderWithSigningCredentials()
    {
        var signingCredentials = GetClientAssertionSigningCredentials();
        // The JwtHeader class is part of the System.IdentityModel library.
        return new JwtHeader(signingCredentials);
    }

    private SigningCredentials GetClientAssertionSigningCredentials()
    {
        var securityKey = new JsonWebKey(_configuration.PrivateKeyJwk.JwkValue);
        return new SigningCredentials(securityKey, _configuration.PrivateKeyJwk.Algorithm);
    }
}
