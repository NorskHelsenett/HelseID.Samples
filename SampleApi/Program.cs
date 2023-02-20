using System.CommandLine;
using HelseId.SampleApi.Configuration;
using HelseID.Samples.Configuration;

namespace HelseId.SampleAPI;

// This file is used for bootstrapping the example. Nothing of interest here.
public static class Program
{
    public static async Task Main(string[] args)
    {
        // The Main method uses the System.Commandline library to parse the command line parameters:
        var useRequestIndiicatorApi1Option = new Option<bool>(
            aliases: new[] {"--use-resource-indicator-api-1", "-a1"},
            description:
            $"If set, the application will expose an endpoint on localhost port {ConfigurationValues.SampleApiForResourceIndicators1Port}",
            getDefaultValue: () => false);

        var useRequestIndiicatorApi2Option = new Option<bool>(
            aliases: new[] {"--use-resource-indicator-api-2", "-a2"},
            description:
            $"If set, the application will expose an endpoint on localhost port {ConfigurationValues.SampleApiForResourceIndicators2Port}",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("An authorization code flow usage sample")
        {
            useRequestIndiicatorApi1Option, useRequestIndiicatorApi2Option
        };

        rootCommand.SetHandler((useRequestIndicatorApi1, useRequestIndicatorApi2) =>
        {
            var settings = CreateSettings(useRequestIndicatorApi1, useRequestIndicatorApi2);
            new Startup(settings).BuildWebApplication().Run();
        }, useRequestIndiicatorApi1Option, useRequestIndiicatorApi2Option);

        await rootCommand.InvokeAsync(args);
    }
    
    private static Settings CreateSettings(
        bool useRequestIndicatorApi1,
        bool useRequestIndicatorApi2)
    {
        if (useRequestIndicatorApi1)
        {
            return new Settings
            {
                ApiPort = ConfigurationValues.SampleApiForResourceIndicators1Port,
                Audience = ConfigurationValues.SampleApiForResourceIndicators1Audience,
                AuthCodeApiScopeForSampleApi = ConfigurationValues.SampleApiForResourceIndicators1Scope,
            };
        }
        if (useRequestIndicatorApi2)
        {
            return new Settings
            {
                ApiPort = ConfigurationValues.SampleApiForResourceIndicators2Port,
                Audience = ConfigurationValues.SampleApiForResourceIndicators2Audience,
                AuthCodeApiScopeForSampleApi = ConfigurationValues.SampleApiForResourceIndicators2Scope,
            };
        }
        return new Settings
        {
            ApiPort = ConfigurationValues.SampleApiPort,
            Audience = ConfigurationValues.SampleApiNameAudience,
            AuthCodeApiScopeForSampleApi = ConfigurationValues.AuthorizationCodeScopeForSampleApi,
        };
    }
}