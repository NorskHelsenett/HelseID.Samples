

# CadastralAddress

Address for specifying areas in a municipality that has yet to get a StreetAddress<br>FREG: Matrikkeladresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**separatelyOccupiedUnitNumber** | **String** | A letter and four digits that uniquely identifies the  separately occupied unit inside a addressable building  or part of a building  &lt;br&gt;Remarks:   Two first digits represent floor number  Freg: Bruksenhetsnummer |  [optional] |
|**separatelyOccupiedUnitType** | **SeparatelyOccupiedUnitType** | Categorization of occupancy unit type  &lt;br&gt;FREG: Bruksenhetstype |  [optional] |
|**cadastralNumber** | [**CadastralAddressCadastralNumber**](CadastralAddressCadastralNumber.md) |  |  [optional] |
|**subNumber** | **Long** | Used with CadastralNumber when a real estate property  is linked to several different addresses  &lt;br&gt;Remarks:   I.e. each building on a farmyard has a sub number  Freg: Undernummer |  [optional] |
|**addressAdditionalName** | **String** | Inherited farm name (bruksnavn) or name of a institution or building,  used as a part of the official address  &lt;br&gt;FREG: Addressetilleggsnavn |  [optional] |
|**city** | [**CadastralAddressCity**](CadastralAddressCity.md) |  |  [optional] |
|**coAddressName** | **String** | Description of who the recipient is in care of (C/O),  or which recipient in an organization (v/ &#x3D; with, or Att: &#x3D; \&quot;Attention\&quot;)  &lt;br&gt;FREG: CoAdressenavn |  [optional] |



