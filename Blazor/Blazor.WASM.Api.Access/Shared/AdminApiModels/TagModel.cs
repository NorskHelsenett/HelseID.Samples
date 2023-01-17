using System;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class TagModel
    {
        public TagModel()
        {
            ClientId = "";
            Tag = "";
            Created = "";
            User = "";
        }

        public string ClientId { get; set; }
        public string Tag { get; set; }
        public string Created { get; set; }
        public string User { get; set; }
    }
}
