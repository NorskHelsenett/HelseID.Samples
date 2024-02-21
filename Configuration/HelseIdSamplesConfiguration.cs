using HelseId.Samples.Common.Configuration;

namespace HelseID.Samples.Configuration;

/// <summary>
/// This class contains configurations that correspond to existing clients in the HelseID TEST environment.
/// </summary>
public class HelseIdSamplesConfiguration : HelseIdConfiguration
{
    private HelseIdSamplesConfiguration(
                SecurityKey privateKeyJwk,
                string clientId,
                string scope,
                List<string>? resourceIndicators = null) :
            base(
                privateKeyJwk,
                clientId,
                scope,
                ConfigurationValues.StsUrl,
                resourceIndicators) {}

    // Configuration for the 'plain' client credentials application
    public static HelseIdSamplesConfiguration ClientCredentialsClient =>
        new(
            ConfigurationValues.ClientCredentialsSampleRsaPrivateKeyJwk,
            ConfigurationValues.ClientCredentialsSampleClientId,
            ConfigurationValues.ClientCredentialsSampleScope);

    // Configuration for the client credentials application with child organization number
    public static HelseIdSamplesConfiguration ClientCredentialsWithChildOrgNumberClient =>
        new(
            ConfigurationValues.ClientCredentialsWithChildOrgNumberSamplePrivateKeyJwk,
            ConfigurationValues.ClientCredentialsWithChildOrgNumberSampleClientId,
            ConfigurationValues.ClientCredentialsSampleScope);

    // Configuration for the client credentials application with multi-tenancy
    public static HelseIdSamplesConfiguration ClientCredentialsSampleForMultiTenantClient =>
        new(
            ConfigurationValues.ClientCredentialsSampleForMultiTenantPrivateKeyJwk,
            ConfigurationValues.ClientCredentialsSampleForMultiTenantClientId,
            ConfigurationValues.ClientCredentialsSampleScope);

    // The configuration for the ApiAccess project in logon only (no API access) mode
    public static HelseIdSamplesConfiguration UserAuthenticationClient =>
        new(
            ConfigurationValues.UserAuthenticationPrivateKeyJwk,
            ConfigurationValues.UserAuthenticationClientId,
            ConfigurationValues.UserAuthenticationClientScope);

    // The configuration for the ApiAccess project (accessing the sample API)
    public static HelseIdSamplesConfiguration ApiAccess =>
        new(
            ConfigurationValues.ApiAccessSampleRsaPrivateKeyJwk,
            ConfigurationValues.ApiAccessSampleClientId,
            ConfigurationValues.ApiAccessSampleScope);

    // The configuration for the ApiAccess project with request objects (accessing the sample API)
    public static HelseIdSamplesConfiguration ApiAccessWithRequestObject =>
        new(
            ConfigurationValues.ApiAccessWithRequestObjectSampleRsaPrivateKeyJwk,
            ConfigurationValues.ApiAccessSampleClientIdWithRequestObject,
            ConfigurationValues.ApiAccessWithRequestObjectSampleScope);

    // Configuration for the ApiAccess project with multi-tenancy
    public static HelseIdSamplesConfiguration ApiAccessForMultiTenantClient =>
        new(
            ConfigurationValues.ApiAccessSampleForMultiTenantPrivateKeyJwk,
            ConfigurationValues.ApiAccessSampleClientIdForMultiTenantApp,
            ConfigurationValues.ApiAccessSampleScopeForMultiTenantApp);

    // The configuration for the ApiAccess project (accessing the token exchange client)
    public static HelseIdSamplesConfiguration ApiAccessWithTokenExchange =>
        new(
            ConfigurationValues.TokenExchangeSubjectRsaPrivateKeyJwk,
            ConfigurationValues.TokenExchangeSubjectClientId,
            ConfigurationValues.TokenExchangeSubjectClientScope);

    // The configuration for the the ApiAccess project with resource indicators
    // We add the audiences for the two APIs that this project will use as resource indicators
    public static HelseIdSamplesConfiguration ResourceIndicatorsClient =>
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
    public static HelseIdSamplesConfiguration TokenExchangeClient =>
        new(
            ConfigurationValues.TokenExchangeActorRsaPrivateKeyJwk,
            ConfigurationValues.TokenExchangeActorClientId,
            ConfigurationValues.TokenExchangeActorClientScope);
}
