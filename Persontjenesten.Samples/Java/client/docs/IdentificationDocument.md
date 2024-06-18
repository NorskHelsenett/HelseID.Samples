

# IdentificationDocument

Identification Document contains information on what documentation  was used to verify the identity of the person  <br>FREG: Identifikasjonsdokument / Legitimasjonsdokument

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**registeredAt** | **OffsetDateTime** | &lt;br&gt;FREG: Ajourholdstidspunkt |  [optional] |
|**isValid** | **Boolean** | &lt;br&gt;FREG: ErGjeldende |  [optional] |
|**source** | **String** | &lt;br&gt;FREG: Kilde |  [optional] |
|**reason** | **String** | &lt;br&gt;FREG: Aarsak |  [optional] |
|**validFrom** | **OffsetDateTime** | &lt;br&gt;FREG: Gyldighetstidspunkt |  [optional] |
|**validTo** | **OffsetDateTime** | &lt;br&gt;FREG: Opphoerstidspunkt |  [optional] |
|**documentNumber** | **String** | Identification number on the document used for identity verification  I.e. a passport number  &lt;br&gt;FREG: Identifikasjondokumentnummer |  [optional] |
|**documentType** | **String** | Type of documentation.  I.e. driver license or passport  &lt;br&gt;FREG: Identifikasjondokumenttype |  [optional] |
|**issuerCountry** | **String** | Name of the country the document is issued in  &lt;br&gt;Remarks:   Newest updates uses ISO 3166-1 Alpha 3 for country codes  XXK &#x3D; Kosovo even if document contains \&quot;RKS\&quot;  Freg: Utstederland |  [optional] |
|**issuerName** | **String** | Name of the issuer  &lt;br&gt;FREG: Utstedernavn |  [optional] |
|**documentValidFrom** | **OffsetDateTime** | Time and date document is valid from  &lt;br&gt;FREG: GyldigFra |  [optional] |
|**documentValidTo** | **OffsetDateTime** | Time and date document is valid to  &lt;br&gt;FREG: GyldigTil |  [optional] |
|**documentVerification** | [**IdentificationDocumentDocumentVerification**](IdentificationDocumentDocumentVerification.md) |  |  [optional] |



