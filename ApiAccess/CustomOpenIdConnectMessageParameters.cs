namespace HelseId.Samples.ApiAccess;

public class CustomOpenIdConnectMessageParameters
{
    public bool HasRequestObject => RequestObject != string.Empty;

    public string RequestObject { get; init; } = string.Empty;

    public List<string> ResourceIndicators { get; init; } = new();
}