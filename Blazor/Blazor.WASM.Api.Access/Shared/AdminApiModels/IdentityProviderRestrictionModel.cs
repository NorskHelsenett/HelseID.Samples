namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class IdentityProviderRestrictionModel
    {
        public IdentityProviderRestrictionModel()
        {
            Provider = "";
        }

        public string Provider { get; set; }
    }
}
