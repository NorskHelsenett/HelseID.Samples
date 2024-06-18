

# ResidentialAddress

Address where person is registered to live.  Only one of StreetAddress, CadastralAddress  or UnknownResidence is in use  <br>FREG: Bostedsadresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**streetAddress** | [**ResidentialAddressStreetAddress**](ResidentialAddressStreetAddress.md) |  |  [optional] |
|**cadastralAddress** | [**ResidentialAddressCadastralAddress**](ResidentialAddressCadastralAddress.md) |  |  [optional] |
|**unknownResidence** | [**ResidentialAddressUnknownResidence**](ResidentialAddressUnknownResidence.md) |  |  [optional] |
|**cadastralIdentifier** | **String** | Unique identifier from the Norwegian Mapping Authority.  &lt;br&gt;FREG: AdresseIdentifikatorFraMatrikkelen |  [optional] |
|**closeCadastralIdentifier** | **String** | Unique identifier from the Mapping Authority.  If the Cadastral information used in CadastralIdentifier  is inaccurate, CloseCadastralIdentifier will contain the  unqiue identifier for a near by house.  I.e. If the house letter is not known, the CloseCadastralIdentifier  could point to the house with letter \&quot;A\&quot;, while CadastralIdentifier  would point to all the house number, without letters (which could be multiple houses)  Used to catch changes to the Cadastral to a nearby  &lt;br&gt;Remarks:   (07.02.2020) Not in use  Freg: NaerAdresseIdentifikatorFraMatrikkelen |  [optional] |
|**addressConfidentiality** | **AddressConfidentiality** | Describes with which confidentiality the address  should be handled  &lt;br&gt;FREG: Adressegradering |  [optional] |
|**moveDate** | **OffsetDateTime** | Reported as move date by person  &lt;br&gt;FREG: Flyttedato |  [optional] |
|**basicStatisticalUnit** | **Long** | Three to four digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  Should be interpreted as a four digit string with leading zero.  Value 401 means \&quot;0401\&quot; and so on. For residential addresses, the basic statistical unit  is provided by FREG.  &lt;br&gt;FREG: Grunnkrets |  [optional] |
|**fullBasicStatisticalUnitNumber** | **String** | Eight digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  The first four digits are the municipality number,  followed by four digits with leading zero identifying the area.  Basic statistical units are defined by SSB in classification 1.  !:https://www.ssb.no/klass/klassifikasjoner/1&lt;br&gt;SSB: Grunnkretsnummer |  [optional] |
|**basicStatisticalUnitName** | **String** | The name of the basic statistical unit.  &lt;br&gt;SSB: BasicStatisticalUnitName |  [optional] |
|**constituency** | **Long** | Unique code by the municipality used as  geographical division of the municipality set by the  electoral committee  &lt;br&gt;Remarks:   Also knows as in the voting constituency Cadastral  Freg: Stemmekrets (Valgkrets i matrikkelen) |  [optional] |
|**schoolDistrict** | **Long** | Unique code by the municipality used to describe  a geographical division, used as a non-binding school  affiliation for kids in the area  &lt;br&gt;FREG: Skolekrets |  [optional] |
|**churchDistrict** | **Long** | Unique code for the parish (kirke sogn)  Parish is the basic unit of the Norwegian Church.  A church district can extend over several municipalities  &lt;br&gt;FREG: Kirkekrets |  [optional] |
|**urbanDistrictCode** | **String** | Six digit code for the urban district that provide municipal health- and social services for the address.  Service providing districts are defined by the city, but only Oslo Kommune  has a classification that differs from the geographical classification defined by SSB.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim. |  [optional] |
|**urbanDistrictName** | **String** | The name of the service providing urban district.  &lt;br&gt;SSB: Bydelsnavn |  [optional] |
|**geographicalUrbanDistrictCode** | **String** | Six digit code for the geographical urban district the address belongs to.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim.  Geographical urban districts are defined by SSB in classification 103.  !:https://www.ssb.no/klass/klassifikasjoner/103&lt;br&gt;SSB: Bydelskode |  [optional] |
|**geographicalUrbanDistrictName** | **String** | The name of the geographical urban district  &lt;br&gt;SSB: Bydelsnavn |  [optional] |



