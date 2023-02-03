using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Samples.ApiAccess.Controllers;

public class AuthorizationController : Controller
{
    private readonly IAuthorizationService _authorizationService;
    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    [Route("authorization/access-denied")]
    public async Task<IActionResult> AccessDenied()
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, Startup.SecurityLevelClaimPolicy);
        if (!authorizationResult.Succeeded)
        {
            return View("WrongSecurityLevel");
        }
        // Handle other types of authorization failures here:
        throw new Exception();
    }
}