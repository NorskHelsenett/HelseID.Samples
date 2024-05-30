

# PresentAddress

<br>Freg: Oppholdsadresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**foreignAddress** | [**ForeignContactAddressForeignAddress**](ForeignContactAddressForeignAddress.md) |  |  [optional] |
|**isAddressUnknown** | **Boolean** | &lt;br&gt;FREG: AdressenErUkjent |  [optional] |
|**streetAddress** | [**PresentAddressStreetAddress**](PresentAddressStreetAddress.md) |  |  [optional] |
|**cadastralAddress** | [**PresentAddressCadastralAddress**](PresentAddressCadastralAddress.md) |  |  [optional] |
|**cadastralIdentifier** | **String** | Unique identifier from the Norwegian Mapping Authority.  &lt;br&gt;FREG: AdresseIdentifikatorFraMatrikkelen |  [optional] |
|**addressConfidentiality** | **AddressConfidentiality** | &lt;br&gt;FREG: Adressegradering |  [optional] |
|**presentAddressDate** | **OffsetDateTime** | The date of when this living address was reported to the National Population Register (FREG).  &lt;br&gt;FREG: Oppholdsadressedato |  [optional] |
|**stayElsewhere** | **StayElsewhere** | &lt;br&gt;FREG: OppholdAnnetSted |  [optional] |
|**urbanDistrictCode** | **String** | Six digit code for the urban district that provide municipal health- and social services for the address.  Service providing districts are defined by the city, but only Oslo Kommune  has a classification that differs from the geographical classification defined by SSB.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim. |  [optional] |
|**urbanDistrictName** | **String** | The name of the service providing urban district.  &lt;br&gt;SSB: Bydelsnavn |  [optional] |
|**geographicalUrbanDistrictCode** | **String** | Six digit code for the geographical urban district the address belongs to.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim.  Geographical urban districts are defined by SSB in classification 103.  !:https://www.ssb.no/klass/klassifikasjoner/103&lt;br&gt;SSB: Bydelskode |  [optional] |
|**geographicalUrbanDistrictName** | **String** | The name of the geographical urban district  &lt;br&gt;SSB: Bydelsnavn |  [optional] |
|**basicStatisticalUnit** | **Long** | Three to four digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  Should be interpreted as a four digit string with leading zero.  Value 401 means \&quot;0401\&quot; and so on. For present addresses, the basic statistical unit  is enriched by the Person API, derived from the street or cadastral address. |  [optional] |
|**fullBasicStatisticalUnitNumber** | **String** | Eight digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  The first four digits are the municipality number,  followed by four digits with leading zero identifying the area.  For present addresses, the basic statistical unit  is enriched by the Person API, attempted to be derived from the street or cadastral address  using data from Kartverket (Matrikkelen).  Basic statistical units are defined by SSB in classification 1.  !:https://www.ssb.no/klass/klassifikasjoner/1&lt;br&gt;SSB: Grunnkretsnummer |  [optional] |
|**basicStatisticalUnitName** | **String** | The name of the basic statistical unit.  &lt;br&gt;SSB: BasicStatisticalUnitName |  [optional] |



