namespace HelseId.Core.BFF.Sample.WebCommon.Identity
{
    public static class IdentityClaims
    {
        private const string Prefix = HelseIdUriPrefixes.Claims + "identity/";

        public const string AssuranceLevel = Prefix + "assurance_level";
        public const string Pid = Prefix + "pid";
        public const string PidPseudonym = Prefix + "pid_pseudonym";
        public const string SecurityLevel = Prefix + "security_level";
        public const string Network = Prefix + "network";
        public const string Name = "name";
    }
}