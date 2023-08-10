using CommandDotNet;
using TestTokenTool.Constants;
using TestTokenTool.InputParameters;

// ReSharper disable InconsistentNaming

namespace TestTokenTool.Options;

public class GetTokenOptions : IArgumentModel
{
    // -----------------------------------------
    // Output from application
    [Option('p', Description = "The returned token is printed in JWT format on screen", BooleanMode = BooleanMode.Explicit)]
    public bool printToken { get; set; } = true;
    [Option(Description = "The returned token is printed in JSON format on screen")]
    public bool prettyPrintToken { get; set; } = false;
    [Option('s', Description = $"The returned token is saved to a file named '{FileConstants.TokenFileName}'")]
    public bool saveTokenToFile { get; set; } = false;
    // -----------------------------------------
    // Invalid output from TTT
    [Option(Description = "The returned token will be signed with an invalid signing key")]
    public bool signJwtWithInvalidSigningKey { get; set; } = false;
    [Option(Description = "The returned token will contain an invalid 'iss' claim")]
    public bool setInvalidIssuer { get; set; } = false;
    [Option(Description = "The returned token will contain an invalid 'aud' claim")]
    public bool setInvalidAudience { get; set; } = false;
    [Option(Description = "The returned token will contain an expired 'nbf', 'iat', and 'exp' claims")]
    public bool setExpirationTimeAsExpired { get; set; } = false;
    [Option(Description = "The returned token will contain an 'exp' claim matching the set expiration time")]
    public int expirationTimeInSeconds { get; set; } = 600;
    // -----------------------------------------
    // Header parameters
    [Option(Description = "The returned token will contain a 'typ' header matching the injected value. Accepted values are 'jwt' and 'at+jwt'")]
    public string? typHeader { get; set; } = string.Empty;
    // -----------------------------------------
    // General parameters
    [Option(Description = "The returned token will contain a 'client_amr' claim matching the injected value")]
    public string? clientAmr { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'helseid://claims/client/amr' claim matching the injected value")]
    public string? helseidClientAmr { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'helseid://claims/client/claims/orgnr_parent' claim matching the injected value")]
    public string? orgnrParent { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'client_id' claim matching the injected value")]
    public Guid? clientId { get; set;} = default;
    [Option(Description = "The returned token will contain a 'helseid://claims/client/client_name' claim matching the injected value")]
    public string? clientName { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'scope' claim matching the injected value. Use quotes and spaces to insert several scopes.")]
    public string? scope { get; set; } = string.Empty;
    // -----------------------------------------
    // User parameters
    [Option(Description = "The returned token will contain an 'amr' claim matching the injected value")]
    public string? amr { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain an 'idp' claim matching the injected value")]
    public string? identityProvider { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'helseid://claims/identity/assurance_level' claim matching the injected value")]
    public string? assuranceLevel { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'helseid://claims/identity/security_level' claim matching the injected value")]
    public string? securityLevel { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a claim with a pseudonymized pid value. Requires that the 'pid' parameter is set.")]
    public bool setPidPseudonym { get; set; } = false;
    [Option(Description = "The returned token will contain a claim with a pseudonymized pid value in the 'sub' claim. Requires that both the 'pid' and 'clientid' parameters are set.")]
    public bool setSubject { get; set; } = false;
    [Option(Description = "The returned token will contain a 'pid' claim matching the injected value")]
    public string? pid { get; set; } = string.Empty;
    [Option(Description = "If this value is set, the PID value will be used to extract person information from Persontjenesten. Requires that the 'pid' parameter is set.")]
    public bool getPersonFromPersontjenesten { get; set; } = false;
    [Option(Description = "If this value and getPersonFromPersontjenesten are set, only the 'name' claim will be issued, not the claims 'given_name', 'middle_name', or 'family_name'.")]
    public bool onlySetNameForPerson { get; set; } = false;
    [Option(Description = "The returned token will contain a 'helseid://claims/identity/pid_pseudonym' claim matching the injected value. If setPidPseudonym is true, this option will be overridden.")]
    public string? pidPseudonym { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.")]
    public string? name { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'given_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.")]
    public string? given_name { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'middle_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.")]
    public string? middle_name { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'family_name' claim matching the injected value. Requires that the 'getPersonFromPersontjenesten' parameter is not set.")]
    public string? family_name { get; set; } = string.Empty;
    [Option(Description = "If this value is set, the PID value will be used to extract person information from HPR-registeret.")]
    public bool getHprNumberFromHprregisteret { get; set; } = false;
    [Option(Description = "The returned token will contain a 'helseid://claims/hpr/hpr_number' claim matching the injected value. Requires that the 'getHprNumberFromHprregisteret' parameter is not set.")]
    public string? hprNumber { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'network' claim matching the injected value")]
    public string? network { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'sid' claim matching the injected value")]
    public string? sid { get; set; } = string.Empty;
    [Option(Description = "The returned token will contain a 'sub' claim matching the injected value")]
    public string? sub { get; set; } = string.Empty;
    // -----------------------------------------
    // Tillitsrammeverk parameters
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string legalEntityId { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string legalEntityName { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string pointOfCareId { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string pointOfCareName { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string practitionerAuthorizationCode { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'tillitsrammeverk' claims")]
    public string practitionerAuthorizationText { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipDepartmentId { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipDepartmentName { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipHealthcareServiceCode { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipHealthcareServiceText { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipPurposeOfUseCode { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipPurposeOfUseText { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipPurposeOfUseDetailsCode { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipPurposeOfUseDetailsText { get; set; } = string.Empty;
    [Option(Description = "Parameter for use in 'dokumentdeling' claims")]
    public string careRelationshipTracingRefId { get; set; } = string.Empty;
    // -----------------------------------------
    // Usage of parameters
    [Option(Description = "Instructs how common claims are created")]
    public ClaimGeneration generalClaimsCreation { get; set; } = ClaimGeneration.DefaultWithParameterValues;
    [Option(Description = "Instructs how user claims are created")]
    public ClaimGeneration userClaimsCreation { get; set; } = ClaimGeneration.DefaultWithParameterValues;
    [Option(Description = "Create claims for dokumentdeling")]
    public bool createDokumentdelingClaims { get; set; } = false;
}