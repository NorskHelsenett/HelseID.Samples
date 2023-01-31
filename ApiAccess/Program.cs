using System.CommandLine;
using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.Common.Configuration;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess;

public class Program
{
    public static async Task Main(string[] args)
    {
        // The Main method uses the System.Commandline library to parse the command line parameters:
        var userLoginOnlyOption = new Option<bool>(
            aliases: new [] {"--user-login-only", "-ul"},
            description: "If set, the application will only log on a user with no API call",
            getDefaultValue: () => false);

        var useTokenExchangeOption = new Option<bool>(
            aliases: new [] {"--use-token-exchange", "-te"},
            description: "If set, the application will use a client that can be used for a token exchange flow",
            getDefaultValue: () => false);

        var useRequsetObjects = new Option<bool>(
            aliases: new [] {"--use-request-objects", "-ro"},
            description: "If set, the application will use a request object in the call to the authorization endpoint",
            getDefaultValue: () => false);

        var useRequestIndicatorsOption = new Option<bool>(
            aliases: new [] {"--use-request-indicators", "-ri"},
            description: "If set, the application will use resource indicators in order to call two APIs with different audiences",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("An authorization code flow usage sample")
        {
            userLoginOnlyOption, useTokenExchangeOption, useRequsetObjects, useRequestIndicatorsOption
        };

        rootCommand.SetHandler((userLoginOnly, useTokenExchange, useRequestObjects, useRequestIndicators) =>
        {
            var settings = CreateSettings(userLoginOnly, useTokenExchange, useRequestObjects, useRequestIndicators);
            new Startup(settings).BuildWebApplication().Run();
        }, userLoginOnlyOption, useTokenExchangeOption, useRequsetObjects, useRequestIndicatorsOption);

        await rootCommand.InvokeAsync(args);
    }

    private static Settings CreateSettings(
        bool userLoginOnly,
        bool useTokenExchange,
        bool useRequestObjects,
        bool useRequestIndicators)
    {
        var clientType = GetClientType(userLoginOnly, useTokenExchange, useRequestObjects, useRequestIndicators);

        return new Settings
        {
            ClientType = clientType,
            ApiUrl1 = GetApiUrl1(clientType),
            ApiUrl2 = GetApiUrl2(clientType),
            ApiAudience1 = GetApiAudience1(clientType),
            ApiAudience2 = GetApiAudience2(clientType),
            HelseIdConfiguration = SetSamplesConfiguration(clientType),
        };
    }

    private static ClientType GetClientType(
        bool userLoginOnly,
        bool useTokenExchange,
        bool useRequestObjects,
        bool useRequestIndicators)
    {
        if (userLoginOnly)
        {
            return ClientType.UserLoginOnly;
        }

        if (useTokenExchange)
        {
            return ClientType.ApiAccessWithTokenExchange;
        }

        if (useRequestObjects)
        {
            return ClientType.ApiAccessWithRequestObject;
        }

        if (useRequestIndicators)
        {
            return ClientType.ApiAccessWithResourceIndicators;
        }

        return ClientType.ApiAccess;
    }

    private static string GetApiUrl1(ClientType clientType)
    {
        return clientType switch
        {
            ClientType.ApiAccessWithTokenExchange => ConfigurationValues.SampleApiUrlForTokenExchange,
            ClientType.ApiAccessWithResourceIndicators => ConfigurationValues.SampleApiUrlForResourceIndicators1,
            _ => ConfigurationValues.SampleApiUrl
        };
    }

    private static string GetApiUrl2(ClientType clientType)
    {
        return clientType == ClientType.ApiAccessWithResourceIndicators ?
            ConfigurationValues.SampleApiUrlForResourceIndicators2 :
            string.Empty;
    }

    private static string GetApiAudience1(ClientType clientType)
    {
        return clientType switch
        {
            ClientType.ApiAccessWithTokenExchange => ConfigurationValues.SampleTokenExchangeApiAudience,
            ClientType.ApiAccessWithResourceIndicators => ConfigurationValues.SampleApiForResourceIndicators1Audience,
            _ => ConfigurationValues.SampleApiNameAudience
        };
    }

    private static string GetApiAudience2(ClientType clientType)
    {
        return clientType == ClientType.ApiAccessWithResourceIndicators ?
            ConfigurationValues.SampleApiForResourceIndicators2Audience :
            string.Empty;
    }

    private static HelseIdConfiguration SetSamplesConfiguration(ClientType clientType)
    {
        return clientType switch
        {
            ClientType.ApiAccessWithTokenExchange => HelseIdSamplesConfiguration.ApiAccessWithTokenExchange,
            ClientType.ApiAccessWithRequestObject => HelseIdSamplesConfiguration.ApiAccessWithRequestObject,
            ClientType.ApiAccessWithResourceIndicators => HelseIdSamplesConfiguration.ResourceIndicatorsClient,
            ClientType.UserLoginOnly => HelseIdSamplesConfiguration.UserAuthenticationClient,
            _ => HelseIdSamplesConfiguration.ApiAccess
        };
    }
}