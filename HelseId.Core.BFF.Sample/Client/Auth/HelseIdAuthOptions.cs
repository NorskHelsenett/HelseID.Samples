using System.ComponentModel.DataAnnotations;

namespace HelseId.Core.BFF.Sample.Client.Auth;

public class HelseIdAuthOptions
{
    [Required]
    public string Authority { get; set; } = "";

    [Required]
    public string ClientId { get; set; } = "";

    [Required]
    public string ClientJwk { get; set; } = "";

    [Required]
    public string Scopes { get; set; } = "";

    public string? AcrValues { get; set; }
}