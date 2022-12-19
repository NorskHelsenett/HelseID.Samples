using HelseId.Samples.Common.Configuration;

namespace HelseID.Samples.Configuration;

/// <summary>
/// This class contains configurations that correspond to existing clients in the HelseID TEST environment.
/// </summary>
public class HelseIdSamplesConfiguration : HelseIdConfiguration
{
    public HelseIdSamplesConfiguration(
                string rsaPrivateKeyJwk,
                string clientId,
                string scope,
                List<string>? resourceIndicators = null) :
            base(
                rsaPrivateKeyJwk,
                clientId,
                scope,
                ConfigurationValues.StsUrl,
                resourceIndicators) {}

    // Configuration for the 'plain' client credentials application
    public static HelseIdSamplesConfiguration ConfigurationForClientCredentialsClient =>
        new(
            ConfigurationValues.ClientCredentialsSampleRsaPrivateKeyJwk,
            ConfigurationValues.ClientCredentialsSampleClientId,
            ConfigurationValues.ClientCredentialsSampleScope);

    // Configuration for the client credentials application with underenhet
    public static HelseIdSamplesConfiguration ConfigurationForClientCredentialsWithUnderenhetClient =>
        new(
            ConfigurationValues.ClientCredentialsWithUnderenhetSamplePrivateKeyJwk,
            ConfigurationValues.ClientCredentialsWithUnderenhetSampleClientId,
            ConfigurationValues.ClientCredentialsSampleScope);

    // The configuration for the ApiAccess project (accesing the sample API)
    public static HelseIdSamplesConfiguration ConfigurationForApiAccess =>
        new(
            ConfigurationValues.ApiAccessSampleRsaPrivateKeyJwk,
            ConfigurationValues.ApiAccessSampleClientId,
            ConfigurationValues.ApiAccessSampleScope);            

    // The configuration for the ApiAccess project with request objects (accesing the sample API)
    public static HelseIdSamplesConfiguration ConfigurationForApiAccessWithRequestObject =>
        new(
            ConfigurationValues.ApiAccessWithRequestObjectSampleRsaPrivateKeyJwk,
            ConfigurationValues.ApiAccessSampleClientIdWithRequestObject,
            ConfigurationValues.ApiAccessWithRequestObjectSampleScope); 
            
    // The configuration for the ApiAccess project (accesing the token exchange client)
    public static HelseIdSamplesConfiguration ConfigurationForApiAccessWithTokenExchange =>
        new(
            ConfigurationValues.TokenExchangeSubjectRsaPrivateKeyJwk,
            ConfigurationValues.TokenExchangeSubjectClientId,
            ConfigurationValues.TokenExchangeSubjectClientScope);            
    
    // The configuration for the the ApiAccess project with resource indicators
    // We add the audiences for the two APIs that this project will call as resource indicators
    public static HelseIdSamplesConfiguration ConfigurationForResourceIndicatorsClient =>
        new(
            ConfigurationValues.ApiAccessResourceIndicatorRsaPrivateKeyJwk,
            ConfigurationValues.ApiAccessResourceIndicatorClientId,
            ConfigurationValues.ApiAccessResourceIndicatorClientScope,
            resourceIndicators: new List<string>
            {
                ConfigurationValues.SampleApiForResourceIndicators1Audience,
                ConfigurationValues.SampleApiForResourceIndicators2Audience,
            });

    // The configuration for the SampleApiForTokenExchange project (used for accessing the SampleApi with the token exchange grant)
    public static HelseIdSamplesConfiguration ConfigurationForTokenExchangeClient =>
        new(
            ConfigurationValues.TokenExchangActorRsaPrivateKeyJwk,
            ConfigurationValues.TokenExchangeActorClientId,
            ConfigurationValues.TokenExchangeActorClientScope);

}
