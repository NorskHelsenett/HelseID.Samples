using System.CommandLine;
using HelseId.Samples.ApiAccess.Configuration;

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

        var useRequestObjectsOption = new Option<bool>(
            aliases: new [] {"--use-request-objects", "-ro"},
            description: "If set, the application will use a request object in the call to the authorization endpoint",
            getDefaultValue: () => false);

        var useRequestObjectsWithContextualClaimsOption = new Option<bool>(
            aliases: new [] {"--use-request-objects-with-contextual-claims", "-roc"},
            description: "If set, the application will use a request object with contextual claims in the call to the authorization endpoint",
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
        
        var rootCommand = new RootCommand("An authorization code flow usage sample")
        {
            userLoginOnlyOption, useTokenExchangeOption, useRequestObjectsOption, useRequestObjectsWithContextualClaimsOption, useResourceIndicatorsOption, useMultiTenantOption, useContextualClaimsOption
        };

        rootCommand.SetHandler((userLoginOnly, useTokenExchange, useRequestObjects, useRequestObjectsWithContextualClaims, useResourceIndicators, useMultiTenant, useContextualClaims) =>
        {
            var settings = CreateSettings(userLoginOnly, useTokenExchange, useRequestObjects, useRequestObjectsWithContextualClaims, useResourceIndicators, useMultiTenant, useContextualClaims);
            new Startup(settings).BuildWebApplication().Run();
        }, userLoginOnlyOption, useTokenExchangeOption, useRequestObjectsOption, useRequestObjectsWithContextualClaimsOption, useResourceIndicatorsOption, useMultiTenantOption, useContextualClaimsOption);

        await rootCommand.InvokeAsync(args);
    }

    private static Settings CreateSettings(
        bool userLoginOnly,
        bool useTokenExchange,
        bool useRequestObjects,
        bool useRequestObjectsWithContextualClaimsOption,
        bool useResourceIndicators,
        bool useMultiTenant,
        bool useContextualClaims)
    {
        if (userLoginOnly)
        {
            return new Settings
            {
                ClientType = ClientType.UserLoginOnly,
                AppsettingsFile = "appsettings.UserAuthentication.json",
            };
        }

        if (useTokenExchange)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessWithTokenExchange,
                AppsettingsFile = "appsettings.ApiAccessWithTokenExchangeClient.json",
            };
        }

        if (useRequestObjects)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessWithRequestObject,
                AppsettingsFile = "appsettings.ApiAccessWithRequestObjectClient.json",
            };
        }
        
        if (useRequestObjectsWithContextualClaimsOption)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessWithRequestObjectsWithContextualClaimsOption,
                AppsettingsFile = "appsettings.ApiAccessWithRequestObjectWithContextualClaimsClient.json",
            };
        }

        if (useResourceIndicators)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessWithResourceIndicators,
                AppsettingsFile = "appsettings.ApiAccessWithResourceIndicatorsClient.json",
            };
        }

        if (useMultiTenant)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessForMultiTenantClient,
                AppsettingsFile = "appsettings.ApiAccessForMultiTenantClient.json",
            };
        }

        if (useContextualClaims)
        {
            return new Settings
            {
                ClientType = ClientType.ApiAccessWithContextualClaims,
                AppsettingsFile = "appsettings.ApiAccessWithContextualClaimsClient.json",
            };
        }

        return new Settings
        {
            ClientType = ClientType.ApiAccess,
            AppsettingsFile = "appsettings.ApiAccessClient.json",
        };
    }
}