using System;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class HelseIdConfigurationOwner
    {
        public HelseIdConfigurationOwner()
        {
            Id = null;
            Name = "";
            Description = "";
            Prefix = "";
            IsEnabled = false;
            OrgNr = "";
            Created = "";
            Updated = "";
        }

        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public bool IsEnabled { get; set; }
        public string OrgNr { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }

        public bool IsChangedFrom(HelseIdConfigurationOwner other)
        {
            return
                !this.Prefix.Equals(other.Prefix, StringComparison.OrdinalIgnoreCase) ||
                !this.Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) ||
                this.Description != other.Description ||
                this.IsEnabled != other.IsEnabled;
        }
    }
}
