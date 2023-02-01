using Duende.AccessTokenManagement;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Blazor.WASM.Api.Access.Server
{
    public class ClientAssertionService : IClientAssertionService
    {
        private readonly ClientAssertionConfiguration _configuration;

        public ClientAssertionService(ClientAssertionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<ClientAssertion?> GetClientAssertionAsync(string? clientName = null, TokenRequestParameters? parameters = null)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration.ClientId,
                Audience = _configuration.HelseIdAuthorithy,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = GetSigningCredentials(),

                Claims = new Dictionary<string, object>
                {
                    { JwtClaimTypes.JwtId, Guid.NewGuid().ToString() },
                    { JwtClaimTypes.Subject, _configuration.ClientId! },
                    { JwtClaimTypes.IssuedAt, DateTime.UtcNow.ToEpochTime() }
                }
            };

            var handler = new JsonWebTokenHandler();
            var jwt = handler.CreateToken(descriptor);

            return Task.FromResult<ClientAssertion?>(new ClientAssertion
            {
                Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                Value = jwt
            });
        }

        private SigningCredentials GetSigningCredentials()
        {
            return new SigningCredentials(new JsonWebKey(_configuration.ClientSecretJwk), SecurityAlgorithms.RsaSha512);
        }
    }
}
