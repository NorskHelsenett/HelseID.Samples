
## API Access With New Token

#### This sample builds upon [API Access](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.APIAccess). Only API acccess with new access token is different.

#### API Access with new access token

In this sample we are requesting a new access token before we access our API. We do this to ensure that the token has as few scopes/access as possible.

To get our new access token, we first set up a new OpenID Connect client for authentication against HelseID.

The client is then redirected to the authorization url.

```csharp 
  public async Task<IActionResult> Index()
        {

            // Setup the oidc client for authentication against HelseID
            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.Authority,
                RedirectUri = _settings.RedirectUri,
                Scope = _settings.Scope,
                ClientId = _settings.ClientId,
            });

            // In production the cookie must be encrypted
            var state = await oidcClient.PrepareLoginAsync();
            Response.Cookies.Append("state", JsonSerializer.Serialize(state));

            // The action redirects the user to the authorization url
            return Redirect(state.StartUrl);
        }
``` 
When authorized, the server will redirect to our ``RedirectUri``, where we ask for a new token.

The token we get back contains information on the lifetime of the token. It can be used several times, thus we do not need to ask for a new token for each connection.

We set our new token as a ``bearer`` token in the authorization header, and connect to our API. <br />

```csharp 
  public async Task<IActionResult> Token()
        {

            var disco = await _cache.GetAsync();

            // Setup the oidc client for authentication against HelseID
            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.Authority,
                RedirectUri = _settings.RedirectUri,
                Scope = _settings.Scope,
                ClientId = _settings.ClientId,
                ClientAssertion = new ClientAssertion() {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = BuildClientAssertion.Generate(_settings.ClientId, disco.TokenEndpoint, new JsonWebKey(System.IO.File.ReadAllText("jwk.json")))
                }
                });

            var state = JsonSerializer.Deserialize<AuthorizeState>(Request.Cookies["state"]);
            var tokenResponse = await oidcClient.ProcessResponseAsync(Request.QueryString.ToString(), state);

       
            //New access token retrieved from the token response
            var accessToken = tokenResponse.AccessToken;

            var client = new HttpClient();
            // Sets the new access token as bearer 
            client.SetBearerToken(accessToken);
            // Accesses the API with the new access token
            var response = await client.GetStringAsync(_settings.ApiUrl);

            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = accessToken.ToString();

            return View();
``` 

## Prerequisites

Visual Studio 2019

.NET Core 5.0
