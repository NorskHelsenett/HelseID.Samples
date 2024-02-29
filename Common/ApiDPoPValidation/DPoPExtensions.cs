using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Samples.Common.ApiDPoPValidation;

public static class DPoPExtensions
{
    private const string DPoPAuthorizationSchema = OidcConstants.AuthenticationSchemes.AuthorizationHeaderDPoP + " ";

    public static bool GetDPoPAccessToken(this HttpRequest request, out string? token)
    {
        token = null;
        var authorization = request.Headers.Authorization.SingleOrDefault();
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith(DPoPAuthorizationSchema))
        {
            return false;
        }
        token = authorization.Substring(DPoPAuthorizationSchema.Length);
        return true;
    }

    public static bool GetDPoPProof(this HttpRequest request, out string? dPopProof)
    {
        dPopProof = request.Headers[OidcConstants.HttpHeaders.DPoP].SingleOrDefault();
        return !string.IsNullOrEmpty(dPopProof);
    }
    
    public static string CreateThumbprint(this JsonWebKey jwk)
    {
        return Base64Url.Encode(jwk.ComputeJwkThumbprint());
    }
}