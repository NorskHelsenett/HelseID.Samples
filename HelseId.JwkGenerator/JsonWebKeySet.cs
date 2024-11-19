using System.Collections.Generic;

namespace HelseId.JwkGenerator;

internal class JsonWebKeySet
{
    public List<JsonWebKey> Keys { get; set; } = [];
}
