# HelseID Samples

HelseID is a national authentication service for the health sector in Norway. 
These samples are targeted at technical personel such as application architects and developers.  

If you have trouble checking out this repository because of long paths, update your git config by running:  
`git config --global core.longpaths true`

## Overview of Samples
* [Client Authentication](#ClientAuthentication)
* [API Access](#APIAccess)
    * [API Access with resource indicators](#APIAccessResourceIndicators)
* [Client Credentials Grant](#ClientCredentials)
* [Refresh Token](#RefreshToken)
* [Token Exchange](#TokenExchange)
* [Request Objects](#RequestObjects)
* [Backend for Frontend (BFF)](#BFF)
* [Persontjenesten Samples](#PersontjenestenSamples)
##
###Other resources

* [Sample API](#SampleAPI)
* [Generate a JSON Web Key (JWK)](#RsaJwk)

### <a name="ClientAuthentication"></a> Client Authentication

This sample can be found in the `ClientAuthentication` folder.

The sample demonstrates how a client can be authenticated through HelseID by using a Json Web Key (JWK) pair. See [ClientAuthentication/README.md](ClientAuthentication/README.md) for a more detailed explanation of the authentication process.

### <a name="APIAccess"></a> API Access

The sample demonstrate how a client can be authenticated through HelseID and get access to the restricted [Sample API/README.md](HelseId.SampleAPI/README.md) with an access token. See [APIAccess/README.md](APIAccess/README.md) for a more detailed explanation.


#### <a name="APIAccessResourceIndicators"></a> API Access with resource indicators

The sample demonstrate how resource indicators are used to download multiple access tokens without performing multiple calls to the authorization endpoint. Each access token can then be used to call a specified API. See [HelseId.Samples.ResourceIndicatorsDemo/README.md](ResourceIndicatorsDemo/README.md) for more information.

### <a name="ClientCredentials"></a> Client Credentials Grant

Simple demonstration of client credentials grant. See [HelseId.Samples.ClientCredentials/README.md](HelseId.Samples.ClientCredentials/README.md) for more information.

#### <a name="ClientCredentials.WithChildOrg"></a> Client Credentials with Child Organization

A modification of the [HelseId.Samples.ClientCredentials](HelseId.Samples.ClientCredentials/README.md) sample, where a child organization is added as well. See [HelseId.Samples.ClientCredentialsWithUnderenhet/README.md](HelseId.Samples.ClientCredentialsWithUnderenhet/README.md).

### <a name="RefreshToken"></a> Refresh Token

Simple demonstration of refresh tokens. See [HelseId.Samples.RefreshTokenDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.RefreshTokenDemo).

### <a name="TokenExchange"></a> Token Exchange

Simple demonstration of token exchange. See [HelseId.Samples.TokenExchangeDemo](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.TokenExchangeDemo).

### <a name="RequestObjects"></a> Request Objects

The sample demonstrates how to use request objects for client authentication against HelseID. See [HelseId.Samples.RequestObjectsDemo](HelseId.Samples.RequestObjectsDemo/README.md).

### <a name="BFF"></a> Backend for Frontend (BFF)

See [HelseId.Core.BFF.Sample](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Core.BFF.Sample).

### <a name="PersontjenestenSamples"></a> Persontjenesten Samples

Running samples for Persontjenesten API clients that exchange valid tokens with HelseID.

Implementations:
1. [.NET](Persontjenesten.Samples/.NET/PersontjenestenDotNetDemo/README.md)
2. [Java](Persontjenesten.Samples/Java/demo/README.md)

### <a name="SampleAPI"></a> Sample API

The code can be found in the `SampleAPI` folder.

Some of the HelseID samples demonstrate how a client can access an API. Therefore, a sample API has been created such that other HelseID samples can access it. The API requires authentication before the client is granted access. The API should be run locally before trying to connect through another HelseID sample. See [SampleAPI/README.md](SampleAPI/README.md) for a more detailed explanation.  

### <a name="RsaJwk"></a> Generate a JSON Web Key (JWK)

A command line program to generate a key pair as a JSON Web Key (JWK). The sample creates two files where the first file contains the whole key pair (including the private key), while the second file only contains the public key. See [HelseId.RsaJwk/README.md](HelseId.RsaJwk/README.md) for a more detailed explanation.

##
More info on https://nhn.no/helseid/ (Norwegian) and https://dokumentasjon.helseid.no/
