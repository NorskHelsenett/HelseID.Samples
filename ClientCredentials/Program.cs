using HelseId.Library;
using HelseId.Library.ClientCredentials;
using HelseId.Library.Configuration;
using HelseId.Library.Models.DetailsFromClient;
using HelseId.Samples.ClientCredentials.CallsToApi;
using HelseId.Samples.ClientCredentials.HostedServices;
using HelseId.Samples.ClientCredentials.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HelseId.Samples.ClientCredentials;

static class Program
{
    static async Task Main(string[] args)
    {
        var helseIdConfiguration = new HelseIdConfiguration
        {
            ClientId = ConfigurationValues.ClientCredentialsSampleClientId,
            Scope = ConfigurationValues.ClientCredentialsSampleScope,
            IssuerUri = ConfigurationValues.IssuerUri, 
            // If you want to set up conformance testing 
            // (https://utviklerportal.nhn.no/informasjonstjenester/helseid/helseid-selvbetjening/test-token-tjenesten/docs/samsvarstesting_enmd)
            // for your client, you can use the session uri as the issuer in this configuration:
            // IssuerUri = "https://samsvarstesting-sts-test.helseid.sky.nhn.no/fbdb001d-91e6-40b3-802a-3fb191ee4f79",
        };
        
        // This is a sample application that only sets up a single service collection.
        // In a proper scenario, you will need to set up a builder, as for instance:
        // var builder = Host.CreateApplicationBuilder(args);
        // builder.Services.AddHelseIdClientCredentials(helseIdConfiguration) ...
        
        var serviceCollection = new ServiceCollection();
        
        // The AddHelseIdClientCredentials method sets the client as a single-tenant client.
        // If you have a multi-tenant client (as in this case), you must use the method .AddHelseIdMultiTenant()
        // as described in the code below.
        
        // The AddHelseIdClientCredentials sets a memory cache that will cache both the Access Token, and the discovery
        // document (https://helseid-sts.test.nhn.no/.well-known/openid-configuration) from HelseID.
        // If you need a distributed cache, you can use the method .AddHelseIdDistributedCaching().
        
        serviceCollection.AddHelseIdClientCredentials(helseIdConfiguration)
            .AddHelseIdMultiTenant()
           // .AddHelseIdDistributedCaching()
            .AddJwkForClientAuthentication(ConfigurationValues.ClientCredentialsSampleRsaPrivateKeyJwk);
        
        serviceCollection.AddSingleton<IApiConsumer, ApiConsumer>();
        
        // Register a service that will call HelseID
        serviceCollection.AddSingleton<ServiceForTokenAndApiResponses>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var serviceForTokenAndApiResponses = serviceProvider.GetService<ServiceForTokenAndApiResponses>();

        // This sets up the organization numbers for a multi-tenant client.
        // If the client is single-tenant, only the child organization is needed.
        var organizationNumbers = new OrganizationNumbers()
        {
            ParentOrganization = ConfigurationValues.GranfjelldalKommuneOrganizationNumber,
            ChildOrganization = ConfigurationValues.GranfjelldalKommuneChildOrganizationNumber1,
        };
        
        await serviceForTokenAndApiResponses!.RunService(ConfigurationValues.ClientCredentialsScopeForSampleApi, organizationNumbers);
    }
}