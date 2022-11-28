## Client Authentication Sample

### Requirements

The [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) is required to build the program.

#### To run the sample:
```
dotnet build
dotnet run
```

#### We start with a .NET 6 MVC Web Application. 

The client application is based on [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0), and the `startup.cs` file contains most of the relevant code.

HelseID is an Authorization Server that is based upon the protocols OAuth2 and OpenId Connect. Before we can use HelseID, we need to have a *client* registered and configured in the HelseID Server Configuration. 

This sample code uses a pre-existing client with the ID = "NewSample-MVCHybridClientAuthentication", which is already present in the development environment for HelseID.

##### Using HelseID from the application
We add authentication services, using "Cookies" and the OpenID Connect Protcol "oidc":

```csharp
 services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
                
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
```

We are using Grant Type: 'Authorization code' (ReponseType=code), and a ClientId that is known to HelseId (Authority)..

```csharp
                    options.Authority = settings.Authority; // "https://helseid-sts.utvikling.nhn.no/"
                    options.ClientId = settings.ClientId; // "NewSample-MVCHybridClientAuthentication"
                    options.ResponseType = settings.ResponseType; // "code"
```
 We send in scopes defining the resources and actions we need access to, in this case `openid`, `profile`, `helseid://scopes/identity/pid helseid://scopes/`, and `identity/security_level`: 

```csharp
                    options.Scope.Add(settings.Scope);   
```

 A Json Web Key (JWK) pair is used as ClientSecret where the public key is known to HelseId (Authority).  

```csharp
                    // In production environment the security key must be protected (stored at a secure location)
                    string fileName = "jwk.json";
                    var securityKey = new JsonWebKey(File.ReadAllText(fileName));
```
<strong> NB! </strong> In this sample the private key is stored as a JSON file inside the project ("jwk.json"). <strong> In a production environment the private security key MUST be protected and stored at a secure location. </strong>

After starting the project with `dotnet start`, you can access the web host on https://localhost:5001 via a web browser. You will be redirected to HelseID, and you can authenticate as a user through one of the federated identity providers.

After authentication, you will be redirected back to the application. You get a view of the user's claims, and you can inspect both the access token and the ID token:

><dt>.Token.access_token</dt>
><dd>eyJhbGciOiJSUzI1NiIsImtpZCI6IkI0Q0FFNDUyQzhCNkE4OTNCNkE4NDBBQzhDODRGQjA3MEE0MjZFNDEiLCJ4NXQiOiJ0TXJrVXNpMnFKTzJxRUNzaklUN0J3Nia0UiLCJ0eXAiOiJK… (rest of access token)</dd>
><dt>.Token.id_token</dt>
><dd>eyJhbGciOiJSUz… (rest of acces tokken)</dd>

