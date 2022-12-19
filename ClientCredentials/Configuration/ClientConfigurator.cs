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
        bool useClientInfoEndpointOptionValue)
    {
        var endpointDiscoverer = new HelseIdEndpointDiscoverer(ConfigurationValues.StsUrl);
        var apiConsumer = new ApiConsumer();
        var tokenRequestBuilder = CreateTokenRequestBuilder(useChildOrganizationNumberOptionValue, endpointDiscoverer);
        var clientInfoRetriever = SetUpClientInfoRetriever(useClientInfoEndpointOptionValue, endpointDiscoverer);
        var tokenRequestParameters = SetUpTokenRequestParameters(useChildOrganizationNumberOptionValue);
        var expirationTimeCalculator = new ExpirationTimeCalculator(new DateTimeService());
        var payloadClaimsCreator = SetUpPayloadClaimsCreator(useChildOrganizationNumberOptionValue);

        return new Machine2MachineClient(
            apiConsumer,
            tokenRequestBuilder,
            clientInfoRetriever,
            expirationTimeCalculator,
            payloadClaimsCreator,
            tokenRequestParameters);
    }

    private ITokenRequestBuilder CreateTokenRequestBuilder(bool useChildOrganizationNumberOptionValue, IHelseIdEndpointDiscoverer endpointDiscoverer)
    {
        // This sets up the building of a token request for the client credentials grant
        var configuration = SetUpHelseIdConfiguration(useChildOrganizationNumberOptionValue);
        var jwtPayloadCreator = new JwtPayloadCreator();
        var signingJwtTokenCreator = new SigningJwtTokenCreator(jwtPayloadCreator, configuration);
        // Two builder classes are used
        //   * A ClientAssertionsBuilder, which creates a client assertion that will be used
        //     inside the token request to HelseID in order to authenticate this client
        //   * A TokenRequestBuilder, which creates the token request that is used against
        //     the HelseID service, and also finds the token endpoint for this request
        //  Also, we need a payloadClaimsCreator that sets the claims for the client assertion token.
        //  The instance of this may or may not create a structured claim for the purpose of
        //  getting back an access token with an underenhet (child organization). 
        var clientAssertionsBuilder = new ClientAssertionsBuilder(signingJwtTokenCreator);
        return new TokenRequestBuilder(clientAssertionsBuilder, endpointDiscoverer, configuration);
    }

    private static IClientInfoRetriever SetUpClientInfoRetriever(bool useClientInfoEndpointOptionValue, IHelseIdEndpointDiscoverer endpointDiscoverer)
    {
        return useClientInfoEndpointOptionValue ?
            // We need the "special" client info retriever if the '--use-client-info-endpoint' option is used on the command line:
            new ClientInfoRetriever(endpointDiscoverer) :
            // Normal flow: we don't need a client info retriever, and set the null (do nothing) version here:
            new NullClientInfoRetriever();
    }

    private  HelseIdConfiguration SetUpHelseIdConfiguration(bool useChildOrganizationNumberOptionValue)
    {
        return useChildOrganizationNumberOptionValue ?
            // This is done when the '--use-child-org-number' option is used on the command line:
            HelseIdSamplesConfiguration.ConfigurationForClientCredentialsWithUnderenhetClient :
            // Sets up the configuration for a "normal" client:
            HelseIdSamplesConfiguration.ConfigurationForClientCredentialsClient;
    }
    
    private  IPayloadClaimsCreatorForClientAssertion SetUpPayloadClaimsCreator(bool useChildOrganizationNumberOptionValue)
    {
        var tokenRequestPayloadClaimsCreator = new ClientAssertionPayloadClaimsCreator(new DateTimeService());
        
        return useChildOrganizationNumberOptionValue ?
            // Sets up payload configuration (for the client assertion) for a client that requests an underenhet
            // (child organization) number for the access token.
            // This is done when the '--use-child-org-number' option is used on the command line.
            new CompositePayloadClaimsCreator(new List<IPayloadClaimsCreator>
            {
                tokenRequestPayloadClaimsCreator,
                new PayloadClaimsCreatorWithChildOrgNumber(),
            }) :
            // Sets up payload configuration (for the client assertion) for a "normal" client
            tokenRequestPayloadClaimsCreator;
    }

    private ClientCredentialsTokenRequestParameters SetUpTokenRequestParameters(bool useChildOrganizationNumberOptionValue)
    {
        var result = new ClientCredentialsTokenRequestParameters();
        if (useChildOrganizationNumberOptionValue)
        {
            result.PayloadClaimParameters = new PayloadClaimParameters()
            {
                ChildOrganizationNumber = ConfigurationValues.ClientCredentialsWithChildOrganizationNumber,
            };
        }
        return result;
    }
}