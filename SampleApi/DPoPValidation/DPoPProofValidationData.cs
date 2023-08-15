using Microsoft.IdentityModel.Tokens;

namespace HelseId.SampleAPI.DPoPValidation;

public class DPoPProofValidationData
{
    public DPoPProofValidationData(string url, string httpMethod, string proofToken, string accessToken)
    {
        Url = url;
        HttpMethod = httpMethod;
        ProofToken = proofToken;
        AccessToken = accessToken;
    }
    
    public string Url { get; }

    public string HttpMethod { get; }

    public string ProofToken { get; }

    public string AccessToken { get; }

    public JsonWebKey? JsonWebKey { get; set; }
    
    public IDictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();
    
    public string? TokenId { get; set; }
}