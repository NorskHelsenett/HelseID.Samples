namespace HelseId.Samples.Common.Models;

public static class HelseIdConstants {
    // A HelseID-specific parameter in the token response for the refresh token expiration time:
    public const string RefreshTokenExpiresIn = "rt_expires_in";
    
    // The HelseID specific name for the endpoint that gives out information about a client 
    public const string ClientInfoEndpointName = "clientinfo_endpoint";
    
    // The name of the claim for the client id in use for request objects
    public const string ClientIdClaimName = "client_id";
}