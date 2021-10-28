## Client Credentials with JWK as secret

The sample requires a client configuration in the HelseID test-environment before use.

The ClientId, JWK and Scope is then sent into the sample

```csharp
  private const string _jwkPrivateKey = @"Add your Jwk Private Key Here";
  private const string _clientId = "Add your client_id here";
  private const string _scopes = "Add scopes here";
```
