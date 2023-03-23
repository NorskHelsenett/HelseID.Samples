using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface ISigningCredentialsStore
{
    SigningCredentials GetSigningCredentials();
}