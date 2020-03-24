using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Core.BFF.Sample.Client.Controllers
{
    [Route("account")]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        [HttpGet("Login")]
        public async Task Login(string redirectUri = "/")
        {
            await HttpContext.ChallengeAsync("HelseID", new AuthenticationProperties {RedirectUri = redirectUri});
        }

        [HttpGet("Logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("HelseID", new AuthenticationProperties
            {
                RedirectUri = "/",
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}