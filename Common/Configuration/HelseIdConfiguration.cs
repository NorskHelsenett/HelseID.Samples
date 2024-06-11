namespace HelseId.Samples.Common.Configuration;

/// <summary>
/// This class contains configurations that correspond to clients in HelseID.
/// </summary>
public class HelseIdConfiguration
{
    public HelseIdConfiguration(
        SecurityKey privateKeyJwk,
        string clientId,
        string scope,
        string stsUrl,
        List<string>? resourceIndicators = null)
    {
        PrivateKeyJwk = privateKeyJwk;
        ClientId = clientId;
        Scope = scope;
        StsUrl = stsUrl;
        if (resourceIndicators != null)
        {
            ResourceIndicators = resourceIndicators;
        }
    }

    // The private key MUST be properly secured inside the client.
    public SecurityKey PrivateKeyJwk  { get; }

    public string ClientId { get; }

    public string Scope { get;  }

    public string StsUrl { get; }

    // These are used for clients that are using resource indicators against the authorization and token endpoints:
    public List<string> ResourceIndicators { get; } = new();
}
