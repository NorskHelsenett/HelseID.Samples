namespace HelseId.Samples.Common.Models;

public class TokenExchangeTokenRequestParameters : TokenRequestParameters
{
    public string SubjectToken { get; init; } = string.Empty;
}