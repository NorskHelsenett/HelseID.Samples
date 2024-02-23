using System.Diagnostics;
using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using Microsoft.AspNetCore.Mvc;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseID.Samples.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;

namespace HelseId.Samples.ApiAccess.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Settings _settings;
    private readonly IApiConsumer _apiConsumer;
    private readonly IAccessTokenUpdater _accessTokenUpdater;
    private readonly IViewModelCreator _viewModelCreator;

    public HomeController(
        ILogger<HomeController> logger,
        Settings settings,
        IApiConsumer apiConsumer,
        IAccessTokenUpdater accessTokenUpdater,
        IViewModelCreator viewModelCreator)
    {
        _logger = logger;
        _settings = settings;
        _apiConsumer = apiConsumer;
        _accessTokenUpdater = accessTokenUpdater;
        _viewModelCreator = viewModelCreator;
    }

    public IActionResult Index()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction(nameof(Login));
        }
        return View();
    }

    // Starts the configured authentication scheme when the user clicks Login
    [Authorize(Startup.SecurityLevelClaimPolicy)]
    public async Task<IActionResult> Login()
    {
        var responseViewModel = await _viewModelCreator.GetApiResponseViewModel(claimsPrincipal: HttpContext.User);

        // Pre-login functionality for multi-tenant pattern:
        if (_settings.ClientType == ClientType.ApiAccessForMultiTenantClient)
        {
            if (string.IsNullOrEmpty(responseViewModel.UserSessionData.SessionId))
            {
                // Not a logged on user; redirect to login
                return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
            }
            if (responseViewModel.UserSessionData.SelectedOrganization.IsEmpty)
            {
                // The user must choose an organization
                return RedirectToAction(nameof(SelectOrganizationForMultiTenantPattern));
            }
        }

        // Standard login
        return View(responseViewModel);
    }

    [Authorize(Startup.SecurityLevelClaimPolicy)]
    public async Task<IActionResult> SelectOrganizationForMultiTenantPattern()
    {
        try
        {
            var responseViewModel = await _viewModelCreator.GetApiResponseViewModel(claimsPrincipal: HttpContext.User);

            return View(responseViewModel);
        } catch (SessionIdDoesNotExistException)
        {
            // The user's session was not found in the token store -- log in the user:
            return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }

    [Authorize(Startup.SecurityLevelClaimPolicy)]
    [HttpPost]
    public async Task<IActionResult> SelectOrganizationForMultiTenantPattern(int organizationId)
    {
        try
        {
            var organization = OrganizationStore.GetOrganization(organizationId);
            await _accessTokenUpdater.SetOrganizationAndDeleteTokens(HttpContext.User, organization!);

            return RedirectToAction(nameof(Login));
        } catch (SessionIdDoesNotExistException)
        {
            // The user's session was not found in the token store -- log in the user:
            return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
        }
    }

    [Route("home/call-api-1-with-resource-indicators")]
    [Authorize(Startup.SecurityLevelClaimPolicy)]
    public async Task<IActionResult> CallApi1WithResourceIndicators()
    {
        var apiUrl = _settings.ApiUrl1;
        var apiIndicators = new ApiIndicators
        {
            ApiAudience = _settings.ApiAudience1,
            ResourceIndicator = ConfigurationValues.SampleApiForResourceIndicators1Audience,
        };

        return await CallApi(apiIndicators, apiUrl);
    }

    [Route("home/call-api-2-with-resource-indicators")]
    [Authorize(Startup.SecurityLevelClaimPolicy)]
    public async Task<IActionResult> CallApi2WithResourceIndicators()
    {
        var apiUrl = _settings.ApiUrl2;
        var apiIndicators = new ApiIndicators
        {
            ApiAudience = _settings.ApiAudience2,
            ResourceIndicator = ConfigurationValues.SampleApiForResourceIndicators2Audience,
        };

        return await CallApi(apiIndicators, apiUrl);
    }

    [Route("home/call-api")]
    [Authorize(Startup.SecurityLevelClaimPolicy)]
    public async Task<IActionResult> CallApi()
    {
        var apiUrl = _settings.ApiUrl1;
        var apiIndicators = new ApiIndicators
        {
            ApiAudience = _settings.ApiAudience1,
        };
        return await CallApi(apiIndicators, apiUrl);
    }

    private async Task<IActionResult> CallApi(ApiIndicators apiIndicators, string apiUrl)
    {
        using var httpClient = new HttpClient();
        try
        {
            // This might be a new or a stored access token
            var accessToken = await _accessTokenUpdater.GetValidAccessToken(httpClient, HttpContext.User, apiIndicators);

            // We use the token to call the API endpoint
            //var apiResponse = await _apiConsumer.CallApiWithBearerToken(httpClient, apiUrl, accessToken);
            var apiResponse = await _apiConsumer.CallApiWithDPoPToken(httpClient, apiUrl, accessToken);

            return View("Login", await _viewModelCreator.GetApiResponseViewModel(
                claimsPrincipal: HttpContext.User,
                apiResponse: apiResponse!,
                isSuccess: true));
        }
        catch (SessionIdDoesNotExistException)
        {
            // The user's session was not found in the token store -- log in the user:
            return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
        }
        catch (RefreshTokenExpiredException)
        {
            // The refresh token has expired -- log in the user:
            return Challenge(OpenIdConnectDefaults.AuthenticationScheme);
        }
        catch (HttpRequestException e)
        {
            return View(
                "Login",
                await _viewModelCreator.GetApiResponseViewModel(claimsPrincipal: HttpContext.User, errorMessage: e.Message));
        }
    }

    public IActionResult Logout()
    {
        return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
