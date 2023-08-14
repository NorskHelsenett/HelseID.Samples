using HelseId.Samples.Common.Interfaces;
using HelseId.Samples.Common.TokenRequests;
using HelseId.Samples.Common.ApiConsumers;
using HelseId.Samples.ClientCredentials.Client;
using HelseId.Samples.ClientCredentials.ClientInfoEndpoint;
using HelseId.Samples.Common.ClientAssertions;
using HelseId.Samples.Common.Configuration;
using HelseId.Samples.Common.Endpoints;
using HelseId.Samples.Common.Interfaces.Endpoints;
using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Interfaces.TokenRequests;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.Common.Models;
using HelseId.Samples.Common.PayloadClaimsCreators;
using HelseId.Samples.Common.TokenExpiration;
using HelseID.Samples.Configuration;

namespace HelseId.Samples.ClientCredentials.Configuration;

public class ClientConfigurator
{
    /// <summary>
    /// Sets up and configures the Machine2MachineClient that will be used to call HelseID.
    /// This code uses static configuration from the public Configuration folder (above this project in the file hierarchy).
    /// </summary>
    public Machine2MachineClient ConfigureClient(
        bool useChildOrganizationNumberOptionValue,
        bool useClientInfoEndpointOptionValue,
        bool useMultiTenantPatternOptionValue)
    {
        var discoveryDocumentGetter = new DiscoveryDocumentGetter(ConfigurationValues.StsUrl);
        var endpointDiscoverer = new HelseIdEndpointsDiscoverer(discoveryDocumentGetter);
        var apiConsumer = new ApiConsumer();
        var tokenRequestBuilder = CreateTokenRequestBuilder(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue, endpointDiscoverer);
        var clientInfoRetriever = SetUpClientInfoRetriever(useClientInfoEndpointOptionValue, endpointDiscoverer);
        var tokenRequestParameters = SetUpTokenRequestParameters(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue);
        var expirationTimeCalculator = new ExpirationTimeCalculator(new DateTimeService());
        var payloadClaimsCreator = SetUpPayloadClaimsCreator(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue);

        return new Machine2MachineClient(
            apiConsumer,
            tokenRequestBuilder,
            clientInfoRetriever,
            expirationTimeCalculator,
            payloadClaimsCreator,
            tokenRequestParameters);
    }

    private ITokenRequestBuilder CreateTokenRequestBuilder(bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue, IHelseIdEndpointsDiscoverer endpointsDiscoverer)
    {
        // This sets up the building of a token request for the client credentials grant
        var configuration = SetUpHelseIdConfiguration(useChildOrganizationNumberOptionValue, useMultiTenantPatternOptionValue);
        var jwtPayloadCreator = new JwtPayloadCreator();
        var signingJwtTokenCreator = new JwtTokenCreator(jwtPayloadCreator, configuration);
        // Two builder classes are used
        //   * A ClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        //   * A TokenRequestBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        //  Also, we need a payloadClaimsCreator that sets the claims for the client assertion token.
        //  The instance of this may or may not create a structured claim for the purpose of
        //  getting back an access token with an underenhet (child organization). 
        var clientAssertionsBuilder = new ClientAssertionsBuilder(signingJwtTokenCreator);
        return new TokenRequestBuilder(clientAssertionsBuilder, endpointsDiscoverer, configuration, signingJwtTokenCreator);
    }

    private static IClientInfoRetriever SetUpClientInfoRetriever(bool useClientInfoEndpointOptionValue, IHelseIdEndpointsDiscoverer endpointsDiscoverer)
    {
        return useClientInfoEndpointOptionValue ?
            // We need the "special" client info retriever if the '--use-client-info-endpoint' option is used on the command line:
            new ClientInfoRetriever(endpointsDiscoverer) :
            // Normal flow: we don't need a client info retriever, and set the null (do nothing) version here:
            new NullClientInfoRetriever();
    }

    private  HelseIdConfiguration SetUpHelseIdConfiguration(bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue)
    {
        if (useMultiTenantPatternOptionValue)
        {
            // This is done when the '--use-multi-tenant-pattern' option is used on the command line:
            return HelseIdSamplesConfiguration.ClientCredentialsSampleForMultiTenantClient;
        }
        else if (useChildOrganizationNumberOptionValue)
        {
            // This is done when the '--use-child-org-number' option is used on the command line:
            return HelseIdSamplesConfiguration.ClientCredentialsWithChildOrgNumberClient;
        }
        // Sets up the configuration for a "normal" client:
        return HelseIdSamplesConfiguration.ClientCredentialsClient;
    }
    
    private  IPayloadClaimsCreatorForClientAssertion SetUpPayloadClaimsCreator(bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue)
    {
        var tokenRequestPayloadClaimsCreator = new ClientAssertionPayloadClaimsCreator(new DateTimeService());

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

    private ClientCredentialsTokenRequestParameters SetUpTokenRequestParameters(bool useChildOrganizationNumberOptionValue, bool useMultiTenantPatternOptionValue)
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
}