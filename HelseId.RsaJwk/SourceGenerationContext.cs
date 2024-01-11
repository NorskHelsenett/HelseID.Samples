namespace HelseId.RsaJwk;

using System.Text.Json.Serialization;

// Source generation is only required because PublishTrimmed is enabled on project
[JsonSerializable(typeof(JsonWebKey))]
[JsonSerializable(typeof(JsonWebKeySet))]
[JsonSourceGenerationOptions(
    WriteIndented = false,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    IgnoreReadOnlyProperties = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
internal partial class SourceGenerationContext : JsonSerializerContext { }
