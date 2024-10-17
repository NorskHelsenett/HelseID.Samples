namespace HelseId.Samples.TestTokenDemo.TttModels.Request;

public class TillitsrammeverkClaimsParameters
{
    public string PractitionerAuthorizationCode { get; set; } = string.Empty;
    public string PractitionerAuthorizationText { get; set; } = string.Empty;

    public string PractitionerLegalEntityId { get; set; } = string.Empty;
    public string PractitionerLegalEntityName { get; set; } = string.Empty;

    public string PractitionerPointOfCareId { get; set; } = string.Empty;
    public string PractitionerPointOfCareName { get; set; } = string.Empty;

    public string PractitionerDepartmentId { get; set; } = string.Empty;
    public string PractitionerDepartmentName { get; set; } = string.Empty;

    public string CareRelationshipHealthcareServiceCode { get; set; } = string.Empty;
    public string CareRelationshipHealthcareServiceText { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseText { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseDetailsCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseDetailsText { get; set; } = string.Empty;

    public string CareRelationshipTracingRefId { get; set; } = string.Empty;

    public string PatientsPointOfCareId { get; set; } = string.Empty;
    public string PatientsPointOfCareName { get; set; } = string.Empty;

    public string PatientsDepartmentId { get; set; } = string.Empty;
    public string PatientsDepartmentName { get; set; } = string.Empty;
}
