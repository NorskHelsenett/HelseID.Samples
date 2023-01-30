# Token Exchange for the client credentials flow
This .NET / C# sample shows how to perform token exchange based on an access token obtained using the client credentials flow (system-to-system, no enduser involved).

The sample is minimal, so the code assumes both the role of an client (the subject) requesting the access token, and the API (the actor) using it with token exchange. The sample uses some helper methods from the [IdentityModel](https://github.com/IdentityModel/IdentityModel) library created by the Duende IdentityServer team.

The sample should run out of the box, and uses the HelseID test environment. It uses a private rsa key (formatted as an JWK) as the client secret. **NOTE: Do not reuse the key**

