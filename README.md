# HelseID Samples

HelseID is a national authentication service for the health sector in Norway. These samples are targeted at technical personel such as application architects and developers. 

A client needs be registered in the HelseID Server Configuration before running the samples.

## Overview of Samples
1. [Client Authentication](#ClientAuthentication)
2. [Sample API](#SampleAPI)
3. [API Access](#APIAccess)
    1. [API Access with new access token](#APIAccessNewToken)
    2. [API Access with resource indicators](#APIAccessResourceIndicators)
4. [Generate a JSON Web Key (JWK)](#RsaJwk)
5. [Client Credentials Grant](#ClientCredentials)
    1. [Client Credentials with Child Organization](#ClientCredentials.WithChildOrg)
    2. [Client Credentials with JWK](#ClientCredentials.Jwk)
    3. [Client Credentials with Enterprise Certificate](#ClientCredentials.EnterpriseCertificate)
6. [Refresh Token](#RefreshToken)
7. [Token Exchange](#TokenExchange)
8. [Request Objects](#RequestObjects)
10. [BFF](#BFF)
##

### <a name="ClientAuthentication"></a> Client Authentication

The sample demonstrates how a client can be authenticated through HelseID by using a Json Web Key pair. See [HelseId.ClientAuthentication](https://github.com/ingvildaadde/Samples/tree/main/ClientAuthentication) for a more detailed explanation of the authentication process.

### <a name="SampleAPI"></a> Sample API

Some of the HelseID samples demonstrate how a client can access an API. Thus, a sample API have been created such that other HelseID samples can access it. The API requires authentication before the client is granted access. The API should be run locally before trying to connect through another HelseID sample. See [HelseId.SampleAPI](https://github.com/ingvildaadde/Samples/tree/main/SampleAPI) for a more detailed explanation.  

### <a name="APIAccess"></a> API Access

The sample demonstrate how a client can be authenticated through HelseID and get access to the restricted [Sample API](https://github.com/ingvildaadde/Samples/tree/main/SampleAPI) with one access token. See [HelseId.APIAccess](https://github.com/ingvildaadde/Samples/tree/main/ClientAuthenticationWithAPIAccess) for a more detailed explanation.

#### <a name="APIAccessNewToken"></a> API Access with new access token

In this sample one can also access the API with a new access token. This is done to make sure that the token has as few scopes/accesses as possible. See [HelseId.APIAccessNewToken](https://github.com/ingvildaadde/Samples/tree/main/ClientAuthenticationAPIAccessNewToken) for a more detailed explanation.

#### <a name="APIAccessResourceIndicators"></a> API Access with resource indicators

The sample demonstrate how resource indicators are used to download multiple access tokens without performing multiple calls to the authorization endpoint. Each access token can then be used to call a specified API. See [HelseId.Samples.ResourceIndicatorsDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.Samples.ResourceIndicatorsDemo) for more information.


### <a name="RsaJwk"></a> Generate a JSON Web Key (JWK)

A command line program to generate a key pair as a JSON Web Key (JWK). The sample creates two files where the first file contains the whole key pair (including the private key), while the second file only contains the public key. See [HelseId.RsaJwk](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.RsaJwk) for a more detailed explanation.

### <a name="ClientCredentials"></a> Client Credentials Grant

The client credentials grant type is commonly used for server-to-server interactions that must run in the background, without immediate interaction with a user. The sample demonstrates how to generate a signed JSON Web Token (JWT) that is used to request access to a web-server. An access token is returned and written to the console if the request was approved. See [HelseId.Samples.ClientCredentials](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials).

#### <a name="ClientCredentials.WithChildOrg"></a> Client Credentials with Child Organization

A modification of the [HelseId.Samples.ClientCredentials](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials) sample, where a child organization is added as well. See [HelseId.Samples.ClientCredentialsWithUnderenhet](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.Samples.ClientCredentialsWithUnderenhet).

#### <a name="ClientCredentials.Jwk"></a> Client Credentials with JWK

The sample uses JWK as secret and is a modification of the [HelseId.Samples.ClientCredentials](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials) sample. See [HelseId.Samples.ClientCredentials.Jwk](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials.Jwk).

#### <a name="ClientCredentials.EnterpriseCertificate"></a> Client Credentials with Enterprise Certificate

The sample uses Enterprise Certificate as secret and is a modification of the [HelseId.Samples.ClientCredentials](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials) sample. See [HelseId.Samples.EnterpriseCertificate](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.EnterpriseCertificate).

### <a name="RefreshToken"></a> Refresh Token

Simple demonstration of refresh tokens. See [HelseId.Samples.RefreshTokenDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.RefreshTokenDemo).

### <a name="TokenExchange"></a> Token Exchange

Simple demonstration of token exchange. See [HelseId.Samples.TokenExchangeDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.TokenExchangeDemo).

### <a name="RequestObjects"></a> Request Objects

The sample demonstrates how to use request objects for client authentication against HelseID. See [HelseId.Samples.RequestObjectsDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.RequestObjectsDemo).

### <a name="BFF"></a> BFF

...

##
More info on https://nhn.no/helseid/ (Norwegian) and https://dokumentasjon.helseid.no/
