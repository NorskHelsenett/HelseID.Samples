## Sample API for Token Exchange

This is a sample REST API that is configured to use HelseID to authorize its usage. The API controller makes use of the token exchange grant in order to call the 'other' sample API with an appropriate access token.

The API provides one endpoint, `token-exchange-clients/greetings`. 

* The endpoint has been set up with an access policy that requires the scope `nhn:helseid-public-samplecode/client-credentials` from the HelseID access token. 

The APIAccess sample is configured to use this API when using the token exchange functionality. While doing this, you must also run the [Sample API](../SampleApi/README.md) as this API consumes it.

The localhost port for this API is configured in the file `ConfigurationValues.cs`in the `Configuration` folder in the hierarchy above this folder. If you want to change the port number, change the `SampleApiForTokenExchangePort`constant in that file.

### Requirements

The [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to build the program.

#### To run the sample, enter the following on the command line inside this folder:
```
dotnet run
```

