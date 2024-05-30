

# SharedResidenceStreetAddress

The street address for the shared residence  <br>Remarks:   May be missing, only one of the address elements are used. See ResidentialAddress  Freg: Vegadresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**municipalityNumber** | **String** | A four digit code identifying a municipality or municipality-like area.  Municipalities are defined by SSB in classification 131.  !:https://www.ssb.no/klass/klassifikasjoner/131&lt;br&gt;FREG: Kommunenummer |  [optional] |
|**municipalityName** | **String** | The name of the municipality. Some municipalities also include names in Sámi languages.  &lt;br&gt;SSB: Kommunenavn |  [optional] |
|**countyNumber** | **String** | A two digit code identifying a county.  Counties are defined by SSB in classification 104.  !:https://www.ssb.no/klass/klassifikasjoner/104&lt;br&gt;SSB: Fylkesnummer |  [optional] |
|**countyName** | **String** | The name of the county. Some counties also include names in Sámi languages.  &lt;br&gt;SSB: Fylkesnavn |  [optional] |
|**separatelyOccupiedUnitNumber** | **String** | A letter and four digits that uniquely identifies the  sperately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer |  [optional] |
|**separatelyOccupiedUnitType** | **SeparatelyOccupiedUnitType** | Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype |  [optional] |
|**addressName** | **String** | Name of a street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;Remarks:   Does not contain street address number (housing number and lettering)  Freg: Adressenavn |  [optional] |
|**addressNumber** | [**StreetAddressAddressNumber**](StreetAddressAddressNumber.md) |  |  [optional] |
|**addressCode** | **String** | Number that uniquely identifies an addressable  street, road, path, place or area  &lt;br&gt;Remarks:   Known as StreetCode (\&quot;gatekode\&quot;) in DSF  Freg: Adressekode |  [optional] |
|**addressAdditionalName** | **String** | Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn |  [optional] |
|**city** | [**CadastralAddressCity**](CadastralAddressCity.md) |  |  [optional] |
|**coAddressName** | **String** | Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn |  [optional] |



