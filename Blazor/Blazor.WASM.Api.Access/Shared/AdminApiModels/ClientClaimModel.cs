using System.Security.Claims;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class ClientClaimModel
    {
        public ClientClaimModel()
        {
            Type = "";
            Value = "";
        }

        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; } = ClaimValueTypes.String;
    }
}
