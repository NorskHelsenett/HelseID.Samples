// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace TestTokenTool.RequestModel;

public class TokenRequest
{
    public string Audience { get; set; } = String.Empty;
    
    public ParametersGeneration GeneralClaimsParametersGeneration { get; set; }
    
    public ParametersGeneration UserClaimsParametersGeneration { get; set; }
    
    public bool WithoutDefaultGeneralClaims { get; set; }
    
    public bool WithoutDefaultUserClaims { get; set; }

    public IssuerEnvironment IssuerEnvironment { get; set; }

    public bool CreateDPoPTokenWithDPoPProof { get; set; }
    
    public bool CreateTillitsrammeverkClaims { get; set; }

    public bool SignJwtWithInvalidSigningKey { get; set; } 

    public bool SetInvalidIssuer { get; set; }
    
    public bool SetInvalidAudience { get; set; }
    
    public bool GetPersonFromPersontjenesten { get; set; }

    public bool OnlySetNameForPerson { get; set; }
    
    public bool GetHprNumberFromHprregisteret { get; set; }

    public bool SetPidPseudonym { get; set; }
    
    public bool SetSubject { get; set; }

    public ExpirationParameters ExpirationParameters { get; set; } = new();

    public HeaderParameters HeaderParameters { get; set; } = new();
    
    public GeneralParameters GeneralClaimsParameters { get; set; } = new();
    
    public UserClaimsParameters UserClaimsParameters { get; set; } = new ();

    public TillitsrammeverkClaimsParameters TillitsrammeverkClaimsParameters { get; set; } = new();
    
    public DPoPProofParameters DPoPProofParameters { get; set; } = new();

    public ApiSpecificClaim[]? ApiSpecificClaims { get; set; } = [];
}
