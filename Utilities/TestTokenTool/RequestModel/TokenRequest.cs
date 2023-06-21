namespace TestTokenTool.RequestModel;

public class TokenRequest
{
    public ParametersGeneration GeneralClaimsParametersGeneration { get; set; }
    
    public ParametersGeneration UserClaimsParametersGeneration { get; set; }

    public bool CreateDokumentdelingClaims { get; set; }

    public bool SignJwtWithInvalidSigningKey { get; set; } 

    public bool SetExpirationTimeAsExpired { get; set; }

    public bool SetInvalidIssuer { get; set; }
    
    public bool SetInvalidAudience { get; set; }
    
    public bool GetPersonFromPersontjenesten { get; set; }
    
    public bool GetHprNumberFromHprregisteret { get; set; }

    public bool SetPidPseudonym { get; set; }

    public int ExpirationTimeInSeconds { get; set; } = Int32.MinValue;

    public HeaderParameters HeaderParameters { get; set; } = new();
    
    public GeneralParameters GeneralClaimsParameters { get; set; } = new();
    
    public UserClaimsParameters UserClaimsParameters { get; set; } = new ();

    public TillitsrammeverkClaimsParameters TillitsrammeverkClaimsParameters { get; set; } = new();

    public DokumentdelingClaimsParameters DokumentdelingClaimsParameters { get; set; } = new();
}