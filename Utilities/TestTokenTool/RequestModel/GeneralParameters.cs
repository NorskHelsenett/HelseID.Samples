namespace TestTokenTool.RequestModel;

public class GeneralParameters
{
    // iss (not applicable for general users)
    public string Issuer { get; set; } = string.Empty;
    // scope
    public IList<string> Scope { get; set; } = new List<string>();
    // client_id
    public string ClientId { get; set; } = string.Empty;
    // helseid://claims/client/claims/orgnr_parent
    public string OrgnrParent { get; set; } = string.Empty;
    // amr
    public IList<string> AuthenticationMethodsReferences { get; set; } = new List<string>();
    // helseid://claims/client/amr
    public string ClientAuthenticationMethodsReferences { get; set; } = string.Empty;
    // helseid://claims/client/client_name
    public string ClientName { get; set; } = string.Empty;
    // jti
    public string Jti { get; set; } = string.Empty;
}