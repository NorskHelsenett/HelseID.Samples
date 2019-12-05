# HelseId.Core.MVCHybrid.ClientAuthentication.Sample
## Client Authentication Sample

#### We start with an ASP.NET Core 3.0 MVC Web Application. 

HelseId is based on OAuth2 and OpenId Connect, it is an Authorization Server that uses identityserver4 as its core component.


A client needs to be registered and configured in the HelseId Server Configuration before use.

#### We add authentication services, using "Cookies" and the OpenID Connect Protcol "oidc":

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

#### We are using Grant Type: 'Authorization code' (ReponseType=code). By using ClientId and ClientSecret that are know to HelseId (Authority)..

```csharp
                    options.Authority = settings.Authority; // "https://helseid-sts.utvikling.nhn.no/"
                    options.ClientId = settings.ClientId; // "NewSample-MVCHybridClientAuthentication"
                    options.ClientSecret = settings.ClientSecret; // "vbHwjXlKYILNpa..."
                    options.ResponseType = settings.ResponseType; // "code"
```
#### ..We identify the client and send in scopes defining the resources and actions we need access to: 

```csharp
                    options.Scope.Add(settings.Scope);  
                    // "openid 
                    //  profile 
                    //  offline_access 
                    //  helseid://scopes/identity/pid 
                    //  helseid://scopes/identity/pid_pseudonym 
                    //  helseid://scopes/identity/assurance_level 
                    //  helseid://scopes/identity/security_level 
                    //  helseid://scopes/hpr/hpr_number 
                    //  helseid://scopes/identity/network"
    
```

#### User is authenticated through one of the federated identity providers. Ex: Id-Porten.

#### We get an accsesstoken back to access protected resources:



><H1>Token.access_token</H1>

>eyJhbGciOiJSUzI1NiIsImtpZCI6IkE2NEUyOTgwMEFGMkIyNzBCMkRERjI2ODAwREZDNjQ0MzMxNURCRTciLCJ0eXAiOiJKV1QiLCJ4NXQiOiJwazRwZ0FyeXNuQ3kzZkpvQU5fR1JETVYyLWMifQ.eyJuYmYiOjE1NzQ5NDU4NDksImV4cCI6MTU3NDk0OTQ0OSwiaXNzIjoiaHR0cHM6Ly9oZWxzZWlkLXN0cy51dHZpa2xpbmcubmhuLm5vIiwiYXVkIjoiaHR0cHM6Ly9oZWxzZWlkLXN0cy51dHZpa2xpbmcubmhuLm5vL3Jlc291cmNlcyIsImNsaWVudF9pZCI6Ik5ld1NhbXBsZS1NVkNIeWJyaWRDbGllbnRBdXRoZW50aWNhdGlvbiIsInN1YiI6InNlR3FhQzlEMnFuLzh0MzdyNmY3K2ZTVitEd25zZkw3OFVBTmV3RW8zeVU9IiwiYXV0aF90aW1lIjoxNTc0OTMwMzk1LCJpZHAiOiJpZHBvcnRlbi1vaWRjIiwianRpIjoiMDBmYjM0YTZmZDRhMjhkYzFiNWJkYTA5NmRkYTdlYWYiLCJzY29wZSI6WyJwcm9maWxlIiwib3BlbmlkIiwiaGVsc2VpZDovL3Njb3Blcy9pZGVudGl0eS9waWRfcHNldWRvbnltIiwiaGVsc2VpZDovL3Njb3Blcy9pZGVudGl0eS9hc3N1cmFuY2VfbGV2ZWwiLCJoZWxzZWlkOi8vc2NvcGVzL2lkZW50aXR5L3BpZCIsImhlbHNlaWQ6Ly9zY29wZXMvaWRlbnRpdHkvc2VjdXJpdHlfbGV2ZWwiLCJoZWxzZWlkOi8vc2NvcGVzL2hwci9ocHJfbnVtYmVyIiwiaGVsc2VpZDovL3Njb3Blcy9pZGVudGl0eS9uZXR3b3JrIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbIkJhbmtJRCJdfQ.OodLwYuHZ5jJ1dNFqfIliFG4gkwTFE_jdJSTOvIkZJIiBsicYQig-mMFe0IyqNgH5QRwIPr9CXzx0MneOU3hlGzO131SQGs2lcV8yifsO3TC7ZT8zPGSNMik9IyyvSkzFjvmUJwKuDDWTFIBaouEALdmoqZjHCgkZ4srJqIUcUpymBANbcfEpXWachdvprczopiAz4j5zAsHcV2xp_cREJtHnPGpEq1sUz4duYNQQLt_RwdCjQSv5q-SdtThxV0ikIbXczSjMQ3eJw5u4narn_ElR5NIvUMDw-9Z-CNWC0uWW_FOJeh5JVaUZjl9UyLIQVxievtfWbyhRJbyFGo0kw

