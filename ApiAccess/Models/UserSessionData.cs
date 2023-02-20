namespace HelseId.Samples.ApiAccess.Models;

public class UserSessionData
{
    public string SessionId { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAtUtc { get; set; }
    // Used if the user has selected a specific organization
    public Organization SelectedOrganization { get; set; } = new();
    public AccessTokenDictionary AccessTokens { get; } = new();
}