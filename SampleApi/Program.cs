using System.CommandLine;
using System.CommandLine.Parsing;
using HelseId.SampleApi.Configuration;
using HelseID.Samples.Configuration;

namespace HelseId.SampleAPI;

// This file is used for bootstrapping the example. Nothing of interest here.
public static class Program
{
    public static async Task Main(string[] args)
    {
        Option<bool> useRequestIndicatorApi1Option = new("--use-resource-indicator-api-1", "-a1")
        {
            Description = $"If set, the application will expose an endpoint on localhost port {ConfigurationValues.SampleApiForResourceIndicators1Port}",
        };

        Option<bool> useRequestIndicatorApi2Option = new("--use-resource-indicator-api-2", "-a2")
        {
            Description = $"If set, the application will expose an endpoint on localhost port {ConfigurationValues.SampleApiForResourceIndicators2Port}",
        };

        var rootCommand = new RootCommand("An authorization code flow usage sample")
        {
            useRequestIndicatorApi1Option, useRequestIndicatorApi2Option
        };
        
        rootCommand.SetAction(parseResult =>
        {
            var settings = CreateSettings(
                parseResult.GetValue(useRequestIndicatorApi1Option),
                parseResult.GetValue(useRequestIndicatorApi2Option));
            new Startup(settings).BuildWebApplication().Run();
        });
        
        await rootCommand.Parse(args).InvokeAsync();
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