#### And an Id Token that identifies the user:

><H1>Token.id_token</H1>

>eyJhbGciOiJSUzI1NiIsImtpZCI6IkE2NEUyOTgwMEFGMkIyNzBCMkRERjI2ODAwREZDNjQ0MzMxNURCRTciLCJ0eXAiOiJKV1QiLCJ4NXQiOiJwazRwZ0FyeXNuQ3kzZkpvQU5fR1JETVYyLWMifQ.eyJuYmYiOjE1NzQ5NDU4NDksImV4cCI6MTU3NDk0NjE0OSwiaXNzIjoiaHR0cHM6Ly9oZWxzZWlkLXN0cy51dHZpa2xpbmcubmhuLm5vIiwiYXVkIjoiTmV3U2FtcGxlLU1WQ0h5YnJpZENsaWVudEF1dGhlbnRpY2F0aW9uIiwibm9uY2UiOiI2MzcxMDU0MjY0OTc3ODYwODMuWWpOaU9XSm1PREF0TmpNNU55MDBZV0UyTFdFM05UQXRaRFJqTmpjMU16TmtZakJpTVdFNFltVXdPRGd0TURCbVlTMDBOalUxTFdJek5qUXRPVEZoTmpJeVpXVmpZamRrIiwiaWF0IjoxNTc0OTQ1ODQ5LCJhdF9oYXNoIjoiQlllRUpEbm9QRXJLcHhMa3RQZkVyQSIsInNpZCI6ImE0ODYzNDVkMGQ1NmQ5OTY2YWQ3NzAxMDBlZTVjZDcxIiwic3ViIjoic2VHcWFDOUQycW4vOHQzN3I2ZjcrZlNWK0R3bnNmTDc4VUFOZXdFbzN5VT0iLCJhdXRoX3RpbWUiOjE1NzQ5MzAzOTUsImlkcCI6ImlkcG9ydGVuLW9pZGMiLCJsb2NhbGUiOiJuYiIsImhlbHNlaWQ6Ly9jbGFpbXMvaWRlbnRpdHkvcGlkIjoiMjQwMTk0OTEwMzYiLCJoZWxzZWlkOi8vY2xhaW1zL2lkZW50aXR5L3NlY3VyaXR5X2xldmVsIjoiNCIsImhlbHNlaWQ6Ly9jbGFpbXMvaWRlbnRpdHkvYXNzdXJhbmNlX2xldmVsIjoiaGlnaCIsImhlbHNlaWQ6Ly9jbGFpbXMvaWRlbnRpdHkvcGlkX3BzZXVkb255bSI6IkNLTGUxWW41SWMwRmFZWThXZG9uakRQaENRKzhoZTVuekNVM1dQWDBQb2s9IiwibmFtZSI6IktBVEUgTUlLQUxTRU4iLCJnaXZlbl9uYW1lIjoiS0FURSIsImZhbWlseV9uYW1lIjoiTUlLQUxTRU4iLCJoZWxzZWlkOi8vY2xhaW1zL2lkZW50aXR5L25ldHdvcmsiOiJoZWxzZW5ldHQiLCJhbXIiOlsiQmFua0lEIl19.Ar0eEfI7-G9bYDNdZShOsPjFjA2bO0IqQVhLlNDXvVf0xA1prpbZ_Nk0TbUvyjx8rpXaVGWY_LrBRp_mdFF8hR5EaoD6gaMBWFoBymkZoSSYsopbhus2dmRo99_GVgVtg4EKLw9a5EGHWpLDPep6_uL5geD5lqdcCchGmMYx10UH6LSrY-jm8HciFMEel7ttBLFRA_AQzFudNrmkflyubNbqIUi8OLiAtjUFa7FjVyeofEGg7T9TqiNJlASBvH5mwjsw_AyHaeLOFTvoWTA0Fj1NPCEKsfqD6TGZTTUIZrAkyGxt-RT5wQg8GYFjHexdLUkocBfy-_Q53dt4J3g3sQ

## Prerequisites

Visual Studio 2019

.NET Core 3.0
