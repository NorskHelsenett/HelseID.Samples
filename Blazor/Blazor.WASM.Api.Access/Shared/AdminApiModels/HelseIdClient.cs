using System.Collections.Generic;

namespace Blazor.WASM.Api.Access.Shared.AdminApiModels
{
    public class HelseIdClient
    {
        public HelseIdClient()
        {
            AllowedCorsOrigins = new List<AllowedCorsOriginModel>();
            AllowedGrantTypes = new List<AllowedGrantTypeModel>();
            AllowedScopes = new List<AllowedScopeModel>();
            Claims = new List<ClientClaimModel>();
            ClientSecrets = new List<ClientSecretModel>();
            IdentityProviderRestrictions = new List<IdentityProviderRestrictionModel>();
            OnBehalfOfs = new List<ClientOnBehalfOfModel>();
            PostLogoutRedirectUris = new List<PostLogoutRedirectUriModel>();
            RedirectUris = new List<RedirectUriModel>();
            OrgNoChildWhitelist = new List<string>();
            ClientId = null!;
            ClientName = null!;
            ClientUri = null!;
            Id = null!;
            LogoUri = null!;
            FrontChannelLogoutUri = null!;
            BackChannelLogoutUri = null!;
            OnBehalfOf = null!;
            ClientClaimsPrefix = null!;
            ProtocolType = "oidc";
            ConfigurationOwnerId = null!;
            ConfigurationOwnerName = null!;
            Created = null!;
            Updated = null!;
            ParentId = null!;
            ParentClient = null!;
            OwnerOrgNo = null!;           
            OwnerOrgNos = new List<OwnerOrgNo>();
            Tags = new List<TagModel>();
        }

        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AccessTokenType { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<AllowedCorsOriginModel> AllowedCorsOrigins { get; set; }
        public List<AllowedGrantTypeModel> AllowedGrantTypes { get; set; }
        public List<AllowedScopeModel> AllowedScopes { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public List<ClientClaimModel> Claims { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public List<ClientSecretModel> ClientSecrets { get; set; }
        public string ClientUri { get; set; }
        public bool Enabled { get; set; }
        public bool EnableLocalLogin { get; set; }
        public string? Id { get; set; }
        public List<IdentityProviderRestrictionModel> IdentityProviderRestrictions { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public bool IncludeJwtId { get; set; }
        public bool IsDcrClient { get; set; }
        public string LogoUri { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public string OnBehalfOf { get; set; }
        public List<ClientOnBehalfOfModel> OnBehalfOfs { get; set; }
        public List<PostLogoutRedirectUriModel> PostLogoutRedirectUris { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public string ProtocolType { get; set; }
        public List<RedirectUriModel> RedirectUris { get; set; }
        public RefreshTokenExpiration RefreshTokenExpiration { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool RequireConsent { get; set; }
        public bool RequirePkce { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public string? ConfigurationOwnerId { get; set; }
        public string ConfigurationOwnerName { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
        public string ParentId { get; set; }
        public int? UserSsoLifetime { get; set; }

        public List<string> OrgNoChildWhitelist { get; set; }
        public string OwnerOrgNo { get; set; }

        public List<OwnerOrgNo> OwnerOrgNos { get; set; }
        public OrganizationSettingsViewModel OrganizationSettings { get; set; } = new OrganizationSettingsViewModel();

        public HelseIdClient ParentClient { get; set; }
        public List<TagModel> Tags { get; set; }
    }

    public class OwnerOrgNo
    {
        public string OrgNo { get; set; }
        public List<Child> Children { get; set; }
    }

    public class Child
    {
        public string OrgNo { get; set; }
    }

    public enum RefreshTokenExpiration
    {
        Sliding = 0,
        Absolute = 1,
    }

    public enum AccessTokenType
    {
        Jwt = 0,
        Reference = 1,
    }
}
