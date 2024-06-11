using System.Security.Cryptography;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.ApiDPoPValidation;

public class DPoPProofValidationData
{
    public DPoPProofValidationData(HttpRequest request, string proofToken, string accessToken, string? cnfClaimValueFromAccessToken)
    {
        Url = request.Scheme + "://" + request.Host + request.PathBase + request.Path;
        HttpMethod = request.Method;
        ProofToken = proofToken;
        AccessTokenHash = HashAccessToken(accessToken);
        CnfClaimValueFromAccessToken = cnfClaimValueFromAccessToken;
    }
    
    private static string HashAccessToken(string accessToken)
    {
        using var sha = SHA256.Create();

        var bytes = Encoding.UTF8.GetBytes(accessToken);
        var hash = sha.ComputeHash(bytes);

        return Base64Url.Encode(hash);
    }

    public string Url { get; }

    public string HttpMethod { get; }

    public string ProofToken { get; }

    public string AccessTokenHash { get; }
    
    public string? CnfClaimValueFromAccessToken { get; }

    public JsonWebKey? JsonWebKey { get; set; }
    
    public IDictionary<string, object> Payload { get; set; } = new Dictionary<string, object>();
    
    public string? TokenId { get; set; }
    
    public string? JktClaimValueFromAccessToken { get; set; }
}