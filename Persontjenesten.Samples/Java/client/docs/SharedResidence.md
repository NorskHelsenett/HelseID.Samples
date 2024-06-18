

# SharedResidence

Address information on shared parenting, shared residence or joint residence  <br>Remarks:   NB! This may be outdated!  Only one of StreetAddress, CadastralAddress  or UnknownResidence is in use.  Contains address information based on the original shared parental agreement.  This address is a copy of the address for the parent not sharing residence  with the child at that point.  This address will not be updated if that parent moves.                See also Person.ResidentialAddress                Freg: Folkeregisterperson.DeltBosted

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**streetAddress** | [**SharedResidenceStreetAddress**](SharedResidenceStreetAddress.md) |  |  [optional] |
|**cadastralAddress** | [**SharedResidenceCadastralAddress**](SharedResidenceCadastralAddress.md) |  |  [optional] |
|**unknownResidence** | [**SharedResidenceUnknownResidence**](SharedResidenceUnknownResidence.md) |  |  [optional] |
|**contractValidFrom** | **OffsetDateTime** | Start date for the agreement between parents for  the shared residence  &lt;br&gt;FREG: StartdatoForKontrak |  [optional] |
|**contractValidTo** | **OffsetDateTime** | End date for the agreement between parents for  the shared residence  &lt;br&gt;Remarks:   May not have a end date  Freg: SluttdatoForKontrak |  [optional] |
|**cadastralIdentifier** | **String** | Unique identifier from the Mapping Authority.  Used to catch changes to the Cadastral  &lt;br&gt;FREG: AdresseidentifikatorFraMatrikkelen |  [optional] |
|**closeCadastralIdentifier** | **String** | ResidentialAddress.CloseCadastralIdentifier  &lt;br&gt;FREG: NaerAdresseIdentifikatorFraMatrikkelen |  [optional] |
|**addressConfidentiality** | **AddressConfidentiality** | Describes with which confidentiality the address  should be handled  &lt;br&gt;FREG: Adressegradering |  [optional] |
|**urbanDistrictCode** | **String** | Six digit code for the urban district that provide municipal health- and social services for the address.  Service providing districts are defined by the city, but only Oslo Kommune  has a classification that differs from the geographical classification defined by SSB.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim. |  [optional] |
|**urbanDistrictName** | **String** | The name of the service providing urban district.  &lt;br&gt;SSB: Bydelsnavn |  [optional] |
|**geographicalUrbanDistrictCode** | **String** | Six digit code for the geographical urban district the address belongs to.  Urban districts are only used in Oslo, Bergen, Stavanger and Trondheim.  Geographical urban districts are defined by SSB in classification 103.  !:https://www.ssb.no/klass/klassifikasjoner/103&lt;br&gt;SSB: Bydelskode |  [optional] |
|**geographicalUrbanDistrictName** | **String** | The name of the geographical urban district  &lt;br&gt;SSB: Bydelsnavn |  [optional] |
|**basicStatisticalUnit** | **Long** | Three to four digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  Should be interpreted as a four digit string with leading zero.  Value 401 means \&quot;0401\&quot; and so on. For shared residences, the basic statistical unit  is enriched by the Person API, derived from the street or cadastral address. |  [optional] |
|**fullBasicStatisticalUnitNumber** | **String** | Eight digit code used to divide a municipality in small, stable geographical units,  used as basis for regional statistical analysis.  The first four digits is the municipality number,  followed by four digits identifying the area.  For shared residences, the basic statistical unit  is enriched by the Person API, attempted to be derived from the street or cadastral address  using data from Kartverket (Matrikkelen).  Basic statistical units are defined by SSB in classification 1.  !:https://www.ssb.no/klass/klassifikasjoner/1&lt;br&gt;SSB: Grunnkretsnummer |  [optional] |
|**basicStatisticalUnitName** | **String** | The name of the basic statistical unit.  &lt;br&gt;SSB: BasicStatisticalUnitName |  [optional] |



