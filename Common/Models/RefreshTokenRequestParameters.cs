namespace HelseId.Samples.Common.Models;

public class RefreshTokenRequestParameters : TokenRequestParameters
{
    public RefreshTokenRequestParameters(string refreshToken, string? resource)
    {
        RefreshToken = refreshToken;
        if (resource != null)
        {
            HasResourceIndicator = true;
            Resource.Add(resource);
        }
    }
    
    public string RefreshToken { get; }

    public List<string> Resource { get; } = new();
    
    public bool HasResourceIndicator { get; }
}