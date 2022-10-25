# Persontjenesten hackathon

This is a working java sample, connecting to the external test environment for Persontjenesten and using HelseId
for authentication.

# Modules

## helseid-token-exchange

Retrieves an access token to persontjenesten from HelseId's test environment. The sample sends a request to
the token endpoint, configured with a valid client id, and scope for persontjenesten. The request is signed
with a private key formatted as a Json Web Key (jwk).

See [GetToken](helseid-token-exchange/src/main/java/GetToken.java) to run a working sample.

## persontjenesten-api-client

A java client for persontjenesten, generated using https://openapi-generator.tech/
The client is using okhttp as a http client and gson for serialization.

See [PersontjenestenApiClient](src/main/java/PersontjenestenApiClient.java) to run a working sample.