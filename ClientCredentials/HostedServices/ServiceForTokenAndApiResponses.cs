using HelseId.Library.ClientCredentials.Interfaces;
using HelseId.Library.ExtensionMethods;
using HelseId.Library.Models.DetailsFromClient;
using HelseId.Samples.ClientCredentials.Interfaces;
using HelseId.Samples.Common.Models;
using Microsoft.Extensions.Logging;

namespace HelseId.Samples.ClientCredentials.HostedServices;

public class ServiceForTokenAndApiResponses (
    IHelseIdClientCredentialsFlow helseIdClientCredentialsFlow,
    IApiConsumer apiConsumer,
    ILogger<ServiceForTokenAndApiResponses> logger)
{
    public async Task RunService(string scope, OrganizationNumbers organizationNumbers)
    {
        var repeatCall = true;
        while (repeatCall)
        {
            repeatCall = await CallApiWithToken(scope, organizationNumbers);
        }
    }
    
    private async Task<bool> CallApiWithToken(string scope, OrganizationNumbers organizationNumbers)
    {
        var tokenResponse = await helseIdClientCredentialsFlow.GetTokenResponseAsync(scope, organizationNumbers);
            
        // Checks if the access token response is successful  
        if (!tokenResponse.IsSuccessful(out var accessTokenResponse))
        {
            // If the response is not successful, you can get an error response:
            var errorResponse = tokenResponse.AsError();
            logger.LogError(errorResponse.Error + " " + errorResponse.ErrorDescription);
            throw new Exception(errorResponse.Error + " " + errorResponse.ErrorDescription);
        }
            
        var apiResponse = await apiConsumer.CallApiWithDPoPToken(ConfigurationValues.SampleApiUrlForM2M, accessTokenResponse);
        SetResponseInConsole(apiResponse);

        return ShouldCallAgain();
    }

    private void SetResponseInConsole(ApiResponse? apiResponse)
    {
        Console.WriteLine("Response from the sample API:");
        Console.WriteLine($"{apiResponse?.Greeting}");
        Console.WriteLine($"Supplier organization number (for multitenancy): '{apiResponse?.SupplierOrganizationNumber ?? "not present"}'");
        Console.WriteLine($"Parent organization number: '{apiResponse?.ParentOrganizationNumber ?? "not present"}'");
        Console.WriteLine($"Child organization number: '{apiResponse?.ChildOrganizationNumber ?? "not present"}'");
    }
    
    private static bool ShouldCallAgain()
    {
        Console.WriteLine("Type 'a' to call the API again, or any other key to exit:");
        var input = Console.ReadKey();
        Console.WriteLine();
        return input.Key == ConsoleKey.A;
    }
}