namespace TestTokenProxy.Models;

public class SuccessResponse
{
    public string AccessTokenJwt { get; set; } = string.Empty;

    public string? DPoPProof { get; set; } = string.Empty;
}