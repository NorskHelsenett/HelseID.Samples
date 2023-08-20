using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;

namespace HelseId.Samples.Common.Interfaces.TokenRequests;

public interface ITokenRequestBuilder
{
    Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        ClientCredentialsTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce);
    
    Task<RefreshTokenRequest> CreateRefreshTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        RefreshTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce);
    
    Task<TokenExchangeTokenRequest> CreateTokenExchangeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        TokenExchangeTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce);

    Task<AuthorizationCodeTokenRequest> CreateAuthorizationCodeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        AuthorizationCodeTokenRequestParameters tokenRequestParameters,
        string? dPoPNonce);
}
