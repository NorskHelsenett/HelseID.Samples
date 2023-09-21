using System.CommandLine;
using HelseId.Samples.ApiAccess.Configuration;
using HelseId.Samples.Common.Configuration;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ApiAccess;

// This file is used for bootstrapping the example. Nothing of real interest here.
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

        var useResourceIndicatorsOption = new Option<bool>(
            aliases: new [] {"--use-resource-indicators", "-ri"},
            description: "If set, the application will use resource indicators in order to call two APIs with different audiences",
            getDefaultValue: () => false);

        var useMultiTenantOption = new Option<bool>(
            aliases: new [] {"--use-multi-tenant", "-mt"},
            description: "If set, the application will use a client set up for multi-tenancy, i.e. it makes use of an organization number that is connected to the client.",
            getDefaultValue: () => false);
        
        var useContextualClaimsOption = new Option<bool>(
            aliases: new [] {"--use-contextual-claims", "-cc"},
            description: "If set, the application will use submit a contextual claim to HelseID, which will in turn be returned to the sample API.",
            getDefaultValue: () => false);
        
        var useDPoPOption = new Option<bool>(
            aliases: new [] {"--use-dpop", "-dp"},
            description: "If set, the application will use the demonstrating proof-of-possesion mechanism for sender-constraining the token sent to the example API.",
            getDefaultValue: () => false);
        
        var rootCommand = new RootCommand("An authorization code flow usage sample")
        {
            userLoginOnlyOption, useTokenExchangeOption, useRequsetObjects, useResourceIndicatorsOption, useMultiTenantOption, useContextualClaimsOption, useDPoPOption
        };

        rootCommand.SetHandler((userLoginOnly, useTokenExchange, useRequestObjects, useResourceIndicators, useMultiTenant, useContextualClaims, useDPoP) =>
        {
            var settings = CreateSettings(userLoginOnly, useTokenExchange, useRequestObjects, useResourceIndicators, useMultiTenant, useContextualClaims, useDPoP);
            new Startup(settings).BuildWebApplication().Run();
        }, userLoginOnlyOption, useTokenExchangeOption, useRequsetObjects, useResourceIndicatorsOption, useMultiTenantOption, useContextualClaimsOption, useDPoPOption);

        await rootCommand.InvokeAsync(args);
    }

    private static Settings CreateSettings(
        bool userLoginOnly,
        bool useTokenExchange,
        bool useRequestObjects,
        bool useResourceIndicators,
        bool useMultiTenant,
        bool useContextualClaims,
        bool useDPoP)
    {
        var clientType = GetClientType(userLoginOnly, useTokenExchange, useRequestObjects, useResourceIndicators, useMultiTenant, useContextualClaims);

        return new Settings
        {
            ClientType = clientType,
            ApiUrl1 = GetApiUrl1(clientType),
            ApiUrl2 = GetApiUrl2(clientType),
            ApiAudience1 = GetApiAudience1(clientType),
            ApiAudience2 = GetApiAudience2(clientType),
            HelseIdConfiguration = SetSamplesConfiguration(clientType, useDPoP),
            UseDPoP = useDPoP,
        };
    }

    private static ClientType GetClientType(
        bool userLoginOnly,
        bool useTokenExchange,
        bool useRequestObjects,
        bool useResourceIndicators,
        bool useMultiTenant,
        bool useContextualClaims)
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

        if (useResourceIndicators)
        {
            return ClientType.ApiAccessWithResourceIndicators;
        }

        if (useMultiTenant)
        {
            return ClientType.ApiAccessForMultiTenantClient;
        }

        if (useContextualClaims)
        {
            return ClientType.ApiAccessWithContextualClaims;
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

    private static HelseIdConfiguration SetSamplesConfiguration(ClientType clientType, bool useDPoP)
    {
        var result = clientType switch
        {
            ClientType.ApiAccessWithTokenExchange => HelseIdSamplesConfiguration.ApiAccessWithTokenExchange,
            ClientType.ApiAccessWithRequestObject => HelseIdSamplesConfiguration.ApiAccessWithRequestObject,
            ClientType.ApiAccessWithResourceIndicators => HelseIdSamplesConfiguration.ResourceIndicatorsClient,
            ClientType.UserLoginOnly => HelseIdSamplesConfiguration.UserAuthenticationClient,
            ClientType.ApiAccessForMultiTenantClient => HelseIdSamplesConfiguration.ApiAccessForMultiTenantClient,
            ClientType.ApiAccessWithContextualClaims => HelseIdSamplesConfiguration.ApiAccessWithContextualClaimsClient,
            _ => HelseIdSamplesConfiguration.ApiAccess
        };
        result.UseDPoP = useDPoP;
        return result;
    }
}