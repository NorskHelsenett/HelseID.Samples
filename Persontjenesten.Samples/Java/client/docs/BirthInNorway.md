

# BirthInNorway

Additional information about a persons birth if this person was born in Norway.  <br>FREG: FoedselINorge

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**organizationName** | **String** | The organization/institution where the person was born, for example \&quot;Molde sjukehus\&quot;.  &lt;br&gt;FREG: Foedselsinstitusjonsnavn |  [optional] |
|**multipleBirthNumber** | **Long** | The number of this person if there were multiple births. This number is a combination of multiple data points:  First number &#x3D; the total number of children in multiple births  Second number &#x3D; this persons number in the sequence                Example: \&quot;21\&quot; means that this child was the firstborn twin (this person was the first of two births)  &lt;br&gt;FREG: Rekkefoelgenummer |  [optional] |



