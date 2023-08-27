namespace TestTokenTool.RequestModel;

public class DPoPProofParameters
{
    public string HtuClaimValue { get; set; } = string.Empty;
    
    public bool DontSetHtuClaimValue { get; set; }

    public string HtmClaimValue { get; set; } = string.Empty;

    public bool DontSetHtmClaimValue { get; set; }

    public bool SetIatValueInThePast { get; set; }

    public bool SetIatValueInTheFuture { get; set; }
    
    public bool DontSetAthClaimValue { get; set; }
    
    public string PrivateKeyForProofCreation { get; set; } = string.Empty;

    public bool SetInvalidDPoPProofJwt { get; set; } 
    
    public bool DontSetAlgHeader { get; set; }

    public bool DontSetJwkHeader { get; set; }

    public bool DontSetJtiClaim { get; set; }

    public bool SetAlgHeaderToNone { get; set; }

    public bool SetAlgHeaderToAnSymmetricAlgorithm { get; set; }

    public bool SetJwkHeaderWithPrivateKey { get; set; }

    // the typ JOSE header parameter has the value dpop+jwt,
    public bool SetInvalidTypHeaderValue { get; set; }
    
    public bool SetAnInvalidSignature { get; set; }
}