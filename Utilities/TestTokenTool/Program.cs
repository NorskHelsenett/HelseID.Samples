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
                SkipArityValidation = true,
                DefaultArgumentMode = ArgumentMode.Operand,
            },
            Help = new AppHelpSettings
            {
                TextStyle = HelpTextStyle.Detailed,
                UsageAppName = "dotnet run",
            }
        };
        var appRunner = new AppRunner<Program>(appSettings);
        appRunner.UseTypoSuggestions();
        await appRunner.RunAsync(args);
    }

    [Command("getToken", Description = "Get a token from the test token service")] 
    public async Task GetToken(GetTokenOptions options)
    {
        
        if (!File.Exists(FileConstants.JwkFileName))
        {
            Console.WriteLine("Ooops! The private key for usage with HelseID is missing. [Read the README.md file for how to create this key and use its public equivalent in HelseID Selvbetjening.]");
            return;
        }
        
        var tokenRequest = new TokenRequest
        {
            // Invalid output from TTT
            SignJwtWithInvalidSigningKey =      options.signJwtWithInvalidSigningKey,
            SetInvalidIssuer =                  options.setInvalidIssuer,
            SetInvalidAudience =                options.setInvalidAudience,
            SetExpirationTimeAsExpired =        options.setExpirationTimeAsExpired,
            ExpirationTimeInSeconds =           options.expirationTimeInSeconds,
            // User parameters
            SetPidPseudonym =                   options.setPidPseudonym,
            SetSubject =                        options.setSubject,
            GetPersonFromPersontjenesten =      options.getPersonFromPersontjenesten,
            OnlySetNameForPerson =              options.onlySetNameForPerson,
            GetHprNumberFromHprregisteret =     options.getHprNumberFromHprregisteret,
            // Usage of parameters
            GeneralClaimsParametersGeneration = options.generalClaimsCreation.ToParametersGeneration(),
            UserClaimsParametersGeneration =    options.userClaimsCreation.ToParametersGeneration(),
            CreateDokumentdelingClaims =        options.createDokumentdelingClaims,
            CreateDPoPTokenWithDPoPProof =      options.createDPoPTokenWithDPoPProof,
            HeaderParameters = new HeaderParameters()
            {
                Typ = options.typHeader.GetEmptyStringIfNotSet(), 
            },
            GeneralClaimsParameters = new GeneralParameters
            {
                AuthenticationMethodsReferences = options.clientAmr.GetListWithParameter(),
                ClientAuthenticationMethodsReferences = options.helseidClientAmr.GetEmptyStringIfNotSet(),
                OrgnrParent = options.orgnrParent.GetEmptyStringIfNotSet(),
                ClientId = options.clientId != null ? options.clientId.ToString()! : string.Empty,
                ClientName = options.clientName.GetEmptyStringIfNotSet(),
                Scope = options.scope.GetListWithMultipleParameters(),
            },
            UserClaimsParameters = new UserClaimsParameters
            {
                Pid = options.pid.GetEmptyStringIfNotSet(),
                Network = options.network.GetEmptyStringIfNotSet(),
                Name = options.name.GetEmptyStringIfNotSet(),
                GivenName = options.given_name.GetEmptyStringIfNotSet(),
                MiddleName = options.middle_name.GetEmptyStringIfNotSet(),
                FamilyName = options.family_name.GetEmptyStringIfNotSet(),
                Sid = options.sid.GetEmptyStringIfNotSet(),
                Subject = options.sub.GetEmptyStringIfNotSet(),
                AssuranceLevel = options.assuranceLevel.GetEmptyStringIfNotSet(),
                SecurityLevel = options.securityLevel.GetEmptyStringIfNotSet(),
                HprNumber = options.hprNumber.GetEmptyStringIfNotSet(),
                IdentityProvider = options.identityProvider.GetEmptyStringIfNotSet(),
                PidPseudonym = options.pidPseudonym.GetEmptyStringIfNotSet(),
                Amr = options.amr.GetEmptyStringIfNotSet(),
            },
            TillitsrammeverkClaimsParameters = new TillitsrammeverkClaimsParameters
            {
                PractitionerAuthorizationCode = options.practitionerAuthorizationCode,
                PractitionerAuthorizationText = options.practitionerAuthorizationText, 
                LegalEntityId = options.legalEntityId,
                LegalEntityName = options.legalEntityName,
                PointOfCareId = options.pointOfCareId,
                PointOfCareName = options.pointOfCareName,
            },
            DokumentdelingClaimsParameters = new DokumentdelingClaimsParameters
            {
                CareRelationshipDepartmentId  = options.careRelationshipDepartmentId,
                CareRelationshipDepartmentName = options.careRelationshipDepartmentName,
                CareRelationshipHealthcareServiceCode = options.careRelationshipHealthcareServiceCode,
                CareRelationshipHealthcareServiceText = options.careRelationshipHealthcareServiceText,
                CareRelationshipPurposeOfUseCode = options.careRelationshipPurposeOfUseCode,
                CareRelationshipPurposeOfUseText = options.careRelationshipPurposeOfUseText,
                CareRelationshipPurposeOfUseDetailsCode = options.careRelationshipPurposeOfUseDetailsCode,
                CareRelationshipPurposeOfUseDetailsText = options.careRelationshipPurposeOfUseDetailsText, 
                CareRelationshipTracingRefId = options.careRelationshipTracingRefId,
            },
            DPoPProofParameters = new DPoPProofParameters
            {
                HtmClaimValue = options.htmClaimValue,
                HtuClaimValue = options.htuClaimValue,
                PrivateKeyForProofCreation = options.privateKeyForProofCreation,
                InvalidDPoPProofParameters = options.invalidDPoPProof,
            }
        };

        var tokenResponse = await TokenRetriever.GetToken(_builder!, tokenRequest);

        var parameters = new Parameters()
        {
            PrintJwt = options.printToken,
            PrettyPrintJwt = options.prettyPrintToken,
            SaveTokenToFile = options.saveTokenToFile,
            UseDPoP = options.createDPoPTokenWithDPoPProof,
            CallApi = options.callApi,
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