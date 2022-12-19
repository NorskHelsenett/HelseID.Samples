using System.Security.Claims;
using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.ApiAccess.Exceptions;
using HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.ApiAccess.ViewModels;

public class ViewModelCreator : IViewModelCreator
{
    private readonly IUserSessionGetter _userSessionGetter;
    private readonly Settings _settings;

    public ViewModelCreator(
        Settings settings,
        IUserSessionGetter userSessionGetter)
    {
        _settings = settings;
        _userSessionGetter = userSessionGetter;
    }

    public async Task<ApiResponseViewModel> GetApiResponseViewModel(
        ClaimsPrincipal? claimsPrincipal = null,
        ApiResponse? apiResponse = null,
        bool isSuccess = false,
        string errorMessage = "")
    {
        var userSessionData = await GetUserSessionData(claimsPrincipal);

        return new ApiResponseViewModel
        {
            ClientType = _settings.ClientType,
            IsSuccess = isSuccess,
            ApiResponse = apiResponse ?? new ApiResponse(),
            ErrorMessage = errorMessage,
            UserSessionData = userSessionData,
        };
    }

    private async Task<UserSessionData> GetUserSessionData(ClaimsPrincipal? claimsPrincipal)
    {
        var userSessionData = new UserSessionData();
        try
        {
            if (claimsPrincipal != null)
            {
                userSessionData = await _userSessionGetter.GetUserSessionData(claimsPrincipal);
            }
        }
        catch (SessionIdDoesNotExistException)
        {
            // No session is allowed in this context
        }

        return userSessionData;
    }
}
