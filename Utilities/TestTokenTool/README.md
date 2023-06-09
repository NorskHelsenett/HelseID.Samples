# Konsollverktøy for å hente ut test-token fra HelseIDs Test-token-tjeneste (TTT)

Denne applikasjonen lar deg hente ut token som er signert med samme signeringsnøkkel som «ekte» token fra HelseIDs testmiljø.

For å ta applikasjonen i bruk må du

  * Generere et nøkkelpar (privatnøkkel og offentlig nøkkel)
  * Opprette en klientkonfigurasjon i [HelseID Selvbetjening TEST](https://selvbetjening.test.nhn.no/) som gir deg tilgang til TTT (se under).
    * Dette fordrer at du har brukertilgang i HelseID Selvbetjening TEST
  

### Generere et nøkkelpar

Bruk kommandoen `dotnet run create_jwk` for å generere et nøkkelpar. Privatnøkkelen vil bli lagt i fila `jwk.json`, og den offentlige nøkkelen vil bli lagt i fila `jwk_pub.json`.
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