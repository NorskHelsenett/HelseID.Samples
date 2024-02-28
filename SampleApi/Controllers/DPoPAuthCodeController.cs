using HelseId.SampleApi.Interfaces;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.SampleAPI.Controllers;

[ApiController]
[Authorize(Policy = Startup.AuthCodePolicy, AuthenticationSchemes = Startup.DPoPTokenAuthenticationScheme)]
public class DPoPAuthCodeController : ControllerBase
{
    private readonly IApiResponseCreator _responseCreator;
    public DPoPAuthCodeController(IApiResponseCreator responseCreator)
    {
        _responseCreator = responseCreator;
    }

    [HttpGet]
    [Route(ConfigurationValues.AuthCodeClientResource)]
    public ActionResult<ApiResponse> GetGreetings()
    {
        return CreateResult("Sample API (with DPoP)");
    }

    [HttpGet]
    [Route(ConfigurationValues.ResourceIndicatorsResource1)]
    public ActionResult<ApiResponse> GetForIndicator1()
    {
        return CreateResult("Sample API (indicator 1 with DPoP)");
    }

    [HttpGet]
    [Route(ConfigurationValues.ResourceIndicatorsResource2)]
    public ActionResult<ApiResponse> GetForIndicator2()
    {
        return CreateResult("Sample API (indicator 2 with DPoP)");
    }

    private ActionResult<ApiResponse> CreateResult(string apiName)
    {
        // The claims of the logged in user:
        var claims = User.Claims.ToList();

        var apiResponse = _responseCreator.CreateApiResponse(claims, apiName);

        return new JsonResult(apiResponse);
    }
}
