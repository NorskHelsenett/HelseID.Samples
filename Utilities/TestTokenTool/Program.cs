using CommandDotNet;
using Microsoft.Extensions.Configuration;
using TestTokenTool.ApiCalls;
using TestTokenTool.Commands;
using TestTokenTool.Configuration;
using TestTokenTool.Constants;
using TestTokenTool.InputParameters;
using TestTokenTool.RequestModel;
using TokenRequest = TestTokenTool.RequestModel.TokenRequest;

namespace TestTokenTool;

public class Program
{
    private static IConfigurationBuilder? _builder;
    
    static async Task Main(string[] args)
    {
        _builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", optional: false);

        var appRunner = new AppRunner<Program>();
        await appRunner.RunAsync(args);
    }

    [Command("getToken", Description = "Get a token from the test token service")] 
    public async Task GetToken(
        // -----------------------------------------
        // Output from application
        [Option('p', Description = "The returned token is printed in JWT format on screen", BooleanMode = BooleanMode.Explicit)]
        bool printToken = true,
        [Option(Description = "The returned token is printed in JSON format on screen", BooleanMode = BooleanMode.Implicit)]
        bool prettyPrintToken = false,
        [Option('s', Description = $"The returned token is saved to a file named '{FileConstants.TokenFileName}'", BooleanMode = BooleanMode.Implicit)]
        bool saveTokenToFile = false,
        // -----------------------------------------
        // Invalid output from TTT
        [Option(Description = "The returned token will be signed with an invalid signing key", BooleanMode = BooleanMode.Implicit)]
        bool signJwtWithInvalidSigningKey = false,
        [Option(Description = "The returned token will contain an invalid 'iss' claim", BooleanMode = BooleanMode.Implicit)]
        bool setInvalidIssuer = false,
        [Option(Description = "The returned token will contain an invalid 'aud' claim", BooleanMode = BooleanMode.Implicit)]
        bool setInvalidAudience = false,
        [Option(Description = "The returned token will contain an expired 'nbf', 'iat', and 'exp' claims", BooleanMode = BooleanMode.Implicit)]
        bool setExpirationTimeAsExpired = false,
        [Option(Description = "The returned token will contain an 'exp' claim matching the set expiration time")]
        int expirationTimeInSeconds = 600,
        // -----------------------------------------
        // Header parameters
        [Option(Description = "The returned token will contain a 'typ' header matching the injected value. Accepted values are 'jwt' and 'at+jwt'")]
        string? typ = "",
        // -----------------------------------------
        // General parameters
        [Option(Description = "The returned token will contain a 'client_amr' claim matching the injected value")]
        string? clientAmr = "",
        [Option(Description = "The returned token will contain a 'helseid://claims/client/amr' claim matching the injected value")]
        string? helseidClientAmr = "",
        [Option(Description = "The returned token will contain a 'helseid://claims/client/claims/orgnr_parent' claim matching the injected value")]
        string? orgnrParent = "",
        [Option(Description = "The returned token will contain a 'client_id' claim matching the injected value")]
        Guid? clientId = default,
        [Option(Description = "The returned token will contain a 'helseid://claims/client/client_name' claim matching the injected value")]
        string? clientName = "",
        [Option(Description = "The returned token will contain a 'scope' claim matching the injected value. Use quotes and spaces to insert several scopes.")]
        string? scope = "",
        // -----------------------------------------
        // User parameters
        [Option(Description = "The returned token will contain an 'amr' claim matching the injected value")]
        string? amr = "",
        [Option(Description = "The returned token will contain an 'idp' claim matching the injected value")]
        string? identityProvider = "",
        [Option(Description = "The returned token will contain a 'helseid://claims/identity/assurance_level' claim matching the injected value")]
        string? assuranceLevel = "",
        [Option(Description = "The returned token will contain a 'helseid://claims/identity/security_level' claim matching the injected value")]
        string? securityLevel = "",
        [Option(Description = "The returned token will contain a claim with a pseudonymized pid value. Requires that the 'pid' parameter is set.", BooleanMode = BooleanMode.Implicit)]
        bool setPidPseudonym = false,
        [Option(Description = "The returned token will contain a 'pid' claim matching the injected value")]
        string? pid = "",
        [Option(Description = "If this value is set, the PID value will be used to extract person information from Persontjenesten. Requires that the 'pid' parameter is set.", BooleanMode = BooleanMode.Implicit)]
        bool getPersonFromPersontjenesten = false,
        [Option(Description = "The returned token will contain a 'helseid://claims/identity/pid_pseudonym' claim matching the injected value. If setPidPseudonym is true, this option will be overridden.")]
        string? pidPseudonym = "",
        [Option(Description = "The returned token will contain a 'name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.")]
        string? name = "",
        [Option(Description = "If this value is set, the PID value will be used to extract person information from HPR-registeret.", BooleanMode = BooleanMode.Implicit)]
        bool getHprNumberFromHprregisteret = false,
        [Option(Description = "The returned token will contain a 'helseid://claims/hpr/hpr_number' claim matching the injected value. Requires that the 'getHprNumberFromHprregisteret' parameter is not set.")]
        string? hprNumber = "",
        [Option(Description = "The returned token will contain a 'network' claim matching the injected value")]
        string? network = "",
        [Option(Description = "The returned token will contain a 'sid' claim matching the injected value")]
        string? sid = "",
        [Option(Description = "The returned token will contain a 'sub' claim matching the injected value")]
        string? sub = "",
        // -----------------------------------------
        // Tillitsrammeverk parameters
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string legalEntityId = "",
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string legalEntityName = "",
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string pointOfCareId = "",
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string pointOfCareName = "",
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string practitionerAuthorizationCode = "",
        [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
        string practitionerAuthorizationText = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipDepartmentId = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipDepartmentName = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipHealthcareServiceCode = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipHealthcareServiceText = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipPurposeOfUseCode = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipPurposeOfUseText = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipPurposeOfUseDetailsCode = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipPurposeOfUseDetailsText = "",
        [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
        string careRelationshipTracingRefId = "",
        // -----------------------------------------
        // Usage of parameters
        [Option(Description = "Instructs how common claims are created")]
        ClaimGeneration generalClaimsCreation = ClaimGeneration.DefaultWithParameterValues,
        [Option(Description = "Instructs how user claims are created")]
        ClaimGeneration userClaimsCreation = ClaimGeneration.DefaultWithParameterValues,
        [Option(Description = "Create claims for dokumentdeling", BooleanMode = BooleanMode.Implicit)]
        bool createDokumentdelingClaims = false)
    {
        
        if (!File.Exists(FileConstants.JwkFileName))
        {
            Console.WriteLine("Ooops! The private key for usage with HelseID is missing. [Read the README.md file for how to create this key and use its public equivalent in HelseID Selvbetjening.]");
            return;
        }
        
        // På tjener: hvis getHprNumberFraRegister er satt, ikke skriv hprnummer først
        
        var tokenRequest = new TokenRequest
        {
            SignJwtWithInvalidSigningKey = signJwtWithInvalidSigningKey,
            SetInvalidIssuer = setInvalidIssuer,
            SetInvalidAudience = setInvalidAudience,
            SetExpirationTimeAsExpired = setExpirationTimeAsExpired,
            ExpirationTimeInSeconds = expirationTimeInSeconds,
            SetPidPseudonym = setPidPseudonym,
            GeneralClaimsParametersGeneration = generalClaimsCreation.ToParametersGeneration(),
            UserClaimsParametersGeneration = userClaimsCreation.ToParametersGeneration(),
            CreateDokumentdelingClaims = createDokumentdelingClaims,
            HeaderParameters = new HeaderParameters()
            {
                Typ = typ.GetEmptyStringIfNotSet(), 
            },
            GeneralClaimsParameters = new GeneralParameters
            {
                AuthenticationMethodsReferences = clientAmr.GetListWithParameter(),
                ClientAuthenticationMethodsReferences = helseidClientAmr.GetEmptyStringIfNotSet(),
                OrgnrParent = orgnrParent.GetEmptyStringIfNotSet(),
                ClientId = clientId != null ? clientId.ToString()! : string.Empty,
                ClientName = clientName.GetEmptyStringIfNotSet(),
                Scope = scope.GetListWithMultipleParameters(),
            },
            UserClaimsParameters = new UserClaimsParameters
            {
                Pid = pid.GetEmptyStringIfNotSet(),
                Network = network.GetEmptyStringIfNotSet(),
                // Service: set error when this is set for GetPersonFromPersontjenesten
                Name = name.GetEmptyStringIfNotSet(),
                Sid = sid.GetEmptyStringIfNotSet(),
                Subject = sub.GetEmptyStringIfNotSet(),
                AssuranceLevel = assuranceLevel.GetEmptyStringIfNotSet(),
                SecurityLevel = securityLevel.GetEmptyStringIfNotSet(),
                HprNumber = hprNumber.GetEmptyStringIfNotSet(),
                IdentityProvider = identityProvider.GetEmptyStringIfNotSet(),
                PidPseudonym = pidPseudonym.GetEmptyStringIfNotSet(),
                Amr = amr.GetEmptyStringIfNotSet(),
            },
            TillitsrammeverkClaimsParameters = new TillitsrammeverkClaimsParameters
            {
                PractitionerAuthorizationCode = practitionerAuthorizationCode,
                PractitionerAuthorizationText = practitionerAuthorizationText, 
                LegalEntityId = legalEntityId,
                LegalEntityName = legalEntityName,
                PointOfCareId = pointOfCareId,
                PointOfCareName = pointOfCareName,
            },
            DokumentdelingClaimsParameters = new DokumentdelingClaimsParameters
            {
              CareRelationshipDepartmentId  = careRelationshipDepartmentId,
              CareRelationshipDepartmentName = careRelationshipDepartmentName,
              CareRelationshipHealthcareServiceCode = careRelationshipHealthcareServiceCode,
              CareRelationshipHealthcareServiceText = careRelationshipHealthcareServiceText,
              CareRelationshipPurposeOfUseCode = careRelationshipPurposeOfUseCode,
              CareRelationshipPurposeOfUseText = careRelationshipPurposeOfUseText,
              CareRelationshipPurposeOfUseDetailsCode = careRelationshipPurposeOfUseDetailsCode,
              CareRelationshipPurposeOfUseDetailsText = careRelationshipPurposeOfUseDetailsText, 
              CareRelationshipTracingRefId = careRelationshipTracingRefId,
            },
            GetPersonFromPersontjenesten = getPersonFromPersontjenesten,
            GetHprNumberFromHprregisteret = getHprNumberFromHprregisteret,
        };

        var tokenResponse = await TokenRetriever.GetToken(_builder!, tokenRequest);

        var parameters = new Parameters()
        {
            PrintJwt = printToken,
            PrettyPrintJwt = prettyPrintToken,
            SaveTokenToFile = saveTokenToFile,
        };
        TokenPrinter.WriteResponse(tokenResponse, parameters);
        IApiCaller apiCaller = new ApiCaller();
        await apiCaller.CallApi(tokenResponse, parameters);        
    }

    [Command("createKeys", Description = "Create a new public/private key pair for authentication with HelseID")]
    public async Task CreateKeys()
    {
        await JwkGenerator.GenerateKey();
    }
}

internal static class StringExtensions
{
    public static string GetEmptyStringIfNotSet(this string? aString)
    {
        return aString ?? string.Empty;
    }

    public static List<string> GetListWithParameter(this string? aString)
    {
        return !string.IsNullOrEmpty(aString) ? new List<string>() { aString } : new List<string>();
    }
    
    public static List<string> GetListWithMultipleParameters(this string? aString)
    {
        return string.IsNullOrEmpty(aString) ?
            new List<string>() :
            aString.Split(' ').ToList();
    }
}

