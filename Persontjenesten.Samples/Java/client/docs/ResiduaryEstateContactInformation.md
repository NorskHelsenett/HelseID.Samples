

# ResiduaryEstateContactInformation

Residuary estate contact information  Will be either a Person, a Lawyer or an Organization.  <br>FREG: KontaktinformasjonForDoedsbo

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**person** | [**ResiduaryEstateContactInformationPerson**](ResiduaryEstateContactInformationPerson.md) |  |  [optional] |
|**lawyer** | [**ResiduaryEstateContactInformationLawyer**](ResiduaryEstateContactInformationLawyer.md) |  |  [optional] |
|**organization** | [**ResiduaryEstateContactInformationOrganization**](ResiduaryEstateContactInformationOrganization.md) |  |  [optional] |
|**probateCertificateType** | **ProbateCertificateType** | Probate certificate type  &lt;br&gt;FREG: Skifteform |  [optional] |
|**probateCertificateIssueDate** | **OffsetDateTime** | Date of the probate certificate issuance  &lt;br&gt;FREG: Attestutstedelsesdato |  [optional] |
|**contactAddress** | [**ResiduaryEstateContactInformationContactAddress**](ResiduaryEstateContactInformationContactAddress.md) |  |  [optional] |



