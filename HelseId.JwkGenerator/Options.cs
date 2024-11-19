using CommandLine;

namespace HelseId.JwkGenerator;

public class Options
{
    [Option('t', "key-type", HelpText = "Supported types: RSA, EC", Default = KeyType.Ec)]
    public KeyType KeyType { get; set; }

    [Option('p', "prefix", HelpText = "Optional prefix for the generated JWK files. Example: {prefix}_jwk.json")]
    public string? Prefix { get; set; }

    [Option('a', "alg", HelpText = "Algorithm intended for use with the key. Defaults to PS512 for RSA, and ES512 for ECDSA")]
    public string? Alg { get; set; }

    [Option('s', "rsa-size", HelpText = "Key size in bits for RSA key.", Default = 4096)]
    public int RsaKeySize { get; set; }

    [Option('c', "curve", HelpText = "Curve used for EC key.", Default = "P-521")]
    public required string EcCurve { get; set; }

    [Option("jwks", HelpText = "Also write public key as a Json Web Key Set.", Default = false)]
    public bool Jwks { get; set; }
}

public enum KeyType
{
    Rsa,
    Ec,
}