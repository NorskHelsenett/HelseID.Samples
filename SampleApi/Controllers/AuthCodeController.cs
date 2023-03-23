using HelseId.SampleApi.Configuration;
using HelseId.SampleApi.Interfaces;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.SampleAPI.Controllers;

[ApiController]
public class AuthCodeController: ControllerBase
{
    private readonly IApiResponseCreator _responseCreator;
    public AuthCodeController(IApiResponseCreator responseCreator)
    {
        _responseCreator = responseCreator;
    }

    [Authorize(Policy = Startup.AuthCodePolicy, AuthenticationSchemes = Startup.TokenAuthenticationSchemeForAuthCode)]
    [HttpGet]
    [Route(ApiOptions.AuthorizationCodeResource)]
    public ActionResult<ApiResponse> GetDefault()
    {
        return CreateResult("Sample API");
    }
    
    [Authorize(Policy = Startup.AuthCodePolicyForResourceIndicator1, AuthenticationSchemes = Startup.TokenAuthenticationSchemeForResourceIndicator1)]
    [HttpGet]
    [Route(ApiOptions.ResourceIndicators1Resource)]
    public ActionResult<ApiResponse> GetForIndicator1()
    {
        return CreateResult("Sample API (indicator 1)");
    }
    
    [Authorize(Policy = Startup.AuthCodePolicyForResourceIndicator2, AuthenticationSchemes = Startup.TokenAuthenticationSchemeForResourceIndicator2)]
    [HttpGet]
    [Route(ApiOptions.ResourceIndicators2Resource)]
    public ActionResult<ApiResponse> GetForIndicator2()
    {
        return CreateResult("Sample API (indicator 2)");
    }

    [Authorize(Policy = Startup.ClientCredentialsPolicy, AuthenticationSchemes = Startup.TokenAuthenticationSchemeForClientCredentials)]
    [HttpGet]
    [Route(ApiOptions.ClientCredentialsResource)]
    public ActionResult<ApiResponse> GetGreetings()
    {
        // Get the claims of the current principal (a system user, not a personal user)
        var claims = User.Claims.ToList();
            
        var apiResponse = _responseCreator.CreateApiResponse(claims, "Sample API");

        return new JsonResult(apiResponse);
    }
 
    private ActionResult<ApiResponse> CreateResult(string apiName)
    {
        // The claims of the logged in user:
        var claims = User.Claims.ToList();

        var apiResponse = _responseCreator.CreateApiResponse(claims, apiName);

        return new JsonResult(apiResponse);
    }
}