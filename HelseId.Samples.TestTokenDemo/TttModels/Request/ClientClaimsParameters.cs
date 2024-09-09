namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class ClientClaimsParameters
{
    // scope
    public IList<string> Scope { get; set; } = new List<string>();
    // client_id
    public string ClientId { get; set; } = string.Empty;
    // nhn:sfm:journal_id
    public string SfmJournalId { get; set; } = string.Empty;
    // helseid://claims/client/claims/orgnr_parent
    public string OrgnrParent { get; set; } = string.Empty;
    // helseid://claims/client/claims/orgnr_child
    public string OrgnrChild { get; set; } = string.Empty;
    // helseid://claims/client/claims/orgnr_supplier
    public string OrgnrSupplier { get; set; } = string.Empty;
    // helseid://claims/client/claims/client_tenancy
    public bool ClientTenancy { get; set; }
    // amr
    public IList<string> AuthenticationMethodsReferences { get; set; } = new List<string>();
    // helseid://claims/client/amr
    public string ClientAuthenticationMethodsReferences { get; set; } = string.Empty;
    // helseid://claims/client/client_name
    public string ClientName { get; set; } = string.Empty;
    // jti
    public string Jti { get; set; } = string.Empty;
    // Cnf (hash)
    public string CnfJkt { get; set; } = string.Empty;
    // Cnf (public key)
    public string CnfPublicKey { get; set; } = string.Empty;
}
