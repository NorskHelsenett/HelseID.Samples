namespace HelseId.Samples.Common.Configuration;

public class SampleApiOptions : ConfigurationOptions
{
    public override string FeatureName => "SampleApiFeatures";

    public string SampleApi1Url { get; set; } = string.Empty;
    
    public string SampleApi1Audience { get; set; } = string.Empty;

    public string SampleApi2Url { get; set; } = string.Empty;
    
    public string SampleApi2Audience { get; set; } = string.Empty;
}