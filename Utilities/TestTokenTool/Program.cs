using CommandDotNet;
using CommandDotNet.Help;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TestTokenTool.ApiCalls;
using TestTokenTool.Commands;
using TestTokenTool.Configuration;
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
            .AddJsonFile("config.json", optional: false)
            .AddJsonFile("config.development.json", optional: true);

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
        var tokenRequest = new TokenRequest
        {
            Audience = options.audience!,
            // Invalid output from TTT
            SignJwtWithInvalidSigningKey =      options.signJwtWithInvalidSigningKey,
            SetInvalidIssuer =                  options.setInvalidIssuer,
            SetInvalidAudience =                options.setInvalidAudience,
            // User parameters
            SetPidPseudonym =                   options.setPidPseudonym,
            SetSubject =                        options.setSubject,
            GetPersonFromPersontjenesten =      options.getPersonFromPersontjenesten,
            OnlySetNameForPerson =              options.onlySetNameForPerson,
            GetHprNumberFromHprregisteret =     options.getHprNumberFromHprregisteret,
            // Usage of parameters
            IssuerEnvironment =                 options.issuerEnvironment,
            WithoutDefaultClientClaims =        options.withoutDefaultClientClaims,
            WithoutDefaultUserClaims =          options.withoutDefaultUserClaims,
            ClientClaimsParametersGeneration =  options.clientClaimsCreation.ToParametersGeneration(),
            UserClaimsParametersGeneration =    options.userClaimsCreation.ToParametersGeneration(),
            CreateTillitsrammeverkClaims =      options.createTillitsrammeverkClaims,
            CreateDPoPTokenWithDPoPProof =      options.createDPoPTokenWithDPoPProof,
            ExpirationParameters = new ExpirationParameters()
            {
                SetExpirationTimeAsExpired =    options.setExpirationTimeAsExpired,
                ExpirationTimeInSeconds =       options.expirationTimeInSeconds,
                ExpirationTimeInDays =          options.expirationTimeInDays == 0 ? Int32.MinValue : options.expirationTimeInDays,
            },
            HeaderParameters = new HeaderParameters()
            {
                Typ = options.typHeader.GetEmptyStringIfNotSet(),
            },
            ClientClaimsParameters = new ClientClaimsParameters
            {
                AuthenticationMethodsReferences = options.clientAmr.GetListWithParameter(),
                ClientAuthenticationMethodsReferences = options.helseidClientAmr.GetEmptyStringIfNotSet(),
                OrgnrParent = options.orgnrParent.GetEmptyStringIfNotSet(),
                OrgnrChild = options.orgnrChild.GetEmptyStringIfNotSet(),
                OrgnrSupplier = options.orgnrSupplier.GetEmptyStringIfNotSet(),
                ClientTenancy = options.clientTenancy,
                ClientTenancyType = options.clientTenancyType,
                SfmJournalId = options.sfmJournalId != null ? options.sfmJournalId.ToString()! : string.Empty,
                ClientId = options.clientId != null ? options.clientId.ToString()! : string.Empty,
                ClientName = options.clientName.GetEmptyStringIfNotSet(),
                Scope = options.scope.GetListWithMultipleParameters(),
                CnfJkt = options.cnfJkt.GetEmptyStringIfNotSet(),
                CnfPublicKey = options.cnfPublicKey.GetEmptyStringIfNotSet(),
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
                DontSetPractitionerAuthorization = options.dontSetPractitionerAuthorization,
                DontSetPractitionerDepartment = options.dontSetPractitionerDepartment,
                DontSetPractitionerHprNr = options.dontSetPractitionerHprNr,
                DontSetCareRelationshipPurposeOfUseDetails = options.dontSetCareRelationshipPurposeOfUseDetails,
                DontSetCareRelationshipHealthcareService = options.dontSetCareRelationshipHealthcareService,
                DontSetPatientsDepartment = options.dontSetPatientsDepartment,
                DontSetPatientsPointOfCare = options.dontSetPatientsPointOfCare,
                
                PractitionerIdentifierSystem = options.practitionerIdentifierSystem,
                PractitionerIdentifierAuthority = options.practitionerIdentifierAuthority,
                
                PractitionerHprNrSystem = options.practitionerHprNrSystem,
                PractitionerHprNrAuthority = options.practitionerHprNrAuthority,
                
                PractitionerAuthorizationCode = options.practitionerAuthorizationCode,
                PractitionerAuthorizationText = options.practitionerAuthorizationText,
                PractitionerAuthorizationSystem = options.practitionerAuthorizationSystem,
                PractitionerAuthorizationAssigner = options.practitionerAuthorizationAssigner, 
                
                PractitionerLegalEntityId = options.practitionerLegalEntityId,
                PractitionerLegalEntityName = options.practitionerLegalEntityName,
                PractitionerLegalEntitySystem = options.practitionerLegalEntitySystem,
                PractitionerLegalEntityAuthority = options.practitionerLegalEntityAuthority,
                
                PractitionerPointOfCareId = options.practitionerPointOfCareId,
                PractitionerPointOfCareName = options.practitionerPointOfCareName,
                PractitionerPointOfCareSystem = options.practitionerPointOfCareSystem,
                PractitionerPointOfCareAuthority = options.practitionerPointOfCareAuthority,
                
                PractitionerDepartmentId  = options.practitionerDepartmentId,
                PractitionerDepartmentName = options.practitionerDepartmentName,
                PractitionerDepartmentSystem = options.practitionerDepartmentSystem,
                PractitionerDepartmentAuthority = options.practitionerDepartmentAuthority,
                
                CareRelationshipHealthcareServiceCode = options.careRelationshipHealthcareServiceCode,
                CareRelationshipHealthcareServiceText = options.careRelationshipHealthcareServiceText,
                CareRelationshipHealthcareSystem = options.careRelationshipHealthcareSystem,
                CareRelationshipHealthcareAssigner = options.careRelationshipHealthcareAssigner,
                
                CareRelationshipPurposeOfUseCode = options.careRelationshipPurposeOfUseCode,
                CareRelationshipPurposeOfUseText = options.careRelationshipPurposeOfUseText,
                CareRelationshipPurposeOfUseSystem = options.careRelationshipPurposeOfUseSystem,
                CareRelationshipPurposeOfUseAssigner = options.careRelationshipPurposeOfUseAssigner,
                
                CareRelationshipPurposeOfUseDetailsCode = options.careRelationshipPurposeOfUseDetailsCode,
                CareRelationshipPurposeOfUseDetailsText = options.careRelationshipPurposeOfUseDetailsText,
                CareRelationshipPurposeOfUseDetailsSystem = options.careRelationshipPurposeOfUseDetailsSystem,
                CareRelationshipPurposeOfUseDetailsAssigner = options.careRelationshipPurposeOfUseDetailsAssigner,
                    
                CareRelationshipDecisionRefId = options.careRelationshipDecisionRefId,
                CareRelationshipDecisionRefUserSelected = options.careRelationshipDecisionRefUserSelected,
                
                PatientsPointOfCareId = options.patientsPointOfCareId,
                PatientsPointOfCareName = options.patientsPointOfCareName,
                PatientsPointOfCareSystem = options.patientsPointOfCareSystem,
                PatientsPointOfCareAuthority = options.patientsPointOfCareAuthority,
                
                PatientsDepartmentId = options.patientsDepartmentId,
                PatientsDepartmentName = options.patientsDepartmentName,
                PatientsDepartmentSystem = options.patientsDepartmentSystem,
                PatientsDepartmentAuthority = options.patientsDepartmentAuthority,
            },
            DPoPProofParameters = new DPoPProofParameters
            {
                HtmClaimValue = options.htmClaimValue,
                HtuClaimValue = options.htuClaimValue,
                PrivateKeyForProofCreation = options.privateKeyForProofCreation,
                InvalidDPoPProofParameters = options.invalidDPoPProof,
            },
            ApiSpecificClaims = AddApiSpecificClaims(options.apiSpecificClaimType, options.apiSpecificClaimValue),
            CreateTokenExchangeClaims = options.createTokenExchangeClaim,
            TokenExchangeParameters = new TokenExchangeParameters
            {
                ClientId = options.tokenExchangeClientId,
                Issuer = options.tokenExchangeIssuer,
                OrgnrParent = options.tokenExchangeOrgnrParent,
            },
        };

        var tokenResponse = await TokenRetriever.GetToken(_builder!, tokenRequest);

        var parameters = new Parameters()
        {
            PrintJwt = options.printToken,
            PrettyPrintJwt = options.prettyPrintToken,
            SaveTokenToFile = options.saveTokenToFile,
            UseDPoP = options.createDPoPTokenWithDPoPProof,
            CallApi = options.callApi,
            ReuseDPoPProof = options.reuseDpopProofForApi,
        };
        TokenPrinter.WriteResponse(tokenResponse, parameters);
        IApiCaller apiCaller = new ApiCaller();
        await apiCaller.CallApi(tokenResponse, parameters);
    }

    private static ApiSpecificClaim[]? AddApiSpecificClaims(string type, string value)
    {
        if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(value))
        {
            return [new ApiSpecificClaim{Value = value, Type = type}];
        }
        return [];
    }
}
