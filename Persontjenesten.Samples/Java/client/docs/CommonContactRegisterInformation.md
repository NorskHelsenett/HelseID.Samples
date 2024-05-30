

# CommonContactRegisterInformation

Contact Register Information for a person in the Person API  Data is from the Contact and Reservation Register (KRR)

## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**reservation** | **Reservation** | &lt;br&gt;KRR: reservasjon              Reservation given by a person, used in accordance to eForvaltningsforskriften § 15 a. |  [optional] |
|**status** | **Status** | &lt;br&gt;KRR: status              The status of a person |  [optional] |
|**notificationStatus** | **NotificationStatus** | &lt;br&gt;KRR: varslingsstatus              Describes if a person can be notified or not |  [optional] |
|**contactInformation** | [**CommonContactRegisterInformationContactInformation**](CommonContactRegisterInformationContactInformation.md) |  |  [optional] |
|**digitalPost** | [**CommonContactRegisterInformationDigitalPost**](CommonContactRegisterInformationDigitalPost.md) |  |  [optional] |
|**certificate** | **String** | &lt;br&gt;KRR: sertifikat              Person certificate for digital post |  [optional] |
|**language** | **String** | &lt;br&gt;KRR: spraak              Persons preferred language for communication with the public |  [optional] |
|**languageLastUpdated** | **String** | &lt;br&gt;KRR: spraak_oppdatert              Last time language choice was updated |  [optional] |



