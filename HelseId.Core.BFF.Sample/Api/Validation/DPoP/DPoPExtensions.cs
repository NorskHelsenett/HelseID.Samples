using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace HelseId.Core.BFF.Sample.Api.Validation.DPoP;

public static class DPoPExtensions
{
    private const string DPoPAuthorizationScheme = OidcConstants.AuthenticationSchemes.AuthorizationHeaderDPoP + " ";

    public static bool GetDPoPAccessToken(this HttpRequest request, [NotNullWhen(true)] out string? token)
    {
        token = null;
        var authorization = request.Headers.Authorization.SingleOrDefault();

        if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith(DPoPAuthorizationScheme))
        {
            return false;
        }

        token = authorization.Substring(DPoPAuthorizationScheme.Length);

        return true;
    }

    public static bool GetDPoPProof(this HttpRequest request, [NotNullWhen(true)] out string? dPopProof)
    {
        dPopProof = request.Headers[OidcConstants.HttpHeaders.DPoP].SingleOrDefault();

        return !string.IsNullOrWhiteSpace(dPopProof);
    }

    public static string CreateThumbprint(this JsonWebKey jwk)
    {
        return Base64Url.Encode(jwk.ComputeJwkThumbprint());
    }
}