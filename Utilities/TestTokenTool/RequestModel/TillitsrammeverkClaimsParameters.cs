namespace TestTokenTool.RequestModel;

public class TillitsrammeverkClaimsParameters
{
    public string PractitionerAuthorizationCode { get; set; } = string.Empty; 
    
    public string PractitionerAuthorizationText { get; set; } = string.Empty; 
    
    public string LegalEntityId { get; set; } = string.Empty;

    public string LegalEntityName { get; set; } = string.Empty;

    public string PointOfCareId { get; set; } = string.Empty;

    public string PointOfCareName { get; set; } = string.Empty;
}