## API Access

#### This sample builds upon [Client Authentication Sample](https://github.com/HelseID/HelseID.Samples/tree/master/HelseId.Core.MVCHybrid.ClientAuthentication.Sample). Only API acccess is different.

In order to successfully access the API you need to:

#### 1. Run the [Sample API](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.SampleAPI) such that an API is available for access requests.

Use the URL of the API that appears in the browser as input for the ApiUrl in `appsettings.json` of the [API Access](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.APIAccess) solution. Remember to add the API to the Scope as well.

```csharp
         "AllowedHosts": "*",
            "Settings": {
               "ApiUrl": "https://localhost:5003/api",
               .
               .
       
               "Scope": "... ingvild:sampleapi/ ..."
```


#### 2. API Access

Next, we can access the [Sample API](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.SampleAPI) by running the [API Access](https://github.com/NorskHelsenett/HelseID.Samples/tree/Ingvild-samples/HelseId.APIAccess) solution provided here.

This is done by reusing the access token we got from authentication.

This access token is set as a ``Bearer`` token in the Authorization Header.

Then, we call the API and retrive the result.

```csharp 
        // The action calls the Sample API, authenticates the user and loads the API data if the user is authenticated
        [Authorize]
        public async Task<IActionResult> CallApi()
        {   
            // Gets the access token from the logged in user
            var token = await HttpContext.GetTokenAsync("access_token");

            // Initializes a new client that can asynchronously interact with web resources
            var client = new HttpClient();

            // Sets the access token as a header value in the HTTP request
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Loads data from the given API (ApiUrl), if the client is allowed access to the API
            var result = await client.GetStringAsync(_settings.ApiUrl);

            // The data is stored in a ViewBag, that can be displayed in the view file
            ViewBag.Json = JArray.Parse(result.ToString());

            return View();
        }
``` 

## Prerequisites

Visual Studio 2019

.NET Core 5.0


