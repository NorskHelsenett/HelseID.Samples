

# FalseIdentity

Information regarding false identity on a person  An identity belonging to another person then the person using it.  Or not belonging to anyone at all, but presented as a real identity                Persons who is currently or have had their NIN (Norwegian Identification Number)  misused by others, will not have information about the misuse.  Neither will there be info on the person misusing another persons identity.  <br>FREG: FalskIdentitet

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**correctIdentityFromInformation** | [**FalseIdentityCorrectIdentityFromInformation**](FalseIdentityCorrectIdentityFromInformation.md) |  |  [optional] |
|**isCorrectIdentityUnknown** | **Boolean** | If true, the correct identity for the false identity is not known  &lt;br&gt;FREG: RettIdentitetErUkjent |  [optional] |
|**correctIdentityFromIdentificationNumber** | **String** | The correct identity for the false identity  is known by the Norwegian Identification Number (fødselsnummer or D-number)  &lt;br&gt;FREG: RettIdentitetVedIdentifikasjonsnummer |  [optional] |
|**isFalseIdentity** | **Boolean** | &lt;br&gt;Remarks:               Should always be true              Freg: ErFalsk |  [optional] |



