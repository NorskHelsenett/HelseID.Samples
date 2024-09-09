namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class DPoPProofParameters
{
    public string HtuClaimValue { get; set; } = string.Empty;
    
    public string HtmClaimValue { get; set; } = string.Empty;
    
    public string PrivateKeyForProofCreation { get; set; } = string.Empty;

    public InvalidDPoPProofParameters InvalidDPoPProofParameters { get; set; } = InvalidDPoPProofParameters.None;
}
