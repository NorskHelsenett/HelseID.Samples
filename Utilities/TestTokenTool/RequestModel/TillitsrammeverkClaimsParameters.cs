namespace TestTokenTool.RequestModel;

public class TillitsrammeverkClaimsParameters
{
    public bool DontSetPractitionerHprNr { get; set; }
    public bool DontSetPractitionerAuthorization { get; set; }
    public bool DontSetPractitionerDepartment { get; set; }
    public bool DontSetCareRelationshipHealthcareService { get; set; }
    public bool DontSetCareRelationshipPurposeOfUseDetails { get; set; }
    public bool DontSetPatientsPointOfCare { get; set; }
    public bool DontSetPatientsDepartment { get; set; }

    public string PractitionerIdentifierSystem { get; set; } = string.Empty;
    public string PractitionerIdentifierAuthority { get; set; } = string.Empty;

    public string PractitionerHprNrSystem { get; set; } = string.Empty;
    public string PractitionerHprNrAuthority { get; set; } = string.Empty;

    public string PractitionerAuthorizationCode { get; set; } = string.Empty;
    public string PractitionerAuthorizationText { get; set; } = string.Empty;
    public string PractitionerAuthorizationSystem { get; set; } = string.Empty;
    public string PractitionerAuthorizationAssigner { get; set; } = string.Empty;

    public string PractitionerLegalEntityId { get; set; } = string.Empty;
    public string PractitionerLegalEntityName { get; set; } = string.Empty;
    public string PractitionerLegalEntitySystem { get; set; } = string.Empty;
    public string PractitionerLegalEntityAuthority { get; set; } = string.Empty;

    public string PractitionerPointOfCareId { get; set; } = string.Empty;
    public string PractitionerPointOfCareName { get; set; } = string.Empty;
    public string PractitionerPointOfCareSystem { get; set; } = string.Empty;
    public string PractitionerPointOfCareAuthority { get; set; } = string.Empty;

    public string PractitionerDepartmentId { get; set; } = string.Empty;
    public string PractitionerDepartmentName { get; set; } = string.Empty;
    public string PractitionerDepartmentSystem { get; set; } = string.Empty;
    public string PractitionerDepartmentAuthority { get; set; } = string.Empty;

    public string CareRelationshipHealthcareServiceCode { get; set; } = string.Empty;
    public string CareRelationshipHealthcareServiceText { get; set; } = string.Empty;
    public string CareRelationshipHealthcareSystem { get; set; } = string.Empty;
    public string CareRelationshipHealthcareAssigner { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseText { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseSystem { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseAssigner { get; set; } = string.Empty;

    public string CareRelationshipPurposeOfUseDetailsCode { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseDetailsText { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseDetailsSystem { get; set; } = string.Empty;
    public string CareRelationshipPurposeOfUseDetailsAssigner { get; set; } = string.Empty;

    public string CareRelationshipDecisionRefId { get; set; } = string.Empty;
    public bool CareRelationshipDecisionRefUserSelected { get; set; } = true;

    public string PatientsPointOfCareId { get; set; } = string.Empty;
    public string PatientsPointOfCareName { get; set; } = string.Empty;
    public string PatientsPointOfCareSystem { get; set; } = string.Empty;
    public string PatientsPointOfCareAuthority { get; set; } = string.Empty;

    public string PatientsDepartmentId { get; set; } = string.Empty;
    public string PatientsDepartmentName { get; set; } = string.Empty;
    public string PatientsDepartmentSystem { get; set; } = string.Empty;
    public string PatientsDepartmentAuthority { get; set; } = string.Empty;
}
