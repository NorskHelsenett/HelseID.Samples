## API Access with Resource Indicators

This sample shows how resource indicators are used to download multiple access tokens without performing multiple calls to the authorization endpoint. This is especially important when calling national health APIs since most of these require 
access tokens where they are the only audience. 

This sample has access to two APIs

```csharp
  const string firstResource = "udelt:resource_indicator_api_1";
  const string secondResource = "udelt:resource_indicator_api_2";
```

The sample requests scopes from both APIs but the first token-call only requests the first resource, while the second token-call requests the second resource (by using a refresh token).

The access token and refresh token is then written to the console for each resource.


### Requirements

The [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) is required to build the program.

#### To run the sample:
```
dotnet build
dotnet run
```
