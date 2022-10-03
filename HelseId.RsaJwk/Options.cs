
using CommandLine;

namespace HelseId.RsaJwk;

public class Options
{
    [Option('t', "key-type", Required = true, HelpText = "Supported types: RSA, EC")]
    public KeyType KeyType { get; set; }

    [Option('p', "prefix", HelpText = "Optional prefix for the generated JWK files.")]
    public string Prefix { get; set; }

    [Option('a', "alg", HelpText = "Algorithm intended for use with the key. Defaults to RS512 for RSA, and ES256 for ECDSA")]
    public string Alg { get; set; }
}

public enum KeyType {
    Rsa,
    Ec,
}