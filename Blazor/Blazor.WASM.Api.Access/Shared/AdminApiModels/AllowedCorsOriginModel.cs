namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class AllowedCorsOriginModel
    {
        public AllowedCorsOriginModel()
        {
            Origin = "";
        }

        public string Origin { get; set; }
    }
}
