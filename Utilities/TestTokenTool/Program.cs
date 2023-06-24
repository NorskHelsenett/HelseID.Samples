using CommandDotNet;
using CommandDotNet.Help;
using Microsoft.Extensions.Configuration;
using TestTokenTool.ApiCalls;
using TestTokenTool.Commands;
using TestTokenTool.Configuration;
using TestTokenTool.Constants;
using TestTokenTool.InputParameters;
using TestTokenTool.Options;
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

        var appSettings = new AppSettings
        {
            Parser = new ParseAppSettings
            {
                AllowSingleHyphenForLongNames = true,
            },
            Arguments = new ArgumentAppSettings
            {
                BooleanMode = BooleanMode.Implicit,
            },
            Help = new AppHelpSettings
            {
                TextStyle = HelpTextStyle.Basic,
                UsageAppName = "dotnet run",
            }
        };
        var appRunner = new AppRunner<Program>(appSettings);
        appRunner.UseTypoSuggestions();
        await appRunner.RunAsync(args);
    }

    [Command("getToken", Description = "Get a token from the test token service")] 
    public async Task GetToken(GetTokenOptions getTokenOptions)
    {
        
        if (!File.Exists(FileConstants.JwkFileName))
        {
            Console.WriteLine("Ooops! The private key for usage with HelseID is missing. [Read the README.md file for how to create this key and use its public equivalent in HelseID Selvbetjening.]");
            return;
        }
        
        var tokenRequest = new TokenRequest
        {
            SignJwtWithInvalidSigningKey = getTokenOptions.signJwtWithInvalidSigningKey,
            SetInvalidIssuer = getTokenOptions.setInvalidIssuer,
            SetInvalidAudience = getTokenOptions.setInvalidAudience,
            SetExpirationTimeAsExpired = getTokenOptions.setExpirationTimeAsExpired,
            ExpirationTimeInSeconds = getTokenOptions.expirationTimeInSeconds,
            SetPidPseudonym = getTokenOptions.setPidPseudonym,
            SetSubject = getTokenOptions.setSubject,
            GeneralClaimsParametersGeneration = getTokenOptions.generalClaimsCreation.ToParametersGeneration(),
            UserClaimsParametersGeneration = getTokenOptions.userClaimsCreation.ToParametersGeneration(),
            CreateDokumentdelingClaims = getTokenOptions.createDokumentdelingClaims,
            HeaderParameters = new HeaderParameters()
            {
                Typ = getTokenOptions.typHeader.GetEmptyStringIfNotSet(), 
            },
            GeneralClaimsParameters = new GeneralParameters
            {
                AuthenticationMethodsReferences = getTokenOptions.clientAmr.GetListWithParameter(),
                ClientAuthenticationMethodsReferences = getTokenOptions.helseidClientAmr.GetEmptyStringIfNotSet(),
                OrgnrParent = getTokenOptions.orgnrParent.GetEmptyStringIfNotSet(),
                ClientId = getTokenOptions.clientId != null ? getTokenOptions.clientId.ToString()! : string.Empty,
                ClientName = getTokenOptions.clientName.GetEmptyStringIfNotSet(),
                Scope = getTokenOptions.scope.GetListWithMultipleParameters(),
            },
            UserClaimsParameters = new UserClaimsParameters
            {
                Pid = getTokenOptions.pid.GetEmptyStringIfNotSet(),
                Network = getTokenOptions.network.GetEmptyStringIfNotSet(),
                Name = getTokenOptions.name.GetEmptyStringIfNotSet(),
                Sid = getTokenOptions.sid.GetEmptyStringIfNotSet(),
                Subject = getTokenOptions.sub.GetEmptyStringIfNotSet(),
                AssuranceLevel = getTokenOptions.assuranceLevel.GetEmptyStringIfNotSet(),
                SecurityLevel = getTokenOptions.securityLevel.GetEmptyStringIfNotSet(),
                HprNumber = getTokenOptions.hprNumber.GetEmptyStringIfNotSet(),
                IdentityProvider = getTokenOptions.identityProvider.GetEmptyStringIfNotSet(),
                PidPseudonym = getTokenOptions.pidPseudonym.GetEmptyStringIfNotSet(),
                Amr = getTokenOptions.amr.GetEmptyStringIfNotSet(),
            },
            TillitsrammeverkClaimsParameters = new TillitsrammeverkClaimsParameters
            {
                PractitionerAuthorizationCode = getTokenOptions.practitionerAuthorizationCode,
                PractitionerAuthorizationText = getTokenOptions.practitionerAuthorizationText, 
                LegalEntityId = getTokenOptions.legalEntityId,
                LegalEntityName = getTokenOptions.legalEntityName,
                PointOfCareId = getTokenOptions.pointOfCareId,
                PointOfCareName = getTokenOptions.pointOfCareName,
            },
            DokumentdelingClaimsParameters = new DokumentdelingClaimsParameters
            {
              CareRelationshipDepartmentId  = getTokenOptions.careRelationshipDepartmentId,
              CareRelationshipDepartmentName = getTokenOptions.careRelationshipDepartmentName,
              CareRelationshipHealthcareServiceCode = getTokenOptions.careRelationshipHealthcareServiceCode,
              CareRelationshipHealthcareServiceText = getTokenOptions.careRelationshipHealthcareServiceText,
              CareRelationshipPurposeOfUseCode = getTokenOptions.careRelationshipPurposeOfUseCode,
              CareRelationshipPurposeOfUseText = getTokenOptions.careRelationshipPurposeOfUseText,
              CareRelationshipPurposeOfUseDetailsCode = getTokenOptions.careRelationshipPurposeOfUseDetailsCode,
              CareRelationshipPurposeOfUseDetailsText = getTokenOptions.careRelationshipPurposeOfUseDetailsText, 
              CareRelationshipTracingRefId = getTokenOptions.careRelationshipTracingRefId,
            },
            GetPersonFromPersontjenesten = getTokenOptions.getPersonFromPersontjenesten,
            GetHprNumberFromHprregisteret = getTokenOptions.getHprNumberFromHprregisteret,
        };

        var tokenResponse = await TokenRetriever.GetToken(_builder!, tokenRequest);

        var parameters = new Parameters()
        {
            PrintJwt = getTokenOptions.printToken,
            PrettyPrintJwt = getTokenOptions.prettyPrintToken,
            SaveTokenToFile = getTokenOptions.saveTokenToFile,
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