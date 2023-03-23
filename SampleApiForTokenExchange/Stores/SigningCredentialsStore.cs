using HelseId.Samples.Common.Interfaces.JwtTokens;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.SampleApiForTokenExchange.Stores;

public class SigningCredentialsStore : ISigningCredentialsStore
{
    private string _contentRootPath;

    public SigningCredentialsStore(IConfiguration configuration)
    {
        _contentRootPath = configuration["contentRoot"]!;
    }

    public SigningCredentials GetSigningCredentials()
    {
        var sharedConfigurationPath = Path.Combine(_contentRootPath, "..", "Configuration", "PrivateRsaKey.jwk");
        var generalPrivateRsaKey = File.ReadAllText(sharedConfigurationPath);

        var securityKey = new JsonWebKey(generalPrivateRsaKey);
        return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
    }
}