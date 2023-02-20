using HelseID.Samples.Configuration;

namespace HelseId.SampleApiForTokenExchange;

public class Settings
{
    public string Authority => ConfigurationValues.StsUrl;
    public string Audience => ConfigurationValues.SampleTokenExchangeApiAudience;
    public string TokenExchangeApiScope => ConfigurationValues.TokenExchangeApiScope;
}
