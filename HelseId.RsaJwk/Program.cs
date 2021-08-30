using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelseId.RsaJwk
{
    static class Program
    {
        static void Main(string[] args)
        {
            const int keySize = 4096;

            string prefix = null;
            if (args.Length >= 1)
            {
                prefix = args[0];
            }

            var rsa = RSA.Create(keySize);
            var rsaKey = new RsaSecurityKey(rsa);

            var rsaXmlPrivate = rsa.ToXmlString(true);
            var rsaXmlPublic = rsa.ToXmlString(false);

            File.WriteAllText("rsa_xml_private.xml", rsaXmlPrivate);
            File.WriteAllText("rsa_xml_public.xml", rsaXmlPublic);



            var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaKey);
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
            };
            var publicJwk = new JsonWebKey
            {
                Kty = jwk.Kty,
                N = jwk.N,
                E = jwk.E,
            };

            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                IgnoreReadOnlyProperties = true,
            };

            var jwkFileName = "jwk.json";
            var publicJwkFileName = "jwk_pub.json";

            if (prefix != null)
            {
                jwkFileName = $"{prefix}_{jwkFileName}";
                publicJwkFileName = $"{prefix}_{publicJwkFileName}";
            }

            File.WriteAllText(jwkFileName, JsonSerializer.Serialize(privateJwk, serializerOptions));
            Console.WriteLine($"Wrote JWK to {jwkFileName}");
            File.WriteAllText(publicJwkFileName, JsonSerializer.Serialize(publicJwk, serializerOptions));
            Console.WriteLine($"Wrote public JWK to {publicJwkFileName}");
        }
    }
}
