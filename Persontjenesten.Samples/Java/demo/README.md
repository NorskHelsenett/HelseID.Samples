# Persontjenesten hackathon

This is a working java sample, connecting to the external test environment for Persontjenesten and using HelseIDs
for authentication.

## HelseIDs test environment and private keys

This repository contains a private key which can be used to sign token requests in HelseIDs test environment,
in order for the sample to work out of the box. In the test environment for HelseID, it's a deliberate strategy
that private keys are considered harmless to keep in source control.

**Never keep private keys in source control in a production environment**.

In a real world application, the private key should be kept secret and loaded into the application by a secret provider
on demand.

# Modules

## helseid-token-exchange

Retrieves an access token to persontjenesten from HelseID's test environment. The sample sends a request to
the token endpoint, configured with a valid client id, and scope for persontjenesten. The request is signed
with a private key formatted as a Json Web Key (jwk).

See [GetToken](helseid-token-exchange/src/main/java/GetToken.java) to run a working sample.

## persontjenesten-api-client

A java client for persontjenesten, generated using https://openapi-generator.tech/
The client is using okhttp as a http client and gson for serialization.

See [PersontjenestenApiClient](api-client/src/main/java/PersontjenestenApiClient.java) to run a working sample.