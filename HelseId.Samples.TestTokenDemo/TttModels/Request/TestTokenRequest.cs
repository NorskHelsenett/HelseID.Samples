namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class TestTokenRequest
{
    public string Audience { get; set; } = string.Empty;

    public ParametersGeneration ClientClaimsParametersGeneration { set; get; }
    public ParametersGeneration UserClaimsParametersGeneration { get; set; }

    public bool WithoutDefaultClientClaims => ClientClaimsParameters != null;
    public bool WithoutDefaultUserClaims => UserClaimsParameters != null;

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

    public ClientClaimsParameters? ClientClaimsParameters { set; get; }
    public UserClaimsParameters? UserClaimsParameters { get; set; } = new();

    public TillitsrammeverkClaimsParameters TillitsrammeverkClaimsParameters { get; set; } = new();

    public DPoPProofParameters DPoPProofParameters { get; set; } = new();

    public ApiSpecificClaim[] ApiSpecificClaims { get; set; } = [];
}