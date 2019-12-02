# HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccess.Sample
## Client Authentication With API Access

#### This sample builds upon [Client Authentication Sample](https://github.com/HelseID/HelseID.Samples/tree/master/HelseId.Core.MVCHybrid.ClientAuthentication.Sample) Only API acccess is different.

#### API Access

We are reusing the access token we got from authentication.

Setting this as a 'Bearer' token in the Authorization Header.

Then we call the api, and retrive the result.

```csharp 
 [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //client.SetBearerToken(token); //could also use this, depends on: IdentityModel.OidcClient

            var result = await client.GetStringAsync(_settings.ApiUrl);
            ViewBag.Json = JArray.Parse(result.ToString());

            return View();
        }
``` 

## Prerequisites

Visual Studio 2019

.NET Core 3.0
