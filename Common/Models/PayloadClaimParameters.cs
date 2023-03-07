namespace HelseId.Samples.Common.Models;

public class PayloadClaimParameters
{
    public bool IsAuthCodeRequest { get; set; } = false;
    public string ParentOrganizationNumber { get; set; } = string.Empty;
    public string ChildOrganizationNumber { get; set; } = string.Empty;
    public string ContextualClaimType { get; set; } = string.Empty;
}