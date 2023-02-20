using System.Security.Claims;
using HelseId.Samples.ApiAccess.ViewModels;
using HelseId.Samples.Common.Models;

namespace HelseId.Samples.ApiAccess.Interfaces.AccessTokenUpdaters;

public interface IViewModelCreator
{
    Task<ApiResponseViewModel> GetApiResponseViewModel(
        ClaimsPrincipal? claimsPrincipal = null,
        ApiResponse? apiResponse = null,
        bool isSuccess = false,
        string errorMessage = "");
}