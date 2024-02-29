using HelseId.Samples.Common.Configuration;

namespace HelseId.Samples.ApiAccess.Configuration;

public class Settings
{
    public ClientType ClientType { get; init; }

    public string ApiUrl1 { get; init; } = string.Empty;

    public string ApiUrl2 { get; init; } = string.Empty;

    public string ApiAudience1 { get; init; } = string.Empty;

    public string ApiAudience2 { get; init; } = string.Empty;

    public HelseIdConfiguration HelseIdConfiguration { get; init; } = null!;
}
