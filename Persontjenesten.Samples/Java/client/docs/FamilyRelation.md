

# FamilyRelation

Family relation between Person and a related person.  The related person has either a valid identification number (nin or d-number) or RelatedBiPerson object  <br>FREG: Familierelasjon

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**relatedPerson** | **String** | The identification number (nin or d-number) for the related person  &lt;br&gt;Remarks: Is null if RelatedPersonWithoutIdentifier is used |  [optional] |
|**relatedPersonWithoutIdentifier** | [**FamilyRelationRelatedPersonWithoutIdentifier**](FamilyRelationRelatedPersonWithoutIdentifier.md) |  |  [optional] |
|**relatedPersonsRole** | **FamilyRelationType** | The relation type seen from the RelatedPerson or RelatedBiPersons view  Freg: RelatertPersonsRolle |  [optional] |
|**myRoleForRelatedPerson** | **FamilyRelationType** | The relation type seen from the Persons view  Freg: MinRolleForPerson |  [optional] |



