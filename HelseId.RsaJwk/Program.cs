using CommandLine;
using HelseId.RsaJwk;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using JsonWebKey = HelseId.RsaJwk.JsonWebKey;


new Parser(with =>
    {
        with.HelpWriter = Console.Out;
        with.AutoHelp = true;
        with.CaseInsensitiveEnumValues = true;
    })
    .ParseArguments<Options>(args)
    .MapResult(
        options => GenerateKey(options),
        errors => 1
    );

static int GenerateKey(Options options)
{
    if (options.RsaKeySize < 2048)
    {
        Console.WriteLine("RSA key size must be at least 2048");
        return 1;
    }

    var keyType = options.KeyType;
    var prefix = options.Prefix;

    var jwkFileName = "jwk.json";
    var publicJwkFileName = "jwk_pub.json";

    if (prefix != null)
    {
        jwkFileName = $"{prefix}_{jwkFileName}";
        publicJwkFileName = $"{prefix}_{publicJwkFileName}";
    }

    JsonWebKey privateJwk, publicJwk;
    switch (keyType)
    {
        case KeyType.Rsa:
            (privateJwk, publicJwk) = GenerateRsaKey(options);
            break;
        case KeyType.Ec:
            (privateJwk, publicJwk) = GenerateEcdsaKey(options);
            break;
        default:
            Console.WriteLine($"Unsupported key type '{keyType}'");
            return 1;
    }

    File.WriteAllText(jwkFileName, JsonSerializer.Serialize(privateJwk, SourceGenerationContext.Default.JsonWebKey));
    Console.WriteLine($"Wrote JWK to {jwkFileName}");
    File.WriteAllText(publicJwkFileName, JsonSerializer.Serialize(publicJwk, SourceGenerationContext.Default.JsonWebKey));
    Console.WriteLine($"Wrote public JWK to {publicJwkFileName}");

    return 0;
}

static (JsonWebKey privateJwk, JsonWebKey publicJwk) GenerateRsaKey(Options options)
{
    var keySize = options.RsaKeySize ?? 4096;
    var key = RSA.Create(keySize);
    var securityKey = new RsaSecurityKey(key);
    securityKey.KeyId = Guid.NewGuid().ToString().Replace("-", string.Empty);

    var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey);
    jwk.Use = "sig";
    jwk.Alg = options.Alg ?? SecurityAlgorithms.RsaSha512;

    var privateJwk = new JsonWebKey
    {
        D = jwk.D,
        DP = jwk.DP,
        DQ = jwk.DQ,
        E = jwk.E,
        Kty = jwk.Kty,
        N = jwk.N,
        P = jwk.P,
        Q = jwk.Q,
        QI = jwk.QI,
        Kid = jwk.Kid,
        Use = jwk.Use,
        Alg = jwk.Alg,
    };
    var publicJwk = new JsonWebKey
    {
        Kty = jwk.Kty,
        N = jwk.N,
        E = jwk.E,
        Kid = jwk.Kid,
        Use = jwk.Use,
        Alg = jwk.Alg,
    };

    return (privateJwk, publicJwk);
}

static (JsonWebKey privateJwk, JsonWebKey publicJwk) GenerateEcdsaKey(Options options)
{
    var key = ECDsa.Create(ECCurve.NamedCurves.nistP521);
    var securityKey = new ECDsaSecurityKey(key)
    {
        KeyId = Guid.NewGuid().ToString().Replace("-", string.Empty)
    };

    var jwk = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(securityKey);
    jwk.Use = "sig";
    jwk.Alg = options.Alg ?? SecurityAlgorithms.EcdsaSha256;

    var privateJwk = new JsonWebKey
    {
        Kty = jwk.Kty,
        D = jwk.D,
        Crv = jwk.Crv,
        X = jwk.X,
        Y = jwk.Y,
        Kid = jwk.Kid,
        Use = jwk.Use,
        Alg = jwk.Alg,
    };
    var publicJwk = new JsonWebKey
    {
        Kty = jwk.Kty,
        Crv = jwk.Crv,
        X = jwk.X,
        Y = jwk.Y,
        Kid = jwk.Kid,
        Use = jwk.Use,
        Alg = jwk.Alg,
    };

    return (privateJwk, publicJwk);
}
