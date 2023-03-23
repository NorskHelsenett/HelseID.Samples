namespace HelseId.Samples.Common.Configuration;

public class ClientOptions : ConfigurationOptions
{
    public override string FeatureName => "ClientFeatures";
    public string ClientId { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public List<string> ResourceIndicators { get; set; } = new();
}