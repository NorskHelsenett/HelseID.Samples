using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess.Models;

public class UserSessionData
{
    public string SessionId { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiresAtUtc { get; set; }
    public Organization SelectedOrganization { get; set; } = new();
    public AccessTokenDictionary AccessTokens { get; set; } = new();
}