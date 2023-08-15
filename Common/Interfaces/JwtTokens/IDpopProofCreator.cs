namespace HelseId.Samples.Common.Interfaces.JwtTokens;

public interface IDpopProofCreator
{
    string CreateDpopProof(string? dPoPNonce, string url, string httpMethod);
}