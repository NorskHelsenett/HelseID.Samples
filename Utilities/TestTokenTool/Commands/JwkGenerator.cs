using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using TestTokenTool.Constants;

namespace TestTokenTool.Commands;

internal class JwkGenerator
{
    public static Task GenerateKey()
    {
        if (File.Exists(FileConstants.JwkFileName))
        {
            Console.WriteLine($"The file '{FileConstants.JwkFileName}' is already present. Do you want to overwrite it [y/n]?");
            var input = Console.ReadKey();
            Console.WriteLine();
            if (input.Key != ConsoleKey.Y)
            {
                return Task.CompletedTask;
            }
        }

        var jsonWebKeyPair = GenerateRsaKey();

        WriteJwkToFile(FileConstants.JwkFileName, jsonWebKeyPair.PrivateJwk, "private");
        WriteJwkToFile("jwk_pub.json", jsonWebKeyPair.PublicJwk, "public");
        return Task.CompletedTask;
    }

    private static JsonWebKeyPair GenerateRsaKey()
    {
        var keySize = 4096;
        var key = RSA.Create(keySize);
        var securityKey = new RsaSecurityKey(key);
        securityKey.KeyId = Guid.NewGuid().ToString().Replace("-", string.Empty);

        var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(securityKey);
        jwk.Use = "sig";
        jwk.Alg = SecurityAlgorithms.RsaSha512;

        return new JsonWebKeyPair
        {
            PrivateJwk = new JsonWebKey
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
            },
            PublicJwk = new JsonWebKey
            {
                Kty = jwk.Kty,
                N = jwk.N,
                E = jwk.E,
                Kid = jwk.Kid,
                Use = jwk.Use,
                Alg = jwk.Alg,
            }
        };
    }
    
    private static void WriteJwkToFile(string jwkFileName, JsonWebKey jsonWebKey, string keyDescription)
    {
        var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            IgnoreReadOnlyProperties = true,
        };
        File.WriteAllText(jwkFileName, JsonSerializer.Serialize(jsonWebKey, serializerOptions));
        Console.WriteLine($"Wrote {keyDescription} JWK to {jwkFileName}");
    }
    
    private struct JsonWebKeyPair
    {
        public JsonWebKey PrivateJwk { get; set; } 
        public JsonWebKey PublicJwk { get; set; }
    }
    
    private struct JsonWebKey
    {
        public string? D { get; set; }
        public string? DP { get; set; }
        public string? DQ { get; set; }
        public string? E { get; set; }
        public string? Kty { get; set; }
        public string? N { get; set; }
        public string? P { get; set; }
        public string? Q { get; set; }
        public string? QI { get; set; }
        public string? Kid { get; set; }
        public string? Use { get; set; }
        public string? Alg { get; set; }
    }
}