namespace HelseID.Samples.Configuration;

public static class ConfigurationValues
{
    // Audience and scopes for using the 'regular' sample API:
    public const string SampleApiNameAudience = "nhn:helseid-public-samplecode";
    
    public const string ClientCredentialsScopeForSampleApi = $"{SampleApiNameAudience}/client-credentials";

    public const string AuthorizationCodeScopeForSampleApi = $"{SampleApiNameAudience}/authorization-code";

    // Audience and scope for using the sample API that uses token exchange:
    public const string SampleTokenExchangeApiAudience = "nhn:helseid-public-samplecode-for-token-exchange";

    public const string TokenExchangeApiScope = $"{SampleTokenExchangeApiAudience}/token-exhange-endpoint";

    // Audiences and scopes for using the sample API for the resource indicator sample:
    public const string SampleApiForResourceIndicators1Audience = "nhn:helseid-public-sample-api-1";
    public const string SampleApiForResourceIndicators2Audience = "nhn:helseid-public-sample-api-2";

    public const string SampleApiForResourceIndicators1Scope = $"{SampleApiForResourceIndicators1Audience}/some-scope";
    public const string SampleApiForResourceIndicators2Scope = $"{SampleApiForResourceIndicators2Audience}/some-scope";
    
    // The claim that is needed for use of contextual claims:
    public const string TestContextClaim = "helseid://claims/external/test-context";

    // HelseID security level claim
    public const string HelseIdSecurityLevelClaim = "helseid://claims/identity/security_level";

    // Port numbers for the different sample APIs
    public const int SampleApiPort = 5081;

    public const int SampleApiForTokenExchangePort = 5092;

    // Port number for the API Access project
    public const int ApiAccessWebServerPort = 5151;

    // URLS and resource parameters (url fragments) for the use of the sample APIs for the API access project
    public const string AuthCodeClientResource = "user-login-clients/greetings";
    public const string TokenExchangeResource = "token-exchange-clients/greetings";
    public const string ResourceIndicators1Resource = "resource-indicator-client-1/greetings";
    public const string ResourceIndicators2Resource = "resource-indicator-client-2/greetings";

    // URLS and resource parameter (url fragment) for the use of the sample API for the client credentials project
    public const string SampleApiMachineClientResource = "machine-clients/greetings";

    public static string SampleApiUrlForM2M = $"https://localhost:{SampleApiPort}/{SampleApiMachineClientResource}";

    // If used, the child organization number (underenhet) must match a number in the client's whitelist as it's held by HelseID:
    public const string ClientCredentialsWithChildOrganizationNumber = "999977776";
    public const string ApiAccessWithRequestObjectChildOrganizationNumber = "999977776";

    public const string GranfjelldalKommuneOrganizationNumber = "999977774";
    public const string GranfjelldalKommuneOrganizationName = "Granfjelldal kommune";
    public const string SupplierOrganizationNumber = "313745392";
    public const string SupplierOrganizationName = "Acme EPJ";
    public const string HansensLegekontorOrganizationNumber = "313325016";
    public const string HansensLegekontorOrganizationName = "Hansens legekontor AS";
    public const string FlaksvaagoeyKommuneOrganizationNumber = "312232324";
    public const string FlaksvaagoeyKommuneOrganizationName = "Flaksvågøy kommune";
    public const string FlaksvaagoeyKommuneChildOrganizationNumber = "312232326";
    public const string FlaksvaagoeyKommuneChildOrganizationName = "Flaksvågøy kommune bo- og omsorgssenter";
}