﻿using System.Text.Json.Serialization;

namespace HelseId.RsaJwk;

internal class JsonWebKey
{
    public string D { get; set; }
    public string DP { get; set; }
    public string DQ { get; set; }
    public string E { get; set; }
    public string Kty { get; set; }
    public string N { get; set; }
    public string P { get; set; }
    public string Q { get; set; }
    public string QI { get; set; }
    public string Crv { get; set; }
    public string X { get; set; }
    public string Y { get; set; }
    public string Kid { get; set; }
    public string Use { get; set; }
    public string Alg { get; set; }
}

// Source generation is only required because PublishTrimmed is enabled on project
[JsonSerializable(typeof(JsonWebKey))]
[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    IgnoreReadOnlyProperties = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
internal partial class SourceGenerationContext : JsonSerializerContext {}