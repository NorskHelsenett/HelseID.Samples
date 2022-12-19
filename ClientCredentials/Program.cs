using System.CommandLine;
using HelseId.Samples.ClientCredentials.Client;
using HelseId.Samples.ClientCredentials.Configuration;

namespace HelseId.Samples.ClientCredentials;

/// <summary>
/// This sample shows how to use the client credential grant to get an access token from HelseID
/// and the use this access token to consume a sample API.
/// </summary>
static class Program
{
    static async Task Main(string[] args)
    {
        // The Main method uses the System.Commandline library to parse the command line parameters:
        var useChildOrgNumberOption = new Option<bool>(
            aliases: new [] {"--use-child-org-number", "-uc"},
            description: "If set, the application will request an child organization (underenhet) claim for the access token.",
            getDefaultValue: () => false);

        var useClientInfoEndpointOption = new Option<bool>(
            aliases: new [] {"--use-client-info-endpoint", "-ci"},
            description: "If set, the application will use the access token to access the client info endpoint on the HelseID service.",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("A client credentials usage sample")
        {
            useChildOrgNumberOption, useClientInfoEndpointOption
        };

        rootCommand.SetHandler(async (useChildOrgNumberOptionValue, useClientInfoEndpointOptionValue) =>
        {
            var clientConfigurator = new ClientConfigurator();
            var client = clientConfigurator.ConfigureClient(useChildOrgNumberOptionValue, useClientInfoEndpointOptionValue);
            var repeatCall = true;
            while (repeatCall)
            {
                repeatCall = await CallApiWithToken(client);
            }
        }, useChildOrgNumberOption, useClientInfoEndpointOption);

        await rootCommand.InvokeAsync(args);
    }

    private static async Task<bool> CallApiWithToken(Machine2MachineClient client)
    {
        await client.CallApiWithToken();
        return ShouldCallAgain();
    }

    private static bool ShouldCallAgain()
    {
        Console.WriteLine("Type 'a' to call the API again, or any other key to exit:");
        var input = Console.ReadKey();
        Console.WriteLine();
        return input.Key == ConsoleKey.A;
    }
}