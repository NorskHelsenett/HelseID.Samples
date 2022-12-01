## PersontjenestenDotNetDemo

A .NET client for persontjenesten, generated using https://openapi-generator.tech/docs/installation/
This client the csharp-netcore generator. For a full list of generators, see
[https://openapi-generator.tech/docs/generators](https://openapi-generator.tech/docs/generators)

This is a working .NET sample, connecting to the external test environment for Persontjenesten and using HelseId
for authentication.

## HelseID test environment and private keys

This repository contains a private key which can be used to sign token requests in HelseID test environment,
in order for the sample to work out of the box. In the test environment for HelseID, it's a deliberate strategy
that private keys are considered harmless to keep in source control.

**Never keep private keys in source control in a production environment**.

In a real world application, the private key should be kept secret and loaded into the application by a secret provider
on demand.

