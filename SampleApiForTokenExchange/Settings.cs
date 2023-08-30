using HelseID.Samples.Configuration;

namespace HelseId.SampleApiForTokenExchange;

public class Settings
{
    public Settings(bool useDPoP)
    {
        UseDPoP = useDPoP;
    }
    
    public string Authority => ConfigurationValues.StsUrl;
    
    public string Audience => ConfigurationValues.SampleTokenExchangeApiAudience;

    public string TokenExchangeApiScope => ConfigurationValues.TokenExchangeApiScope;

    public bool UseDPoP { get; }
}
