using System.Text.Json;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.SampleAPI.DPoPValidation;

static class DPoPExtensions
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
    
    /// <summary>
    /// Create the value of a thumbprint-based cnf claim
    /// </summary>
    public static string CreateThumbprintCnf(this JsonWebKey jwk)
    {
        var jkt = jwk.CreateThumbprint();
        var values = new Dictionary<string, string>
        {
            { JwtClaimTypes.ConfirmationMethods.JwkThumbprint, jkt }
        };
        return JsonSerializer.Serialize(values);
    }

    /// <summary>
    /// Create the value of a thumbprint
    /// </summary>
    public static string CreateThumbprint(this JsonWebKey jwk)
    {
        var jkt = Base64Url.Encode(jwk.ComputeJwkThumbprint());
        return jkt;
    }
/*
    public static bool IsDPoPAuthorizationScheme(this HttpRequest request)
    {
        var authz = request.Headers.Authorization.FirstOrDefault();
        return authz?.StartsWith(DPoPAuthorizationSchema, System.StringComparison.Ordinal) == true;
    }

    public static string GetAuthorizationScheme(this HttpRequest request)
    {
        return request.Headers.Authorization.FirstOrDefault()?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0];
    }


    public static string GetDPoPNonce(this AuthenticationProperties props)
    {
        if (props.Items.ContainsKey("DPoP-Nonce"))
        {
            return props.Items["DPoP-Nonce"] as string;
        }
        return null;
    }
    public static void SetDPoPNonce(this AuthenticationProperties props, string nonce)
    {
        props.Items["DPoP-Nonce"] = nonce;
    }

    /// <summary>
    /// Create the value of a thumbprint-based cnf claim
    /// </summary>
    public static string CreateThumbprintCnf(this JsonWebKey jwk)
    {
        var jkt = jwk.CreateThumbprint();
        var values = new Dictionary<string, string>
        {
            { JwtClaimTypes.ConfirmationMethods.JwkThumbprint, jkt }
        };
        return JsonSerializer.Serialize(values);
    }

    /// <summary>
    /// Create the value of a thumbprint
    /// </summary>
    public static string CreateThumbprint(this JsonWebKey jwk)
    {
        var jkt = Base64Url.Encode(jwk.ComputeJwkThumbprint());
        return jkt;
    }
    */
}

