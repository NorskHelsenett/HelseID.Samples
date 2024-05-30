

# ContactAddress

<br>FREG: Kontaktadresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**addressConfidentiality** | **AddressConfidentiality** | &lt;br&gt;FREG: Adressegradering |  [optional] |
|**postBoxAddress** | [**ContactAddressPostBoxAddress**](ContactAddressPostBoxAddress.md) |  |  [optional] |
|**streetAddress** | [**ContactAddressStreetAddress**](ContactAddressStreetAddress.md) |  |  [optional] |
|**freeFormPostalAddress** | [**ContactAddressFreeFormPostalAddress**](ContactAddressFreeFormPostalAddress.md) |  |  [optional] |
|**cadastralIdentifier** | **String** | Unique identifier from the Norwegian Mapping Authority.  &lt;br&gt;FREG: AdresseIdentifikatorFraMatrikkelen |  [optional] |



