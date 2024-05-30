

# PersonIdentification

Identification for a person, used for foreign persons identification and immigration authorities identification  <br>FREG: Personidentifikasjon  

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**identificationNumber** | **String** | The identification number  &lt;br&gt;FREG: Identifikasjonsnummer |  [optional] |
|**identificationNumberType** | **String** | Type of foreign identification  Possible values:  - utenlandskIdentifikasjonsnummer &#x3D; Foreign identification number  - taxIdentificationNumber &#x3D; Tax Identification Number(TIN)  - socialSecurityNumber &#x3D; Social Security Number(SSN)  - utlendingsmyndighetenesIdentifikasjonsnummer &#x3D; \&quot;DUF-nummer\&quot; from the Norwegian UDI, for now  &lt;br&gt;FREG: Identifikasjonsnummertype |  [optional] |
|**issuerCountry** | **String** | Country codes given in ISO 3166-1 Alpha 3  Always NOR for Person.ImmigrationAuthoritiesIdentificationNumber  XXK &#x3D; Kosovo  &lt;br&gt;FREG: Utstederland |  [optional] |



