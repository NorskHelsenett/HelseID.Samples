# Konsollverktøy for å hente ut test-token fra HelseIDs Test-token-tjeneste (TTT)

Denne applikasjonen lar deg bruke HelseIDs *test-tokentjeneste* for å hente ut token som er signert med samme signeringsnøkkel som «ekte» token fra HelseIDs testmiljø.

Test-token-tjenesten er satt opp som et REST-API, og finnes på endepunktet [https://helseid-ttt.test.nhn.no/create-test-token](https://helseid-ttt.test.nhn.no/create-test-token). En enkel API-spesifikasjon finnes på adressen [https://helseid-ttt.test.nhn.no/swagger/index.html#/TokenService](https://helseid-ttt.test.nhn.no/swagger/index.html#/TokenService).

Denne applikasjonen viser hvordan Test-token-tjenesten kan konsumeres, og kan eksempelvis også brukes i forbindelse med skripting av uttrekk av test-token.

For å ta applikasjonen i bruk, må du 

* Få en API-nøkkel fra [HelseID Selvbetjening TEST](https://selvbetjening.test.nhn.no/) (bruk knappen «Integrasjonstesting»).
* Legge denne nøkkelen i konfigurasjonsfila `config.json` under parameteret `Authentication:ApiKey`

## Bruk av applikasjonen for å hente ut et token

Hvis du bruker kommandoen `dotnet run getToken`, vil applikasjonen skrive ut et token med standard-claim:

```
>dotnet run getToken
eyJhbGciOiJSUzI1NiIsImtpZCI6IjVGN0IzOEJBODA3NkZERThDRDYwMDgwRUFFNkVBOEY0NEY5QU...
```

Applikasjonen har forskjellige parametre som kan brukes for å justere hvordan et generert token skal se ut. Disse kan vagt kategoriseres som
* Parametre for å beskrive utput
* Parametre for å lage ugyldige claims
* Parametre for å justere på headeren i tokenet
* Generelle parametre: for å justere standard-claim som beskriver aspekter med en (fiktiv) klient
* Brukerparametre: for å justere claim som beskriver en (fiktiv) bruker
* Parametre for tillitsrammeverket: brukes for å injisere verdier i en claim-struktur som brukes i forbindelse med dokumentdeling
* Metaparametre: brukes for å beskrive bruken av enkelte av parametrene ovenfor

Et par eksempel:

For å liste ut alle kommandoene som er tilgjengelige:

`dotnet run`

For å liste ut alle parametrene som kan brukes med `getToken`-kommandoen:

`dotnet run getToken -- --help`


For å få skrevet ut et token i JSON-format som inneholder kun obligatoriske claim (altså ikke claim som beskriver en klient og en bruker):

`dotnet run getToken --prettyPrintToken --withoutDefaultClientClaims --withoutDefaultUserClaims`

For å hente ut navn fra Persontjenesten og HPR-nummer fra HPR-registeret:

`dotnet run getToken --prettyPrintToken --withoutDefaultClientClaims --withoutDefaultUserClaims --pid 16858399649 --getPersonFromPersontjenesten --getHprNumberFromHprregisteret`


For å få en liste over alle parametrene, kan du bruke kommandoen
`dotnet run getToken -- --help`


For å kalle SampleAPI-applikasjonen (i ../../SampleApi-katalogen) med DPoP:
`dotnet run getToken --createDPoPTokenWithDPoPProof --htuClaimValue https://localhost:5081/machine-clients/dpop-greetings --htmClaimValue GET --callApi
`

### Parametre for å beskrive utput

```
-p | --printToken                          <BOOLEAN>          [True]
  The returned token is printed in JWT format on screen
  Allowed values: true, false

  --prettyPrintToken                                            [False]
  The returned token is printed in JSON format on screen

  -s | --saveTokenToFile                                        [False]
  The returned token is saved to a file named 'token.jwt'
```
### Metaparametre: brukes for å beskrive bruken av enkelte av parametrene nedenfor
```
  
  --withoutDefaultClientClaims              <BOOLEAN>          [False]
  No default client claims are created
  
  --withoutDefaultUserClaims                 <BOOLEAN>          [False]
  No default user claims are created
  
  --clientClaimsCreation                    <CLAIMGENERATION>  [DefaultWithParameterValues]
  Instructs how client claims are created
  Allowed values: None, Default, ParameterValues, DefaultWithParameterValues

  --userClaimsCreation                       <CLAIMGENERATION>  [DefaultWithParameterValues]
  Instructs how user claims are created
  Allowed values: None, Default, ParameterValues, DefaultWithParameterValues

  --createTillitsrammeverkClaims                               [False]
  Create claims for tillitsrammeverk
  
```
### Parametre for å lage ugyldige claims
```
  --signJwtWithInvalidSigningKey                                [False]
  The returned token will be signed with an invalid signing key
  
  --setInvalidIssuer                                            [False]
  The returned token will contain an invalid 'iss' claim

  --setInvalidAudience                                          [False]
  The returned token will contain an invalid 'aud' claim

  --setExpirationTimeAsExpired                                  [False]
  The returned token will contain an expired 'nbf', 'iat', and 'exp' claims

  --expirationTimeInSeconds                  <NUMBER>           [600]
  The returned token will contain an 'exp' claim matching the set expiration time
```
### Parametre for å justere på headeren i tokenet
```
  --typ                                      <TEXT>             []
  The returned token will contain a 'typ' header matching the injected value. Accepted values are 'jwt' and 'at+jwt'
```
### Generelle parametre: for å justere standard-claim som beskriver aspekter med en (fiktiv) klient
```
  --audience                                 <TEXT>             []
  The returned token will contain an audience claim that matches the injected value. If this is not set, the audience will be set as 'nhn:some:api'.

  --issuerEnvironment
  The returned token will contain an issuer claim that matches the injected value
  Allowed values: Test, IntTest, Development
    
  --clientAmr                                <TEXT>             []
  The returned token will contain a 'client_amr' claim matching the injected value

  --helseidClientAmr                         <TEXT>             []
  The returned token will contain a 'helseid://claims/client/amr' claim matching the injected value

  --orgnrParent                              <TEXT>             []
  The returned token will contain a 'helseid://claims/client/claims/orgnr_parent' claim matching the injected value

  --orgnrChild                               <TEXT>             []
  The returned token will contain a 'helseid://claims/client/claims/orgnr_child' claim matching the injected value

  --orgnrSupplier                            <TEXT>             []
  The returned token will contain a 'helseid://claims/client/claims/orgnr_supplier' claim matching the injected value

  --clientTenancy
  The returned token will contain a 'helseid://claims/client/claims/client_tenancy' claim matching the injected value if 'clientTenancyType' is set

  --clientTenancyType
  The returned token will contain a 'helseid://claims/client/claims/client_tenancy' claim with either 'none', 'single-tenant', or 'multi-tenant', matching the injected value if 'clientTenancy' is set
  
  --sfmJournalId                             <GUID>
  The returned token will contain a 'nhn:sfm:journal_id' claim matching the injected value

  --clientId                                 <GUID>
  The returned token will contain a 'client_id' claim matching the injected value

  --clientName                               <TEXT>             []
  The returned token will contain a 'helseid://claims/client/client_name' claim matching the injected value

  --scope                                    <TEXT>             []
  The returned token will contain a 'scope' claim matching the injected value. Use quotes and spaces to insert several scopes.

```
### Brukerparametre: for å justere claim som beskriver en (fiktiv) bruker
```

  --amr                                      <TEXT>             []
  The returned token will contain an 'amr' claim matching the injected value

  --identityProvider                         <TEXT>             []
  The returned token will contain an 'idp' claim matching the injected value

  --assuranceLevel                           <TEXT>             []
  The returned token will contain a 'helseid://claims/identity/assurance_level' claim matching the injected value

  --securityLevel                            <TEXT>             []
  The returned token will contain a 'helseid://claims/identity/security_level' claim matching the injected value

  --setPidPseudonym                                             [False]
  The returned token will contain a claim with a pseudonymized pid value. Requires that the 'pid' parameter is set.

  --pid                                      <TEXT>             []
  The returned token will contain a 'pid' claim matching the injected value

  --getPersonFromPersontjenesten                                [False]
  If this value is set, the PID value will be used to extract person information from Persontjenesten. Requires that the 'pid' parameter is set.

  --onlySetNameForPerson                                        [False]
  If this value and getPersonFromPersontjenesten are set, only the 'name' claim will be issued, not the claims 'given_name', 'middle_name', or 'family_name'.    

  --pidPseudonym                             <TEXT>             []
  The returned token will contain a 'helseid://claims/identity/pid_pseudonym' claim matching the injected value. If setPidPseudonym is true, this option will be overridden.

  --name                                     <TEXT>             []
  The returned token will contain a 'name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.

  --given_name                               <TEXT>             []
  The returned token will contain a 'given_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.

  --middle_name                              <TEXT>             []
  The returned token will contain a 'middle_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.

  --family_name                              <TEXT>             []
  The returned token will contain a 'family_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.

  --getHprNumberFromHprregisteret                               [False]
  If this value is set, the PID value will be used to extract person information from HPR-registeret.

  --hprNumber                                <TEXT>             []
  The returned token will contain a 'helseid://claims/hpr/hpr_number' claim matching the injected value. Requires that the 'getHprNumberFromHprregisteret' parameter is not set.

  --network                                  <TEXT>             []
  The returned token will contain a 'network' claim matching the injected value

  --sid                                      <TEXT>             []
  The returned token will contain a 'sid' claim matching the injected value

  --sub                                      <TEXT>             []
  The returned token will contain a 'sub' claim matching the injected value
```
### Parametre for tillitsrammeverket: brukes for å injisere verdier i en claim-struktur som brukes i forbindelse med tillitsrammeverket (dokumentdeling)
```
  --dontSetPractitionerHprNr
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetPractitionerAuthorization
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetPractitionerDepartment
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetCareRelationshipHealthcareService
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetCareRelationshipPurposeOfUseDetails
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetPatientsPointOfCare
  Parameter for use in 'tillitsrammeverk' claims

  --dontSetPatientsDepartment
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerIdentifierSystem                 <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerIdentifierAuthority              <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerHprNrSystem                      <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerHprNrAuthority                   <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationCode                <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationText                <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationSystem              <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationAssigner            <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerLegalEntityId                    <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerLegalEntityName                  <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerLegalEntitySystem                <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerLegalEntityAuthority             <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerPointOfCareId                    <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerPointOfCareName                  <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerPointOfCareSystem                <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerPointOfCareAuthority             <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerDepartmentId                     <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerDepartmentName                   <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerDepartmentSystem                 <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerDepartmentAuthority              <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipHealthcareServiceCode        <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipHealthcareServiceText        <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipHealthcareSystem             <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipHealthcareAssigner           <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseCode             <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseText             <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseSystem           <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseAssigner         <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseDetailsCode      <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseDetailsText      <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseDetailsSystem    <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipPurposeOfUseDetailsAssigner  <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipDecisionRefId                <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipDecisionRefUserSelected                                    [True]
  Parameter for use in 'tillitsrammeverk' claims

  --patientsPointOfCareId                        <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsPointOfCareName                      <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsPointOfCareSystem                    <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsPointOfCareAuthority                 <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsDepartmentId                         <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsDepartmentName                       <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsDepartmentSystem                     <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims

  --patientsDepartmentAuthority                  <TEXT>                        []
  Parameter for use in 'tillitsrammeverk' claims
```
### Brukerparametre: for å justere claim og bevis for DPoP
```
  --createDPoPTokenWithDPoPProof
  This will create a DPoP proof and a corresponding 'cnf' claim in the returned token

  --htuClaimValue                          <TEXT>             []
  This parameter will be set as the 'htu' claim value in the DPoP proof. If 'createDPoPTokenWithDPoPProof' is set, and 'dontSetHtuClaimValue' is not set, this parameter must have a value.
    
  --htmClaimValue                          <TEXT>             []
  This parameter will be set as the 'htm' claim value for the DPoP proof. If 'createDPoPTokenWithDPoPProof' is set, and 'dontSetHtmClaimValue' is not set, this parameter must have a value. Accepted values are methods as described in RFC9110 (https://www.rfc-editor.org/rfc/rfc9110#name-method-definitions)
  
  --privateKeyForProofCreation             <TEXT>             []
  If set with a valid jwk, the DPoP proof will be signed with this key"

  --invalidDPoPProof                         <INVALIDDPOPPROOFPARAMETERS>
  If this value is set, an invalid DPoP proof will we returned
  Allowed values: None, DontSetHtuClaimValue, DontSetHtmClaimValue, SetIatValueInThePast, SetIatValueInTheFuture, DontSetAthClaimValue, DontSetAlgHeader, DontSetJwkHeader, DontSetJtiClaim, SetAlgHeaderToASymmetricAlgorithm, SetPrivateKeyInJwkHeader, SetInvalidTypHeaderValue, SetAnInvalidSignature, SetAnInvalidJktValueInToken
  
  --invalidDPoPProof
  When calling an API with a DPoP proof, the API is called twice with the same DPoP proof. This should trigger an error on the second call.
```
### Parametre for å lage et 'cnf'-claim (DPoP) i tokenet

Disse brukes hvis du vil lage DPoP-beviset selv, og derfor trenger et 'cnf'-claim i tokenet. 
```

  --cnfJkt                                   <TEXT>                        []
  The returned token will contain a 'cnf' claim matching the injected value

  --cnfPublicKey                             <TEXT>                        []
  The returned token will contain a 'cnf' claim with a hash matching the injected value


```

### Brukerparametre: for å legge til et API-spesifikt claim
```
  --apiSpecificClaimType
  This will set the claim type for the API specific claim. Both type and value must be set in order to get an API specific claim.

  --apiSpecificClaimValue
  This will set the claim value for the API specific claim. Both type and value must be set in order to get an API specific claim.
 ```

