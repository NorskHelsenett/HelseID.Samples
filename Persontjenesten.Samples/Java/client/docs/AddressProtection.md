

# AddressProtection

This information is used to describe the overall protection level a person have on all his addresses of types  residential, living and shared residence addresses. The address protection does not apply to  contact addresses.  <br>FREG: GraderingAvAdresse

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**protectionLevel** | **AddressConfidentiality** | Overall protection level on addresses.  Legal values are:  AddressConfidentiality.UnclassifiedAddressConfidentiality.ConfidentialAddressConfidentiality.StrictlyConfidential&lt;br&gt;FREG: Graderingsnivaa |  [optional] |



