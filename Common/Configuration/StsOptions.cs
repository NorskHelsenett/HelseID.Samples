namespace HelseId.Samples.Common.Configuration;

public class StsOptions : ConfigurationOptions
{
    public override string FeatureName => "HelseIdFeatures";
    public string StsUrl { get; set; } = string.Empty;
}