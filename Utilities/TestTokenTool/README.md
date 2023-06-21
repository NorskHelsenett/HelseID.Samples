# Konsollverktøy for å hente ut test-token fra HelseIDs Test-token-tjeneste (TTT)

Denne applikasjonen lar deg hente ut token som er signert med samme signeringsnøkkel som «ekte» token fra HelseIDs testmiljø.

For å ta applikasjonen i bruk må du

  * Generere et nøkkelpar (privatnøkkel og offentlig nøkkel)
  * Opprette en klientkonfigurasjon i [HelseID Selvbetjening TEST](https://selvbetjening.test.nhn.no/) som gir deg tilgang til TTT (se under).
    * Dette fordrer at du har brukertilgang i HelseID Selvbetjening TEST
  

### Generere et nøkkelpar

Bruk kommandoen `dotnet run createKeys` for å generere et nøkkelpar. Privatnøkkelen vil bli lagt i fila `jwk.json`, og den offentlige nøkkelen vil bli lagt i fila `jwk_pub.json`.
Denne siste fila må du senere laste opp til HelseID selvbetjening. 

### Framgangsmåte i [HelseID Selvbetjening TEST](https://selvbetjening.test.nhn.no/):

1. Velg «Ta i bruk HelseID»
2. Klikk på «Ny klientkonfigurasjon»
3. Under «Søk etter fagsystem», søk på «HelseID»
4. Velg «HelseID TTT-klient»
5. Huk av for «Test-token-tjeneste…» og skriv inn navnet på audience du vil bruke test-token mot
6. Klikk på «Gå videre»
7. Klikk på «Nøkkelpar (avansert)»
8. Klikk på «Laste opp en offentlig nøkkel»
9. Bruk «Last opp fil», og legg til fila `jwk_pub.json` fra forrige steg
10. Slå opp klientkonfigurasjonen du har laget, og kopier verdien for Klient-ID
11. Putt denne klient-ID-en inn i konfigurasjonsfila `config.json` under parameteret `Authentication:ClientId`

## Bruk av applikasjonen for å hente ut et token

Hvis du bruker kommandoen `dotnet run getToken`, vil applikasjonen skrive ut et token med standard-claim:

```
>dotnet run getToken
eyJhbGciOiJSUzI1NiIsImtpZCI6IkI0Q0FFNDUyQzhCNkE4OTNCNkE4NDBBQzhDODRGQjA3MEE0MjZFNDEiLCJ4NXQiOiJ0TXJrVXNpMnFKTzJxRUNzaklUN0J3cENia0UiLCJ0eXAiOiJKV1QifQ.eyJhdWQiOiJuaG46dGVzdDp0ZXN0LWFwaSIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzY2LyIsIm5iZiI6MTY4NzMzNTAxNywiaWF0IjoxNjg3MzM1MDE3LCJleHAiOjE2ODczMzU2MTcsImp0aSI6ImFhMGVlZWY1NjhhNzQyMzliMGFjZTYzNzRhZDY3NmRlIiwiaGVsc2VpZDovL2NsYWltcy9jbGllbnQvY2xpZW50X25hbWUiOiJBQ01FIENsaWVudCIsImNsaWVudF9hbXIiOiJwcml2YXRlX2tleV9qd3QiLCJzY29wZSI6WyJuaG46dGVzdC10b2tlbi10amVuZXN0ZS9zY29wZSJdLCJoZWxzZWlkOi8vY2xhaW1zL2NsaWVudC9hbXIiOiJyc2FfcHJpdmF0ZV9rZXkiLCJoZWxzZWlkOi8vY2xhaW1zL2NsaWVudC9jbGFpbXMvb3JnbnJfcGFyZW50IjoiOTk0NTk4NzU5IiwiY2xpZW50X2lkIjoiODgzNzM0NzUtY2ZiOS00MWQ2LTk4YzMtYTg4ZjA5MmI3OTRiIiwiaWRwIjoidGVzdGlkcC1vaWRjIiwiaGVsc2VpZDovL2NsYWltcy9pZGVudGl0eS9waWQiOiIwMzY3ODgyNjEyOSIsImhlbHNlaWQ6Ly9jbGFpbXMvaWRlbnRpdHkvcGlkX3BzZXVkb255bSI6IjhnYzFKTmRFVEhhK0EvYjdFU0pKWlZGdmxmTndWS05tdXhSbnlaYnJRdmc9IiwiaGVsc2VpZDovL2NsYWltcy9pZGVudGl0eS9zZWN1cml0eV9sZXZlbCI6IjQiLCJoZWxzZWlkOi8vY2xhaW1zL2lkZW50aXR5L2Fzc3VyYW5jZV9sZXZlbCI6ImhpZ2giLCJoZWxzZWlkOi8vY2xhaW1zL2lkZW50aXR5L25ldHdvcmsiOiJpbnRlcm5ldHQiLCJzdWIiOiJmK0hHSG1LSGpaQXd0RCtROEtmRGFSQy9TVjQzdFNzc1pVeTNKR3pua05nPSIsInNpZCI6IjNiMjA2ZWE0YjE5ZjRiODBhMDZiYTc4MWFiMjQ2ZDRhIiwibmFtZSI6IkdSQU5VTEVSVCBOSUxGSVNLIiwiaGVsc2VpZDovL2NsYWltcy9ocHIvaHByX251bWJlciI6IjEyMzQ1Njc4IiwiYW1yIjpbInB3ZCJdfQ.Y31Q-1Krjns5OCbc_1g2qLwYvEzaUiYIOOCWxDNxC0MeG0eIkwJAKWRZuNy9Ms4lm0hng-_v2mp4J5yGnHS503FuopIjYAYIeJQcPTAyRqJUpwYCz3YdgA6e1OmYHw8NbrbRoMN0A7RGVIMAixtKzdnnV3UwI38bR052KS46eKeuDWNbXDteh2fybYlv0q39Yymm1iclFIWKgnnsBRFzQ2wZPyt6R9jijGZySHSkRWTuw-2yG3t1WorH1qcSGi95WXsTs1_k6EA-J2LuLShkev8VoujqZ8fd6Y6Qta8BfODcN92nwrDzUXgUvDb-zjXC5ag7_fSJr8vecHF9f3Ua1A
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

For å få skrevet ut et token i JSON-format som inneholder kun obligatoriske claim:

`dotnet run getToken --prettyPrintToken --generalClaimsCreation ParameterValues --userClaimsCreation ParameterValues`

For å hente ut navn fra Persontjenesten og HPR-nummer fra HPR-registeret:

`dotnet run getToken --prettyPrintToken --generalClaimsCreation ParameterValues --userClaimsCreation ParameterValues --pid 16858399649 --getPersonFromPersontjenesten --getHprNumberFromHprregisteret`


For å få en liste over alle parametrene, kan du bruke kommandoen
`dotnet run getToken -- --help`

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
  --clientAmr                                <TEXT>             []
  The returned token will contain a 'client_amr' claim matching the injected value

  --helseidClientAmr                         <TEXT>             []
  The returned token will contain a 'helseid://claims/client/amr' claim matching the injected value

  --orgnrParent                              <TEXT>             []
  The returned token will contain a 'helseid://claims/client/claims/orgnr_parent' claim matching the injected value

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

  --pidPseudonym                             <TEXT>             []
  The returned token will contain a 'helseid://claims/identity/pid_pseudonym' claim matching the injected value. If setPidPseudonym is true, this option will be overridden.

  --name                                     <TEXT>             []
  The returned token will contain a 'name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.

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
### Parametre for tillitsrammeverket: brukes for å injisere verdier i en claim-struktur som brukes i forbindelse med dokumentdeling
```
  --legalEntityId                            <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --legalEntityName                          <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --pointOfCareId                            <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --pointOfCareName                          <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationCode            <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --practitionerAuthorizationText            <TEXT>             []
  Parameter for use in 'tillitsrammeverk' claims

  --careRelationshipDepartmentId             <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipDepartmentName           <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipHealthcareServiceCode    <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipHealthcareServiceText    <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipPurposeOfUseCode         <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipPurposeOfUseText         <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipPurposeOfUseDetailsCode  <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipPurposeOfUseDetailsText  <TEXT>             []
  Parameter for use in 'dokumentdeling' claims

  --careRelationshipTracingRefId             <TEXT>             []
  Parameter for use in 'dokumentdeling' claims
```
### Metaparametre: brukes for å beskrive bruken av enkelte av parametrene ovenfor
```
  --generalClaimsCreation                    <CLAIMGENERATION>  [DefaultWithParameterValues]
  Instructs how common claims are created
  Allowed values: None, Default, ParameterValues, DefaultWithParameterValues

  --userClaimsCreation                       <CLAIMGENERATION>  [DefaultWithParameterValues]
  Instructs how user claims are created
  Allowed values: None, Default, ParameterValues, DefaultWithParameterValues

  --createDokumentdelingClaims                                  [False]
  Create claims for dokumentdeling
```
