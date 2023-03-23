using HelseId.Samples.Common.Configuration;

namespace HelseId.SampleApiForTokenExchange;

public class ApiOptions : ConfigurationOptions
{
    public override string FeatureName => "ApiFeatures";

    public string Audience { get; set; } = string.Empty;
}