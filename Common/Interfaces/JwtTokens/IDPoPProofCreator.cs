namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface IDPoPProofCreator
{
    string CreateDPoPProof(string url, string httpMethod, string? dPoPNonce = null, string? accessToken = null);
}