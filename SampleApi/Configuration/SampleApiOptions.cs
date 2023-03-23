using HelseID.Samples.Configuration;

namespace HelseId.SampleApi.Configuration;

public class ApiOptions
{
    public const string StandardApiAudience = ConfigurationValues.SampleApiNameAudience;

    public const string ClientCredentialsResource = ConfigurationValues.SampleApiMachineClientResource;
    
    public const string ClientCredentialsScope = ConfigurationValues.ClientCredentialsScopeForSampleApi;

    public const string AuthorizationCodeResource = ConfigurationValues.AuthCodeClientResource;
    
    public const string AuthorizationCodeScope = ConfigurationValues.AuthorizationCodeScopeForSampleApi;

    public const string ResourceIndicators1Resource = ConfigurationValues.ResourceIndicators1Resource;
    
    public const string ResourceIndicators1Audience = ConfigurationValues.SampleApiForResourceIndicators1Audience;

    public const string ResourceIndicators1Scope = ConfigurationValues.SampleApiForResourceIndicators1Scope;
    
    public const string ResourceIndicators2Resource = ConfigurationValues.ResourceIndicators2Resource;
    
    public const string ResourceIndicators2Audience = ConfigurationValues.SampleApiForResourceIndicators2Audience;

    public const string ResourceIndicators2Scope = ConfigurationValues.SampleApiForResourceIndicators2Scope;
    
    public const int ApiPort = ConfigurationValues.SampleApiPort;
}
