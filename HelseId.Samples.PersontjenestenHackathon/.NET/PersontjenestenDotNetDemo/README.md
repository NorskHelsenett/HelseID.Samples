# Persontjenesten hackathon

This is a working .NET sample, connecting to the external test environment for Persontjenesten and using HelseId
for authentication.

## HelseId test environment and private keys

This repository contains a private key which can be used to sign token requests in HelseIds test environment,
in order for the sample to work out of the box. In the test environment for HelseId, it's a deliberate strategy
that private keys are considered harmless to keep in source control.

**Never keep private keys in soruce control in a production environment**.

In a real world application, the private key should be kept secret and loaded into the application by a secret provider
on demand.

# Modules

## helseid-token-exchange

Retrieves an access token to persontjenesten from HelseId's test environment. The sample sends a request to
the token endpoint, configured with a valid client id, and scope for persontjenesten. The request is signed
with a private key formatted as a Json Web Key (jwk).

See [GetToken](helseid-token-exchange/src/main/java/GetToken.java) to run a working sample.

## PersontjenestenDotNetDemo

A .NET client for persontjenesten, generated using https://openapi-generator.tech/docs/installation/
This client used csharp-netcore under generators list (https://openapi-generator.tech/docs/generators)