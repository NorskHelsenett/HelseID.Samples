using System.CommandLine;
using System.Net.Mime;
using HelseId.Samples.ClientCredentials.Client;
using HelseId.Samples.ClientCredentials.ClientInfoEndpoint;
using HelseId.Samples.ClientCredentials.Configuration;
using HelseId.Samples.ClientCredentials.SigningCredentials;
using HelseId.Samples.Common.ApiConsumers;
using HelseId.Samples.Common.ClientAssertions;
using HelseId.Samples.Common.Endpoints;
using HelseId.Samples.Common.Interfaces;
using HelseId.Samples.Common.Interfaces.ApiConsumers;
using HelseId.Samples.Common.Interfaces.ClientAssertions;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.JwtTokens;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenExpiration;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.Common.Models;
using HelseId.Samples.Common.PayloadClaimsCreators;
using HelseId.Samples.Common.TokenExpiration;
using HelseId.Samples.Common.TokenRequests;
using HelseID.Samples.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HelseId.Samples.ClientCredentials;

// This file is used for bootstrapping the example. Nothing of interest here.
static class Program
{
    /// <summary>
    ///     Configuration for the application
    /// </summary>
    public static IConfiguration Configuration { get; private set; }
    
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

        var useClientInfoEndpointOption = new Option<bool>(
            aliases: new [] {"--use-client-info-endpoint", "-ci"},
            description: "If set, the application will use the access token to access the client info endpoint on the HelseID service.",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("A client credentials usage sample")
        {
            useChildOrgNumberOption, useClientInfoEndpointOption, useMultiTenantPattern
        };

        rootCommand.SetHandler(async (useChildOrgNumberOptionValue, useClientInfoEndpointOptionValue, useMultiTenantPatternOptionValue) =>
        {
            Configuration = SetUpConfiguration(useChildOrgNumberOptionValue, useMultiTenantPatternOptionValue);
            var clientConfigurator = new ClientConfigurator();

            var services = ConfigureServices(useChildOrgNumberOptionValue, useClientInfoEndpointOptionValue, useMultiTenantPatternOptionValue);
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<Machine2MachineClient>();

            var repeatCall = true;
            while (repeatCall)
            {
                repeatCall = await CallApiWithToken(client);
            }
        }, useChildOrgNumberOption, useClientInfoEndpointOption, useMultiTenantPattern);

        await rootCommand.InvokeAsync(args);
    }

    private static IServiceCollection ConfigureServices(bool useChildOrganizationNumberOptionValue, bool useClientInfoEndpointOptionValue, bool useMultiTenantPatternOptionValue)
    {
        IServiceCollection services = new ServiceCollection();

        // Configuration should be singleton as the entire application should use one
        services.AddSingleton<IConfiguration>(Configuration);
        services.AddSingleton<IDiscoveryDocumentGetter, DiscoveryDocumentGetter>();
        services.AddSingleton<IHelseIdEndpointsDiscoverer, HelseIdEndpointsDiscoverer>();
        services.AddSingleton<IApiConsumer, ApiConsumer>();
        services.AddSingleton<IJwtPayloadCreator, JwtPayloadCreator>();
        services.AddSingleton<ISigningCredentialsStore, SigningCredentialsStore>();
        services.AddSingleton<ISigningJwtTokenCreator, SigningJwtTokenCreator>();
        services.AddSingleton<IClientAssertionsBuilder, ClientAssertionsBuilder>();
        services.AddSingleton<ITokenRequestBuilder, TokenRequestBuilder>();
        services.AddSingleton<IClientInfoRetriever, ClientInfoRetriever>();
        if (useClientInfoEndpointOptionValue)
        {
            services.AddSingleton<IClientInfoRetriever, ClientInfoRetriever>();
        }
        else
        {
            services.AddSingleton<IClientInfoRetriever, NullClientInfoRetriever>();
        }
        var payloadClaimsCreatorForClientAssertion = SetUpPayloadClaimsCreator(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue);
        services.AddSingleton<IPayloadClaimsCreatorForClientAssertion>(payloadClaimsCreatorForClientAssertion);
        var tokenRequestParameters = SetUpTokenRequestParameters(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue);
        services.AddSingleton(tokenRequestParameters);
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IExpirationTimeCalculator, ExpirationTimeCalculator>();
        services.AddSingleton<Machine2MachineClient>();

        return services;
    }

    private static IPayloadClaimsCreatorForClientAssertion SetUpPayloadClaimsCreator(
        bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue)
    {
        var tokenRequestPayloadClaimsCreator = new ClientAssertionPayloadClaimsCreator(new DateTimeService(), Configuration);

        if (useMultiTenantPatternOptionValue)
        {
            // Sets up payload configuration (for the client assertion) for a client that implements a
            // multi-tenancy pattern.
            // This is done when the '--use-multi-tenant' option is used on the command line.
            return new CompositePayloadClaimsCreator(new List<IPayloadClaimsCreator>
            {
                tokenRequestPayloadClaimsCreator,
                new PayloadClaimsCreatorForMultiTenantClient(),
            });
        }

        if (useChildOrganizationNumberOptionValue)
        {
            // Sets up payload configuration (for the client assertion) for a client that requests an underenhet
            // (child organization) number for the access token.
            // This is done when the '--use-child-org-number' option is used on the command line.
            return new CompositePayloadClaimsCreator(new List<IPayloadClaimsCreator>
            {
                tokenRequestPayloadClaimsCreator,
                new PayloadClaimsCreatorWithChildOrgNumber(),
            });
        }
        
        // Sets up payload configuration (for the client assertion) for a "normal" client
        return tokenRequestPayloadClaimsCreator;
    }

    private static ClientCredentialsTokenRequestParameters SetUpTokenRequestParameters(bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue)
    {
        var result = new ClientCredentialsTokenRequestParameters();
        if (useChildOrganizationNumberOptionValue)
        {
            result.PayloadClaimParameters = new PayloadClaimParameters()
            {
                ChildOrganizationNumber = ConfigurationValues.ClientCredentialsWithChildOrganizationNumber,
            };
        }
        if (useMultiTenantPatternOptionValue)
        {
            result.PayloadClaimParameters = new PayloadClaimParameters()
            {
                ParentOrganizationNumber = ConfigurationValues.FlaksvaagoeyKommuneOrganizationNumber,
                // Optional: we pass on a child organization number for the organization that has
                // delegated rights to the (multitenancy) supplier
                ChildOrganizationNumber = ConfigurationValues.FlaksvaagoeyKommuneChildOrganizationNumber,
            };
        }
        return result;
    }

    private static IConfiguration SetUpConfiguration(
        bool useChildOrgNumberOptionValue,
        bool useMultiTenantPatternOptionValue)
    {
        var appsettingsFileForConfiguration = "appsettings.ClientCredentialsClient.json";
        if (useChildOrgNumberOptionValue)
        {
            appsettingsFileForConfiguration = "appsettings.ClientCredentialsWithChildOrgNumberClient.json";
        }

        if (useMultiTenantPatternOptionValue)
        {
            appsettingsFileForConfiguration = "appsettings.ClientCredentialsForMultiTenantClient.json";
        }

        return new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.General.json", optional: false, reloadOnChange: true)
            .AddJsonFile(appsettingsFileForConfiguration, optional: false, reloadOnChange: true)
            .Build();
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