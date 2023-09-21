## Test token proxy

This project functions as a test token retriever. For instance, it can be used by a Swagger UI application, as described in the
`README.md` file in the `SampleApi` folder.

Most of the code is contained in the `TestAccessTokenController` class. This code calls a service that will create an Access Token for test purposes.

The Token will be signed by the same key as the test version of HelseID uses to sign its Tokens.

In order to get access to this service, you'll need an API key; one is provided in the `appsettings.json` file, and this key
gives authorization for creating tokens for the `nhn:helseid-public-samplecode` audience.

To run the sample, enter the following on the command line inside this folder:
```
dotnet run
```