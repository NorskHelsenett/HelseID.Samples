using System.Text.Json;
using System.Text.Json.Serialization;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;

namespace HelseId.Samples.ApiAccess.AccessTokenUpdaters;

// The GetRefreshTokenExpiresInValue method in this class gets the 'rt_expires_in' value
// from the token response and parses its value to an int that is returned to the caller.
public static class TokenResponseExtensions
{
    public static int GetRefreshTokenExpiresInValue(this TokenResponse tokenResponse)
    {
        if (tokenResponse.Raw == null)
        {
            throw new Exception($"No valid raw property found in token response");
        }
        var tokenResponseFragment = JsonSerializer.Deserialize<TokenResponseFragment?>(tokenResponse.Raw);
        if (tokenResponseFragment == null)
        {
            throw new Exception($"No {HelseIdConstants.RefreshTokenExpiresIn} property found in token response");
        }

        return tokenResponseFragment.RefreshTokenExpiresIn;

    }

    // This is used for getting the HelseID-specific parameter for the refresh token expiration time
    private class TokenResponseFragment
    {
        [JsonPropertyName(HelseIdConstants.RefreshTokenExpiresIn)]
        public int RefreshTokenExpiresIn { get; set; }
    }
}