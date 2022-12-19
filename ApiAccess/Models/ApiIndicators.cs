namespace HelseId.Samples.ApiAccess.Models;

// This sets up parameters for accessing an API
public class ApiIndicators
{
    // The API audience, always needed
    public string ApiAudience { get; init; } = string.Empty;
    
    // A resource indicator, only needed when the first call to the authorization endpoint 
    // had resource indicators attached
    public string? ResourceIndicator { get; init; }
}