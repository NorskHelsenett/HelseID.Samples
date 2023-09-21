namespace TestTokenTool.RequestModel;

public class DokumentdelingClaimsParameters
{
    public string CareRelationshipHealthcareServiceCode { get; set; } = string.Empty;
    public string CareRelationshipHealthcareServiceText { get; set; } = string.Empty;
    
    public string CareRelationshipDepartmentId { get; set; } = string.Empty;
    public string CareRelationshipDepartmentName { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseText { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseDetailsCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseDetailsText { get; set; } = string.Empty;

    public string CareRelationshipTracingRefId { get; set; } = string.Empty;
}