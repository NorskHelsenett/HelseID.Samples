

# MaritalStatus

Martial status  <br>FREG: Sivilstand

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**status** | **MaritalStatusType** | Marital status type  &lt;br&gt;FREG: Sivilstand |  [optional] |
|**statusDate** | **OffsetDateTime** | Date of  the marial status  &lt;br&gt;FREG: SivilstandDato |  [optional] |
|**authority** | **String** | Code value for which authority that has granted the marital status  If the source of the Marital Status is DSF, the current values:  DSF er denne verdien representert med vigselstype (VIGSELSTYPE)  1 &#x3D; The Norwegian Church  2 &#x3D; Civil  3 &#x3D; Dissenter for  a non-established Church or other denominations with marital rights  4 &#x3D; Abroad - Foreign authority  Updated 2021.10.14  &lt;br&gt;FREG: Myndighet |  [optional] |
|**municipalityNumber** | **String** | A four digit code identifying the municipality where the marital status was changed.  Municipalities are defined by SSB in classification 131.  !:https://www.ssb.no/klass/klassifikasjoner/131&lt;br&gt;FREG: Kommune |  [optional] |
|**municipalityName** | **String** | The name of the municipality. Some municipalities also include names in Sámi languages.  &lt;br&gt;SSB: Kommunenavn |  [optional] |
|**countyNumber** | **String** | A number identifying a county   &lt;br&gt;SSB: Fylkesnummer |  [optional] |
|**countyName** | **String** | A name identifying a county  &lt;br&gt;SSB: Fylkesnavn |  [optional] |
|**place** | **String** | Name of the place in Norway or abroad where the status change was made.  May be an address or less exact  &lt;br&gt;FREG: Sted |  [optional] |
|**abroad** | **String** | Name of country if happened abroad  &lt;br&gt;FREG: Utland |  [optional] |
|**relatedByMaritalStatus** | **String** | NIN for person related by marital status  &lt;br&gt;FREG: RelatertVedSivilstatus |  [optional] |



