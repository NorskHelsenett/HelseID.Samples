namespace HelseId.Samples.Common.Configuration;

public class SecurityKey
{
    public string JwkValue { get; }
    public string Algorithm { get; }

    public SecurityKey(string jwkValue, string algorithm)
    {
        JwkValue = jwkValue;
        Algorithm = algorithm;
    }
}