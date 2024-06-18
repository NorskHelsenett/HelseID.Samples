

# ContactAddressStreetAddress

<br>FREG: Vegadresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**coAddressName** | **String** | Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn |  [optional] |
|**addressName** | **String** | Name of a street, road, path, place or area  as registered in the cadastral of a municipality  &lt;br&gt;Remarks:   Does not contain street address number (housing number and lettering)  Freg: Adressenavn |  [optional] |
|**streetAddressNumber** | [**StreetAddressAddressNumber**](StreetAddressAddressNumber.md) |  |  [optional] |
|**city** | [**CadastralAddressCity**](CadastralAddressCity.md) |  |  [optional] |
|**addressCode** | **String** | Number that uniquely identifies an addressable  street, road, path, place or area  &lt;br&gt;Remarks:   Known as StreetCode (\&quot;gatekode\&quot;) in DSF  Freg: Adressekode |  [optional] |
|**addressAdditionalName** | **String** | Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn |  [optional] |
|**separatelyOccupiedUnitNumber** | **String** | A letter and four digits that uniquely identifies the  separately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer |  [optional] |



