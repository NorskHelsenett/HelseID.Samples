using HelseId.SampleApi.Interfaces;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.SampleAPI.Controllers;

[ApiController]
[Authorize(Policy = Startup.ClientCredentialsPolicy, AuthenticationSchemes = Startup.TokenAuthenticationScheme)]
public class ClientCredentialsController : ControllerBase
{
    private readonly IApiResponseCreator _responseCreator;
    public ClientCredentialsController(IApiResponseCreator responseCreator)
    {
        _responseCreator = responseCreator;
    }

    [HttpGet]
    [Route(ConfigurationValues.SampleApiMachineClientResource)]
    public ActionResult<ApiResponse> GetGreetings()
    {
        // Get the claims of the current principal (a system user, not a personal user)
        var claims = User.Claims.ToList();
            
        var apiResponse = _responseCreator.CreateApiResponse(claims, "Sample API");

        return new JsonResult(apiResponse);
    }
}