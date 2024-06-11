using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.ApiAccess.Models;
using HelseId.Samples.Common.Models;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess.ViewModels;

public class ApiResponseViewModel
{
    public ClientType ClientType { get; init; }

    public bool IsSuccess { get; init; }

    public string ErrorMessage { get; init; } = string.Empty;

    public ApiResponse ApiResponse { get; init; } = new();

    public UserSessionData UserSessionData { get; init; } = new();

    public List<Organization> SelectableOrganizations { get; } = new List<Organization>
    {
        OrganizationStore.GetOrganization((int) OrganizationIds.HansensLegekontor)!,
        OrganizationStore.GetOrganization((int) OrganizationIds.FlaksvaagoeyKommuneBoOgOmsorgssenter)!,
    };
}