namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class ClientOnBehalfOfModel
    {
        public ClientOnBehalfOfModel()
        {
            OnBehalfOfId = "";
            OrgNo = "";
        }

        public string OnBehalfOfId { get; set; }
        public string OrgNo { get; set; }
        public bool Default { get; set; }
    }
}
