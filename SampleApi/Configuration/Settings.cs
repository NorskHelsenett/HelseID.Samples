using HelseID.Samples.Configuration;

namespace HelseId.SampleApi.Configuration;

public class Settings
{
    public string Authority => ConfigurationValues.StsUrl;

    public string Audience { get; init; } = string.Empty;
    
    public string ClientCredentialsApiScopeForSampleApi => ConfigurationValues.ClientCredentialsScopeForSampleApi;

    public string AuthCodeApiScopeForSampleApi { get; init; } = string.Empty;
    
    public int ApiPort { get; init; }
}
