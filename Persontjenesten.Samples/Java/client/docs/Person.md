

# Person


## Properties

| Name | Type | Description | Notes |
|------------ | ------------- | ------------- | -------------|
|**falseIdentity** | [**PersonFalseIdentity**](PersonFalseIdentity.md) |  |  [optional] |
|**norwegianIdentificationNumber** | [**List&lt;NorwegianIdentificationNumber&gt;**](NorwegianIdentificationNumber.md) | &lt;br&gt;FREG: Identifikasjonsnummer |  [optional] |
|**identityVerification** | [**List&lt;IdentityVerification&gt;**](IdentityVerification.md) | If empty, is the same as IdentityVerificationStatus.None&lt;br&gt;FREG: Identitetsgrunnlag |  [optional] |
|**residuaryEstateContactInformation** | [**List&lt;ResiduaryEstateContactInformation&gt;**](ResiduaryEstateContactInformation.md) | &lt;br&gt;FREG: KontaktinformasjonForDoedsbo |  [optional] |
|**identificationDocument** | [**List&lt;IdentificationDocument&gt;**](IdentificationDocument.md) | &lt;br&gt;FREG: Legitimasjonsdokument |  [optional] |
|**status** | [**List&lt;PersonStatus&gt;**](PersonStatus.md) | &lt;br&gt;FREG: Status |  [optional] |
|**immigrationAuthoritiesIdentificationNumber** | [**List&lt;PersonIdentification&gt;**](PersonIdentification.md) | &lt;br&gt;FREG: UtlendingsmyndighetenesIdentifikasjonsnummer |  [optional] |
|**foreignPersonIdentificationNumber** | [**List&lt;PersonIdentification&gt;**](PersonIdentification.md) | &lt;br&gt;FREG: UtenlandskPersonidentifikasjon |  [optional] |
|**sharedResidence** | [**List&lt;SharedResidence&gt;**](SharedResidence.md) | List may only contain 0 or 1 element  &lt;br&gt;FREG: DeltBosted |  [optional] |
|**gender** | [**List&lt;PersonGender&gt;**](PersonGender.md) | &lt;br&gt;FREG: Kjoenn |  [optional] |
|**birth** | [**List&lt;Birth&gt;**](Birth.md) | &lt;br&gt;FREG: Foedsel |  [optional] |
|**birthInNorway** | [**List&lt;BirthInNorway&gt;**](BirthInNorway.md) | &lt;br&gt;FREG: FoedselINorge |  [optional] |
|**familyRelation** | [**List&lt;FamilyRelation&gt;**](FamilyRelation.md) | &lt;br&gt;FREG: Familierelasjon |  [optional] |
|**maritalStatus** | [**List&lt;MaritalStatus&gt;**](MaritalStatus.md) | &lt;br&gt;FREG: Sivilstand |  [optional] |
|**death** | [**PersonDeath**](PersonDeath.md) |  |  [optional] |
|**name** | [**List&lt;PersonName&gt;**](PersonName.md) | &lt;br&gt;FREG: Navn |  [optional] |
|**addressProtection** | [**List&lt;AddressProtection&gt;**](AddressProtection.md) | &lt;br&gt;FREG: Adressebeskyttelse |  [optional] |
|**residentialAddress** | [**List&lt;ResidentialAddress&gt;**](ResidentialAddress.md) | &lt;br&gt;FREG: Bostedsadresse |  [optional] |
|**presentAddress** | [**List&lt;PresentAddress&gt;**](PresentAddress.md) | &lt;br&gt;FREG: Oppholdsadresse |  [optional] |
|**immigrationToNorway** | [**List&lt;ImmigrationToNorway&gt;**](ImmigrationToNorway.md) | &lt;br&gt;FREG: Innflytting |  [optional] |
|**emigrationFromNorway** | [**List&lt;EmigrationFromNorway&gt;**](EmigrationFromNorway.md) | &lt;br&gt;FREG: Utflytting |  [optional] |
|**useOfSamiLanguage** | [**List&lt;SamiLanguage&gt;**](SamiLanguage.md) | &lt;br&gt;FREG: BrukAvSamiskSpraak |  [optional] |
|**samiParliamentElectoralRegistryStatus** | [**List&lt;SamiParliamentElectoralRegistry&gt;**](SamiParliamentElectoralRegistry.md) | &lt;br&gt;FREG: ForholdTilSametingetsValgmanntall |  [optional] |
|**preferredContactAddress** | [**List&lt;PreferredContactAddress&gt;**](PreferredContactAddress.md) | Will be removed with deprecation of API v1.0  &lt;br&gt;FREG: PreferertKontaktadresse |  [optional] |
|**postalAddress** | [**List&lt;ContactAddress&gt;**](ContactAddress.md) | &lt;br&gt;FREG: Postadresse |  [optional] |
|**foreignPostalAddress** | [**List&lt;ForeignContactAddress&gt;**](ForeignContactAddress.md) | &lt;br&gt;FREG: PostadresseIUtlandet |  [optional] |
|**parentalResponsibility** | [**List&lt;ParentalResponsibility&gt;**](ParentalResponsibility.md) | &lt;br&gt;FREG: Foreldreansvar |  [optional] |
|**citizenship** | [**List&lt;Citizenship&gt;**](Citizenship.md) | &lt;br&gt;FREG: Statsborgerskap |  [optional] |
|**citizenshipRetention** | [**List&lt;NorwegianCitizenshipRetention&gt;**](NorwegianCitizenshipRetention.md) | &lt;br&gt;FREG: Bibehold |  [optional] |
|**residencePermit** | [**List&lt;ResidencePermit&gt;**](ResidencePermit.md) | &lt;br&gt;FREG: Opphold |  [optional] |
|**stayOnSvalbard** | [**List&lt;StayOnSvalbard&gt;**](StayOnSvalbard.md) | &lt;br&gt;FREG: OppholdPaaSvalbard |  [optional] |
|**guardianshipOrFuturePowerOfAttorney** | [**List&lt;GuardianshipOrFuturePowerOfAttorney&gt;**](GuardianshipOrFuturePowerOfAttorney.md) | &lt;br&gt;FREG: VergemaalEllerFremtidsfullmakt |  [optional] |
|**deprivedLegalAuthority** | [**List&lt;DeprivedLegalAuthority&gt;**](DeprivedLegalAuthority.md) | &lt;br&gt;FREG: FratattRettsligHandleevne  |  [optional] |
|**legalAuthority** | [**List&lt;LegalAuthority&gt;**](LegalAuthority.md) | &lt;br&gt;FREG: RettsligHandleevne  |  [optional] |
|**commonContactRegisterInformation** | [**PersonCommonContactRegisterInformation**](PersonCommonContactRegisterInformation.md) |  |  [optional] |
|**id** | **String** |  |  [optional] |
|**sequenceNumber** | **Long** |  |  [optional] |



