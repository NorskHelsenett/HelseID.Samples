using CommandLine;
using HelseId.JwkGenerator;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using AlgorithmValidator = HelseId.JwkGenerator.AlgorithmValidator;
using JsonWebKey = HelseId.JwkGenerator.JsonWebKey;
using JsonWebKeySet = HelseId.JwkGenerator.JsonWebKeySet;

new Parser(with =>
    {
        with.HelpWriter = Console.Out;
        with.AutoHelp = true;
        with.CaseInsensitiveEnumValues = true;
    })
    .ParseArguments<Options>(args)
    .MapResult(
        GenerateKey,
        errors => 1
    );

static int GenerateKey(Options options)
{
    if (options.RsaKeySize < 2048)
    {
        Logger.Error("RSA key size must be at least 2048");
        return 1;
    }

    var keyType = options.KeyType;

    JsonWebKey privateJwk, publicJwk;

    try
    {
        switch (keyType)
        {
            case KeyType.Rsa:
                (privateJwk, publicJwk) = GenerateRsaKey(options);
                break;

            case KeyType.Ec:
                (privateJwk, publicJwk) = GenerateEcdsaKey(options);
                break;

            default:
                Logger.Error($"Unsupported key type '{keyType}'");
                return 1;
        }
    }
    catch (Exception ex)
    {
        Logger.Error(ex.Message);
        return 1;
    }

    if (!string.IsNullOrWhiteSpace(options.Alg)
        && !AlgorithmValidator.IsValidAlgorithm(options.Alg, options.KeyType))
    {
        Logger.Warning($"Algorithm '{options.Alg}' is not approved by HelseID for key type '{options.KeyType.ToString().ToUpperInvariant()}'");
    }

    WriteKeyPair(privateJwk, publicJwk, options);

    return 0;
}

static (JsonWebKey privateJwk, JsonWebKey publicJwk) GenerateRsaKey(Options options)
{
    var key = RSA.Create(options.RsaKeySize);
    var securityKey = new RsaSecurityKey(key)
    {
        KeyId = Guid.NewGuid().ToString().Replace("-", string.Empty)
    };

    var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey);
    jwk.Use = "sig";
    jwk.Alg = options.Alg ?? SecurityAlgorithms.RsaSsaPssSha512;

    jwk.Kid = CreateKid(jwk);

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
    ECCurve GetCurveFromName(string curveName)
    {
        return curveName switch
        {
            "P-256" => ECCurve.NamedCurves.nistP256,
            "P-384" => ECCurve.NamedCurves.nistP384,
            "P-521" => ECCurve.NamedCurves.nistP521,
            _ => throw new Exception($"Unsupported curve '{curveName}'"),
        };
    }

    var curve = !string.IsNullOrEmpty(options.EcCurve)
        ? GetCurveFromName(options.EcCurve)
        : ECCurve.NamedCurves.nistP521;

    var key = ECDsa.Create(curve);
    var securityKey = new ECDsaSecurityKey(key);

    var jwk = JsonWebKeyConverter.ConvertFromECDsaSecurityKey(securityKey);
    jwk.Use = "sig";
    jwk.Alg = options.Alg ?? SecurityAlgorithms.EcdsaSha512;
    jwk.Kid = CreateKid(jwk);

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

static void WriteKeyPair(JsonWebKey privateJwk, JsonWebKey publicJwk, Options options)
{
    var privateJwkFileName = "jwk.json";
    var publicJwkFileName = "jwk_pub.json";
    var jwksFileName = "jwks.json";

    var prefix = options.Prefix;

    if (!string.IsNullOrWhiteSpace(prefix))
    {
        privateJwkFileName = $"{prefix}_{privateJwkFileName}";
        publicJwkFileName = $"{prefix}_{publicJwkFileName}";
        jwksFileName = $"{prefix}_{jwksFileName}";
    }

    Logger.Info($"Key type is {(options.KeyType == KeyType.Rsa ? $"RSA with a key length of {options.RsaKeySize} bits" : $"ECDSA {nameof(ECCurve.NamedCurves.nistP521)}")}");

    File.WriteAllText(privateJwkFileName, JsonSerializer.Serialize(privateJwk, SourceGenerationContext.Default.JsonWebKey));
    Logger.Success($"Wrote private JWK to {privateJwkFileName}");

    File.WriteAllText(publicJwkFileName, JsonSerializer.Serialize(publicJwk, SourceGenerationContext.Default.JsonWebKey));
    Logger.Success($"Wrote public JWK to {publicJwkFileName}");

    if (options.Jwks)
    {
        var jwks = new JsonWebKeySet
        {
            Keys = [publicJwk]
        };

        File.WriteAllText(jwksFileName, JsonSerializer.Serialize(jwks, SourceGenerationContext.Default.JsonWebKeySet));
        Logger.Success($"Wrote JWKS to {jwksFileName}");
    }
}

static string CreateKid(Microsoft.IdentityModel.Tokens.JsonWebKey jwk)
{
    return Base64UrlEncoder.Encode(jwk.ComputeJwkThumbprint());
}