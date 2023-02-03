namespace HelseId.Samples.ApiAccess.Models;

public class AccessTokenWithExpiration {

    public AccessTokenWithExpiration(string accessToken, DateTime accessTokenExpiresAtUtc)
    {
        AccessToken = accessToken;
        AccessTokenExpiresAtUtc = accessTokenExpiresAtUtc;
    }

    public string AccessToken { get; init; }

    public  DateTime AccessTokenExpiresAtUtc { get; init; }
}