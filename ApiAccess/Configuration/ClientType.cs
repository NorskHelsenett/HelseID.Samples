namespace HelseId.Samples.ApiAccess.Configuration;

public enum ClientType
{
    UserLoginOnly,
    ApiAccess,
    ApiAccessWithRequestObject,
    ApiAccessWithTokenExchange,
    ApiAccessWithResourceIndicators,
    ApiAccessForMultiTenantClient,
    ApiAccessWithoutDPoP,
}
