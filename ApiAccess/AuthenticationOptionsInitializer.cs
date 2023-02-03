using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace HelseId.Samples.ApiAccess;

// Sets the options for authentication schemes 
public class AuthenticationOptionsInitializer : IConfigureOptions<AuthenticationOptions>
{
    public void Configure(AuthenticationOptions authenticationOptions)
    {
        authenticationOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // OpenID connect is used as authentication mechanism when a user wants to access the requested resource
        authenticationOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; 
    }
}