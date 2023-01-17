namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class OrganizationSettingsViewModel
    {
        /// <summary>
        /// <para>
        /// The configuration may contain multiple child organizations, but only one parent organization
        /// The client may post one of these when authorizing
        /// If true, the client may post any child organization numbers but not the parent.
        /// </para>
        /// <para>See the class "LegacyOrgNumberClaimProcessor" for validation rules</para>
        /// </summary>
        public bool AllowChildOrgNoFromClient { get; set; }

        /// <summary>
        /// <para>
        /// The configuration may contain multiple parent organizations
        /// The client may post one of these when authorizing, and optionally include a child organization.
        /// Configured child organization numbers will be ignored when this flag is true.
        /// </para>
        /// <para>See the class "IsoOrgNumberClaimProcessor" for validation rules</para>
        /// </summary>
        public bool AllowParentOrgnoAndChildOrgnoFromClient { get; set; }
    }
}
