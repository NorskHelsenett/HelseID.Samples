using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace HelseId.Samples.Common.JwtTokens;

public static class JwtDecoder
{
    private static readonly JsonSerializerOptions JsonSerializerOptions;

    public static string Decode(string jwt)
    {
        var parts = jwt.Split(".");
        // string header = parts[0];
        var payload = parts[1];

        return DecodeJwtPart(payload);
    }

    private static string DecodeJwtPart(string part)
    {
        part = part.Replace('_', '/').Replace('-', '+');

        switch (part.Length % 4)
        {
            case 2:
                part += "==";
                break;
            case 3:
                part += "=";
                break;
        }

        string json = Encoding.Default.GetString(Convert.FromBase64String(part));

        var jsonObject = JsonSerializer.Deserialize<JsonObject>(json, options: JsonSerializerOptions);
        return JsonSerializer.Serialize(jsonObject, options: JsonSerializerOptions);
    }

    static JwtDecoder()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            IgnoreReadOnlyProperties = true,
        };

        JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
}