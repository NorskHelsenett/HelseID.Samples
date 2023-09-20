## Sample API

This is a sample REST API that is configured to use HelseID to authorize its usage.

The other HelseID samples are configured to use this API. Have an instance of this API running locally before trying to run the other samples.

The API provides three endpoints, `machine-clients/greetings`, `user-login-clients/greetings`, and `machine-clients/dpop-greetings`. 

* The first endpoint has been set up with an access policy that requires the scope `nhn:helseid-public-samplecode/client-credentials`
from the HelseID access token. 

* The second endpoint has been set up with an access policy that requires the scope
`nhn:helseid-public-samplecode/authorization-code` from the HelseID access token, as well as
claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`).

* The third has been set up with an access policy that requires the scope `nhn:helseid-public-samplecode/client-credentials`, and it also 
requires the use of a [DPoP](https://www.rfc-editor.org/rfc/rfc9449) Token.

The localhost port for this API is configured in the file [`ConfigurationValues.cs`](../Configuration/ConfigurationValues.cs) in the `Configuration` folder in the hierarchy above this folder. If you want to change the port number, change the `SampleApiPort`constant in that file.

### OpenAPI/Swagger 

The API also provides a Swagger UI at the endpoint `/swagger`. In addition to the general [OpenAPI metadata](https://swagger.io/specification/), this
UI is set up to retrieve Test Access and DPoP Tokens from a proxy server. (See [../TestTokenProxy/README.md](TestTokenProxy/README.md) for more information about the call to the test token server).

### Requirements

The [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) is required to build the program.

### Default mode 

To run the sample, enter the following on the command line inside this folder:
```
dotnet run
```

### Resource indicators

To run the sample with for use in the Resource Indicator sample for the [ApiAccess](./ApiAccess/README.md) project, enter the following on the command line inside this folder:
```
dotnet run --use-resource-indicator-api-1
```
(for API 1) or 
```
dotnet run --use-resource-indicator-api-2
```
(for API 2)

### To list all options:
```
dotnet run -- --help
```