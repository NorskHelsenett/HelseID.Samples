using HelseId.Samples.Common.Interfaces.JwtTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.ClientCredentials.SigningCredentials;

public class SigningCredentialsStore : ISigningCredentialsStore
{
    public Microsoft.IdentityModel.Tokens.SigningCredentials GetSigningCredentials()
    {
        var contentRootPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var sharedConfigurationPath = Path.Combine(contentRootPath, "..", "Configuration");
        var generalPrivateRsaKey = File.ReadAllText("PrivateRsaKey.jwk");

        var securityKey = new JsonWebKey(generalPrivateRsaKey);
        return new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
    }
}