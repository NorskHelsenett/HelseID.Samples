# Native Client Sample for DPoP use

This sample shows how to use a native client (a command line application) to log in an user, 
request a DPoP Token and call an API with this token.

### Requirements

The [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build the program.

First, you will need to start the [Sample API project](../../SampleAPI/README.md) in a separate terminal. 
Then, you can run this application, either with or without a request for a
child organization number. 

## A special case for this example:

The code makes use of the NuGet library `IdentityModel.OidcClient.DPoP`. This library is still in 
preview, and functionality may be changed at a later date. Also, it requires that the
NuGet libraries `Microsoft.IdentityModel.Tokens` and `System.IdentityModel.Tokens.Jwt` are constrained
to version 6. These libraries have known vulnerabilities, and should not be used in a production environment!


#### To run the sample:
```
dotnet run
```
