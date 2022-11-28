## API Access

#### This sample builds upon [Client Authentication Sample](https://github.com/HelseID/HelseID.Samples/tree/master/HelseId.ClientAuthentication). Only API acccess is different.

### Requirements

The [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) is required to build the program.

### Building and running the program

In order to successfully access the API you will need to


#### 1. Run the [Sample API](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI) such that an API is available for access requests.

Go to the `SampleAPI` folder and run `dotnet run`.

Check that the URL of the API that appears in the console (for example `https://localhost:5181` matches the value for 'ApiUrl' in `appsettings.json` in the [API Access](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/APIAccess) application.

```csharp
         "AllowedHosts": "*",
            "Settings": {
               "ApiUrl": "https://localhost:5081/api",
```


#### 2. API Access

Next, we can get access to the [Sample API](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.SampleAPI) by running the [API Access](https://github.com/NorskHelsenett/HelseID.Samples/tree/master/APIAccess) application provided here.

This is done by sending the access token we got from authentication process as a bearer token when accessing the Sample API.

The access token is set as a ``Bearer`` token in the Authorization Header, after which we call the API and retrive the result. You can find this code in the `HomeController.cs` file:

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


