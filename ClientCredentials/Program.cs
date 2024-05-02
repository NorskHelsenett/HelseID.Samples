using System.CommandLine;
using HelseId.Samples.ClientCredentials.Client;
using HelseId.Samples.ClientCredentials.Configuration;

namespace HelseId.Samples.ClientCredentials;

// This file is used for bootstrapping the example. Nothing of interest here.
static class Program
{
    static async Task Main(string[] args)
    {
        // The Main method uses the System.Commandline library to parse the command line parameters:
        var useMultiTenantPattern = new Option<bool>(
            aliases: new [] {"--use-multi-tenant", "-mt"},
            description: "If set, the application will use a client set up for multi-tenancy, i.e. it makes use of an organization number that is connected to the client.",
            getDefaultValue: () => false);

        var useChildOrgNumberOption = new Option<bool>(
            aliases: new [] {"--use-child-org-number", "-uc"},
            description: "If set, the application will request an child organization (underenhet) claim for the access token.",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("A client credentials usage sample")
        {
            useChildOrgNumberOption, useMultiTenantPattern
        };

        rootCommand.SetHandler(async (useChildOrgNumberOptionValue, useMultiTenantPatternOptionValue) =>
        {
            var clientConfigurator = new ClientConfigurator();
            var client = clientConfigurator.ConfigureClient(useChildOrgNumberOptionValue, useMultiTenantPatternOptionValue);
            var repeatCall = true;
            while (repeatCall)
            {
                repeatCall = await CallApiWithToken(client);
            }
        }, useChildOrgNumberOption, useMultiTenantPattern);

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
