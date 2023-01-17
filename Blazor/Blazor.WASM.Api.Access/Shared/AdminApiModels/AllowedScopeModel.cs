using System;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class AllowedScopeModel
    {
        public AllowedScopeModel()
        {
            Scope = "";
        }

        public string Scope { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is AllowedScopeModel model && Scope == model.Scope;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Scope);
        }
    }
}
