# HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample
## Client Authentication And API Access With New Token

#### This sample builds upon [Client Authentication Sample](https://github.com/HelseID/HelseID.Samples/tree/master/HelseId.Core.MVCHybrid.ClientAuthentication.Sample) Only API acccess with new access token is different.

#### API Access with new access token

In this sample we are requesting a new access token before we access our API, we do this to ensure that the token has as few scopes/access as possible.

To get our new access token, we first connect to our 'Discovery Endpoint' (and cache this) to get metadata for the connection.

We get our request url, and then compose the autorize url.

```csharp 
  [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var cache = new DiscoveryCache(_settings.Authority);
            var disco = await cache.GetAsync();

            var requestUrl = new RequestUrl(disco.AuthorizeEndpoint);
            var authorizeUrl = requestUrl.CreateAuthorizeUrl(
                  _settings.ApiClientId,  // "NewSample-MVCHybridClientAuthentication"
                  _settings.ResponseType, // "code"
                  _settings.ApiScope,     // "willy:newsampleapi/"
                  _settings.RedirectUri,  // "https://localhost:44388/Auth/Token"
                  _settings.Nonce,        // "3123131313"
                  prompt: "none", 
                  responseMode: "form_post");

            return Redirect(authorizeUrl);
        }
``` 
When authorized, the server will redirect to our 'RedirectUri', where we ask for a new token.

The token we get back contains info on the lifetime of the token, it can be used several times, we do not need to ask for a new token for each connection.

We set our new token as a 'bearer' token in the authorization header, and connect to our API

```csharp 
  public async Task<IActionResult> Token(string code)
        {

            var hc = new HttpClient();
             
            var cache = new DiscoveryCache(_settings.Authority);
            var disco = await cache.GetAsync();
            
            var tokenRequest = new AuthorizationCodeTokenRequest { 
                  Address = disco.TokenEndpoint, 
                  ClientId = _settings.ApiClientId, 
                  RedirectUri = _settings.RedirectUri, 
                  ClientSecret = _settings.ApiClientSecret, 
                  Code = code };
         
            var tokenResponse = await hc.RequestAuthorizationCodeTokenAsync(tokenRequest);
            var accessToken = tokenResponse.AccessToken;
            
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var response = await client.GetStringAsync(_settings.ApiUrl);
           
            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = new JwtBuilder().Decode(accessToken);

            return View();
        }
``` 

## Prerequisites

Visual Studio 2019

.NET Core 3.0
