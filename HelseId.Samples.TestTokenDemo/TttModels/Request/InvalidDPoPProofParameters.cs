namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public enum InvalidDPoPProofParameters
{
    None = 0,
    DontSetHtuClaimValue = 1,
    DontSetHtmClaimValue = 2,
    SetIatValueInThePast = 3,
    SetIatValueInTheFuture = 4,
    DontSetAthClaimValue = 5,
    DontSetAlgHeader = 6,
    DontSetJwkHeader = 7,
    DontSetJtiClaim = 8,
    SetAlgHeaderToASymmetricAlgorithm = 9,
    SetPrivateKeyInJwkHeader = 10,
    SetInvalidTypHeaderValue = 11,
    SetAnInvalidSignature = 12,
}
