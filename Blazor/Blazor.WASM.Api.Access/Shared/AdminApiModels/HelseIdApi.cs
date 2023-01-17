using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class HelseIdApi
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool Enabled { get; set; }
        public string ConfigurationOwnerId { get; set; } = string.Empty;
        public string ConfigurationOwnerName { get; set; } = string.Empty;
        public List<HelseIdClaimType> UserClaims { get; set; } = new List<HelseIdClaimType>();
        public List<HelseIdClaimType> ClientClaims { get; set; } = new List<HelseIdClaimType>();

        public List<HelseIdApiSecret> Secrets { get; set; } = new List<HelseIdApiSecret>();
        public List<HelseIdApiScope> Scopes { get; set; } = new List<HelseIdApiScope>();

        public string Created { get; set; } = string.Empty;
        public string Updated { get; set; } = string.Empty;
    }

    public class HelseIdApiSecret
    {
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Expiration { get; set; }
    }

    public class HelseIdApiScope
    {
        public string Name { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool ShowInDiscoveryDocument { get; set; }
        public List<HelseIdClaimType> UserClaims { get; set; } = new List<HelseIdClaimType>();
        public List<HelseIdClaimType> ClientClaims { get; set; } = new List<HelseIdClaimType>();
    }

    public class HelseIdClaimType
    {
        public string Type { get; set; } = string.Empty;
    }
}
