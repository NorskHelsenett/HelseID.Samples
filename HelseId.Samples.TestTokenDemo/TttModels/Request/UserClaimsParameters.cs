namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class UserClaimsParameters
{
    // helseid://claims/identity/pid
    public string Pid { get; set; } = string.Empty;
    // helseid://claims/identity/pid_pseudonym
    public string PidPseudonym { get; set; } = string.Empty;
    // helseid://claims/hpr/hpr_number
    public string HprNumber { get; set; } = string.Empty;
    // name
    public string Name { get; set; } = string.Empty;
    // given_name
    public string GivenName { get; set; } = string.Empty;
    // middle_name
    public string MiddleName { get; set; } = string.Empty;
    // family_name
    public string FamilyName { get; set; } = string.Empty;
    // idp
    public string IdentityProvider { get; set; } = string.Empty;
    // helseid://claims/identity/security_level
    public string SecurityLevel { get; set; } = string.Empty;
    // helseid://claims/identity/assurance_level
    public string AssuranceLevel { get; set; } = string.Empty;
    // helseid://claims/identity/network
    public string Network { get; set; } = string.Empty;
    // amr
    public string Amr { get; set; } = string.Empty;
    // sub
    public string Subject { get; set; } = string.Empty;
    // sid
    public string Sid { get; set; } = string.Empty;
}
