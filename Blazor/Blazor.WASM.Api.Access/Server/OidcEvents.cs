using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Duende.Bff;
using Duende.AccessTokenManagement;

namespace Blazor.WASM.Api.Access.Server
{
    public class OidcEvents : BffOpenIdConnectEvents
    {
        private readonly IClientAssertionService _clientAssertionService;

        public OidcEvents(IClientAssertionService clientAssertionService, ILogger<BffOpenIdConnectEvents> logger) : base(logger)
        {
            _clientAssertionService = clientAssertionService;
        }

        public override async Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            var clientAssertion = await _clientAssertionService.GetClientAssertionAsync();

            context.TokenEndpointRequest!.ClientAssertionType = clientAssertion!.Type;
            context.TokenEndpointRequest.ClientAssertion = clientAssertion.Value;
        }
    }
}
