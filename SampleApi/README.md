## Sample API

This is a sample REST API that is configured to use HelseID to authorize its usage.

The API provides four endpoints:
- `machine-clients/greetings`
  - This enpoint can bee used by a machine-to-machine (client credentials) client
  - This endpoint requires the `aud` claim with a value of `nhn:helseid-public-samplecode`
  - This endpoint requires the scope `nhn:helseid-public-samplecode/client-credentials`
    from the HelseID access token.
  
- `user-login-clients/greetings`
  - This endpoint requires the `aud` claim with a value of `nhn:helseid-public-samplecode`
  - This endpoint requres the scope `nhn:helseid-public-samplecode/authorization-code` from the HelseID access token, as well as
  claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`)

- `resource-indicator-client-1/greetings`
    - This endpoint requires the `aud` claim with a value of `nhn:helseid-public-sample-api-1`
    - This endpoint requires the scope `nhn:helseid-public-sample-api-1/some-scope` from the HelseID access token, as well as
      claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`)

- `resource-indicator-client-2/greetings`
   - This endpoint requires the `aud` claim with a value of `nhn:helseid-public-sample-api-2`
   - This endpoint requires the scope `nhn:helseid-public-sample-api-2/some-scope` from the HelseID access token, as well as
      claims for a logged on user (`helseid://claims/identity/pid`) and a valid security level (`helseid://claims/identity/security_level`)

The other HelseID samples are configured to use this API. Have an instance of this API running locally before trying to run the other samples.

### Requirements

The [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) is required to build the program.

### Default mode 

To run the sample, enter the following on the command line inside this folder:
```
dotnet run
```

