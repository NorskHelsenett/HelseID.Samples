namespace TestTokenTool.RequestModel;

public class TokenRequest
{
    public ParametersGeneration GeneralParametersGeneration { get; set; }
    
    public ParametersGeneration UserClaimsParametersGeneration { get; set; }

    public ParametersGeneration DokumentdelingClaimsParametersGeneration { get; set; }

    public bool SignJwtWithInvalidSigningKey { get; set; } 

    public bool SetExpirationTimeAsExpired { get; set; }

    public bool SetInvalidIssuer { get; set; }
    
    public bool SetInvalidAudience { get; set; }
    
    public bool GetPersonFromPersontjenesten { get; set; }
    
    public bool GetHprNumberFromHprregisteret { get; set; }

    public bool SetPidPseudonym { get; set; }

    public int ExpirationTimeInSeconds { get; set; } = Int32.MinValue;

    public GeneralParameters GeneralParameters { get; set; } = new();
    
    public UserClaimsParameters UserClaimsParameters { get; set; } = new ();

    public TillitsrammeverkClaimsParameters TillitsrammeverkClaimsParameters { get; set; } = new();
}