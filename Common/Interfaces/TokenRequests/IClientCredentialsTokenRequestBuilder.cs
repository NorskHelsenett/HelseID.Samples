using HelseId.Samples.Common.Interfaces.PayloadClaimsCreators;
using HelseId.Samples.Common.Models;
using IdentityModel.Client;

namespace HelseId.Samples.Common.Interfaces.TokenRequests;

public interface ITokenRequestBuilder
{
    Task<ClientCredentialsTokenRequest> CreateClientCredentialsTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        ClientCredentialsTokenRequestParameters tokenRequestParameters);
    
    Task<RefreshTokenRequest> CreateRefreshTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        RefreshTokenRequestParameters tokenRequestParameters);
    
    Task<TokenExchangeTokenRequest> CreateTokenExchangeTokenRequest(
        IPayloadClaimsCreator payloadClaimsCreator,
        TokenExchangeTokenRequestParameters tokenRequestParameters);
}
