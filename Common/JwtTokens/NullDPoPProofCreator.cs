using HelseId.Samples.Common.Interfaces.JwtTokens;

namespace HelseId.Samples.Common.JwtTokens;

public class NullDPoPProofCreator : IDPoPProofCreator
{
    public string CreateDPoPProof(string url, string httpMethod, string? dPoPNonce = null, string? accessToken = null)
    {
        return "";
    }
}
