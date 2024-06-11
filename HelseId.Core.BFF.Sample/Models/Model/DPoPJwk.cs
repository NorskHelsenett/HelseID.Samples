namespace HelseId.Core.BFF.Sample.Models.Model;

public class DPoPJwk
{
    public DPoPJwk(string jwk, string algorithm)
    {
        Jwk = jwk;
        Algorithm = algorithm;
    }

    public string Jwk { get; init; }
    public string Algorithm { get; init; }
}