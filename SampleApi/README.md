## Sample API

This is a sample REST API that is configured to use HelseID to authorize its usage.

The other HelseID samples are configured to use this API. Have an instance of this API running locally before trying to run the other samples.

The API provides these endpoints:

* `/machine-clients/greetings`: this endpoint has been set up with an access policy that requires the scope `nhn:helseid-public-samplecode/client-credentials`
 
* `/user-login-clients/greetings`, `/resource-indicator-client-1/greetings` and `resource-indicator-client-2/greetings`: these endpoints have been set up with an access policy that requires the scope
`nhn:helseid-public-samplecode/authorization-code` from the HelseID access token, as well as
claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`).

All endpoints require the use of a [DPoP](https://www.rfc-editor.org/rfc/rfc9449) Token with an accompanying DPoP proof.

The localhost port for this API is configured in the file [`ConfigurationValues.cs`](../Configuration/ConfigurationValues.cs) in the `Configuration` folder in the hierarchy above this folder. If you want to change the port number, change the `SampleApiPort`constant in that file.

### OpenAPI/Swagger 

The API also provides a Swagger UI, a Javascript library which allows you to explore the documentation for a the API at the [https://localhost:5081/swagger/index.html](https://localhost:5081/swagger/index.html) address. 
In addition to displaying the general [OpenAPI metadata](https://swagger.io/specification/), this UI is set up to retrieve Test DPoP Tokens from a Test Token proxy server. In order to utilize this
functionality, you must also run the `TestTokenProxy` project, which sets up the Test Token proxy server. To do this, go to the `TestTokenProxy` folder and run the command `dotnet run`.

The Swagger UI is set up in the `Startup` class, and an interceptor for its use of the Test Token proxy server is configured with the code
```
options.UseRequestInterceptor($"(req) => {{ return setDPoPTokenInRequest(req, testTokenProxyEndpointAddress); }} ");
```
The function `setDPoPTokenInRequest` described above is contained in the file `./wwwroot/swagger/extend-swagger.js`, and this function 
calls the Test Token proxy server.

See [TestTokenProxy README](https://github.com/NorskHelsenett/HelseID.Samples/blob/master/TestTokenProxy/README.md) for more information about the call to the test token server.

### Requirements

The [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build the program.

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
