using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HelseId.JwkGenerator;

internal class JsonWebKeySet
{
    public List<JsonWebKey> Keys { get; set; }
}
