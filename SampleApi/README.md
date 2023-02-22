## Sample API

This is a sample REST API that is configured to use HelseID to authorize its usage.

The API provides two endpoints, `machine-clients/greetings` and `user-login-clients/greetings`. 

* The former has been set up with an access policy that requires the scope `nhn:helseid-public-samplecode/client-credentials`
from the HelseID access token. 

* The latter has been set up with an access policy that requres the scope
`nhn:helseid-public-samplecode/authorization-code` from the HelseID access token, as well as
claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`).

The other HelseID samples are configured to use this API. Have an instance of this API running locally before trying to run the other samples.

The localhost port for this API is configured in the file [`ConfigurationValues.cs`](../Configuration/ConfigurationValues.cs) in the `Configuration` folder in the hieararchy above this folder. If you want to change the port number, change the `SampleApiPort`constant in that file.

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