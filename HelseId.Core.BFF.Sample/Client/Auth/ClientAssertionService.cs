using System.Threading.Tasks;
using Duende.AccessTokenManagement;
using Duende.IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace HelseId.Core.BFF.Sample.Client.Auth;

public class ClientAssertionService : IClientAssertionService
{
    private readonly IOptionsMonitor<HelseIdAuthOptions> _options;

    public ClientAssertionService(IOptionsMonitor<HelseIdAuthOptions> options)
    {
        _options = options;
    }

    public Task<ClientAssertion?> GetClientAssertionAsync(string? clientName = null, TokenRequestParameters? parameters = null)
    {
        var options = _options.CurrentValue;

        var clientAssertion = ClientAssertionBuilder.GetClientAssertion(options.ClientId, options.ClientJwk, options.Authority);

        return Task.FromResult<ClientAssertion?>(new ClientAssertion
        {
            Type = clientAssertion.Type,
            Value = clientAssertion.Value,
        });
    }
}
