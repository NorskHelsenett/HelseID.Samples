namespace HelseId.Samples.ApiAccess.Configuration;

public enum ClientType
{
    UserLoginOnly,
    ApiAccess,
    ApiAccessWithRequestObject,
    ApiAccessWithRequestObjectsWithContextualClaimsOption,
    ApiAccessWithTokenExchange,
    ApiAccessWithResourceIndicators,
    ApiAccessForMultiTenantClient,
    ApiAccessWithContextualClaims,
}