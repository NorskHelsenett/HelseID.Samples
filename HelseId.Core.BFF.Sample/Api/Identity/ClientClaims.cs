using HelseId.Core.BFF.Sample.WebCommon.Identity;

namespace HelseId.Core.BFF.Sample.Api.Identity
{
    public static class ClientClaims
    {
        private const string Prefix = HelseIdUriPrefixes.Claims + "client/";

        public const string OnBehalfOf = Prefix + "on_behalf_of";

        public const string ParentOrganizationNumberEC = Prefix + "ec/orgnr_parent";

        public const string ChildOrganizationNumberEC = Prefix + "ec/orgnr_child";

        public const string CertificateExpiryDate = Prefix + "ec/exp";
        public const string CertificateCommonName = Prefix + "ec/common_name";

        public const string Amr = Prefix + "amr";
    }
}