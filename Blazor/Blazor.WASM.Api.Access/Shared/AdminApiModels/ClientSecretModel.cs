using System;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class ClientSecretModel
    {
        public ClientSecretModel()
        {
            Description = "";
            Expiration = "";
            Type = "";
            Value = "";
        }

        public string Description { get; set; }
        public string? Expiration { get; set; }
        public string Type { get; set; }
        public string? Value { get; set; }
    }
}
