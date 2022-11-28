## Retreiving client information from HelseID

The sample requires a client configuration in the HelseID test-environment before use. It is ready to run using an existing configuration in the HelseID test environment.

```csharp
  private const string _jwkPrivateKey = @"Add your Jwk Private Key Here";
  private const string _clientId = "Add your client_id here";
  private const string _scopes = "Add scopes here";
```


### Requirements

The [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) is required to build the program.

#### To run the sample:
```
dotnet build
dotnet run
```
