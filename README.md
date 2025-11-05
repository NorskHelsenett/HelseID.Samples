#### A disclaimer

*This code has been written for the purpose of creating examples for the use of HelseID. We have tried to emulate real-life scenarios of authentication flows. If you find that any part of the code is illegible or hard to understand, we appreciate your feedback. Any input or advice that leads to improve the example code will be received with gratitude!*

# HelseID samples

HelseID is a national authentication service for the healthcare sector in Norway. 
These code samples are targeted at technical personnel such as application architects and developers.  

If you experience trouble while checking out this repository because of long paths, update your git config by running:  

`git config --global core.longpaths true`

## Prerequisite knowledge
As HelseID is based on the [OAuth 2.0 framework](https://oauth.net/2/), both the code and the usage scenarios are strongly connected to this authorization protocol. If you have not had any previous experience with this framework, we strongly advise you to check out online resources on this topic. Good starting points can be found at [OAuth 2.0 Simplified](https://www.oauth.com/).

### General information
The examples connect to the HelseID **test** environment. For each example, there exists a corresponding client in HelseID. The sample code will be able to use 1) a client ID in connection with 2) a self-issued client assertion signed with a [JWK-encoded](https://www.rfc-editor.org/rfc/rfc7517) private key in order to authenticate against HelseID.

For all samples that requires user authentication you can use our "TestIDP" which provides a way to simulate user login without the need for actual test users.

#### Important notice

Please be advised that the sample code does not in any way represent working code for a production environment. In particular, special care **MUST** be taken in order to secure the private keys that the code uses to authenticate against HelseID.

## Basic samples

These samples use either machine-to-machine or user logon authentication methods.

### Sample API (C# / ASP.NET Core)

The code can be found in the `SampleAPI` folder.

Most of the HelseID samples demonstrate how a client can access an API. Therefore, a sample API has been created such that other HelseID samples can access it. The API requires authentication before the client is granted access. The API should be run locally before trying to connect through another HelseID sample.

See [SampleApi/README.md](SampleApi/README.md) for more information.

### Sample API (Java / Spring)

The code can be found in the `SpringSecurity` folder.

Most of the HelseID samples demonstrate how a client can access an API. Therefore, a sample API has been created such that other HelseID samples can access it. The API requires authentication before the client is granted access. The API should be run locally before trying to connect through another HelseID sample.

This version of the API is set up with the use of [Spring Security](https://docs.spring.io/spring-security/reference/index.html) for authorization. The code is also set up to use Nimbus OAuth SDK for the usage of [DPoP](https://connect2id.com/products/nimbus-oauth-openid-connect-sdk/examples/oauth/dpop).

### Grant types

HelseID supports the use of several grants, that is, the resource owner's (user or enterprise) authorization used by the client to obtain an access token. Code samples are provided for different scenarios, and each will use at least one grant type. The following grants are provided:

All these samples are configured to request a DPoP (Demonstrating Proof-of-Possession)-bound token.

### Machine-to-machine sample (C# / .NET)

The code can be found in the `ClientCredentials` folder.

This sample use the [Client Credentials Grant](https://www.rfc-editor.org/rfc/rfc6749#section-4.4) in order to request an access token from HelseID. Its intended usage is when you need to connect to an API resource without logging on a user.

When using this sample, you will also need to start the [sample API](./SampleApi/README.md).

See [ClientCredentials/README.md](ClientCredentials/README.md) for more information.

### User authentication/API access sample (C# / ASP.NET Core)

The code can be found in the `SampleApi` folder.

This sample uses the [Authorization Code Grant](https://www.rfc-editor.org/rfc/rfc6749#section-4.1) with the [OpenID connect](https://openid.net/connect/) identity layer on top of OAuth 2.0 
to demonstrate how a user can authenticated through HelseID. 

You can use this sample for user login only, or, if you want to access an API, you will also need to start the [sample API](./SampleApi/README.md).

This sample also exhibits the use of the [Refresh Token Grant](https://www.rfc-editor.org/rfc/rfc6749#section-1.5).


See [APIAccess/README.md](ApiAccess/README.md) for more information.

### Token exchange sample (C# / .NET)

The code can be found in the `SampleApiForTokenExchange` folder.

This sample uses the [Token Exchange Grant](https://www.rfc-editor.org/rfc/rfc8693). It sets up a simple demonstration of token exchange, using a sample API that uses HelseID to exchange an access token for another. This exchanged access token is then used with another API.

If you wish to use this sample, you will need to start both
* The API access sample in "token exchange mode"; see [ApiAccess/README.md](./ApiAccess/README.md) for details, and
* The [sample API](./SampleApi/README.md)

 See [SampleApiForTokenExchange/README.md](./SampleApiForTokenExchange/README.md) for more information.

## Native clients (C# / .NET)

For use in "native clients", i.e. applications that does not run in a web client, we have created a set of simple examples for the use of a system browser for login against HelseID. These examples can be found in the `NativeClients`folder.

### <a name="SimpleNativeClientWithDPoP"></a> Simple API access with API login

The sample demonstrates a simple user login. See [NativeClients/SimpleNativeClientWithDPoP/README.md](NativeClients/SimpleNativeClientWithUserLoginAndApiCall/README.md) for more information.

### <a name="APIAccessResourceIndicators"></a> Simple API access with resource indicators

The sample demonstrates how resource indicators are used to download multiple access tokens without performing multiple calls to the authorization endpoint. Each access token can then be used to call a specified API. See [NativeClients/SimpleResourceIndicatorsDemo/README.md](NativeClients/SimpleResourceIndicatorsDemo/README.md) for more information.

### <a name="RequestObjects"></a> Request objects

The sample demonstrates how to use request objects for client authentication against HelseID. See [NativeClients/SimpleRequestObjectsDemo](NativeClients/SimpleRequestObjectsDemo/README.md).

## Advanced samples

We've created a set of samples for more advanced cases.

### <a name="BFF"></a> Backend for frontend (BFF)

See [HelseId.Core.BFF.Sample](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Core.BFF.Sample).

### <a name="PersontjenestenSamples"></a> Persontjenesten samples

Running samples for Persontjenesten API clients that exchange valid tokens with HelseID.

Implementations:
1. [.NET](Persontjenesten.Samples/.NET/PersontjenestenDotNetDemo/README.md)
2. [Java](Persontjenesten.Samples/Java/README.md)


## <a name="Tooling"></a> Tooling

We have included some tools to enable a smoother developing experience


### <a name="RsaJwk"></a> Generate a JSON web key (JWK) (C# / .NET)

A command line program to generate a key pair as a JSON Web Key (JWK). The sample creates two files where the first file contains the whole key pair (including the private key), while the second file only contains the public key. See [HelseId.RsaJwk/README.md](HelseId.RsaJwk/README.md) for a more detailed explanation.

## Other resources

An excellent introductory guide to OAuth 2 can be found [here](https://www.oauth.com/)

You can find the OAuth 2 specs here: [https://oauth.net/specs/](https://oauth.net/specs/)

You can find the OpenID Connect specs here: [https://openid.net/developers/specs/](https://openid.net/developers/specs/)

More info can be found at https://nhn.no/helseid/ (in Norwegian) and https://dokumentasjon.helseid.no/.
