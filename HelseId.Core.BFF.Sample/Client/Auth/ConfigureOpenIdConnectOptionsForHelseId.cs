using System;
using System.Threading.Tasks;
using Duende.AccessTokenManagement;
using HelseId.Core.BFF.Sample.WebCommon.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ClientAssertion = Duende.IdentityModel.Client.ClientAssertion;

namespace HelseId.Core.BFF.Sample.Client.Auth;

/// <summary>
/// Configures OpenIdConnectOptions for supporting Client Assertions,
/// which is currently not handled by Duende.AccessTokenManagement during logon.
///
/// Must be added after AddOpenIdConnectAccessTokenManagement.
/// </summary>
public class ConfigureOpenIdConnectOptionsForHelseId : IConfigureNamedOptions<OpenIdConnectOptions>
{
    public void Configure(string? name, OpenIdConnectOptions options)
    {
        Configure(options);
    }

    public void Configure(OpenIdConnectOptions options)
    {
        // Handles Client Assertion
        options.Events.OnPushAuthorization = CreateCallback(options.Events.OnPushAuthorization);
        options.Events.OnAuthorizationCodeReceived = CreateCallback(options.Events.OnAuthorizationCodeReceived);
    }

    private Func<PushedAuthorizationContext, Task> CreateCallback(Func<PushedAuthorizationContext, Task> inner)
    {
        async Task Callback(PushedAuthorizationContext context)
        {
            await inner.Invoke(context);

            if (!context.HandledPush)
            {
                await OnPushAuthorization(context);
            }
        }

        return Callback;
    }

    private Func<AuthorizationCodeReceivedContext, Task> CreateCallback(Func<AuthorizationCodeReceivedContext, Task> inner)
    {
        async Task Callback(AuthorizationCodeReceivedContext context)
        {
            await inner.Invoke(context);

            await OnAuthorizationCodeReceived(context);
        }

        return Callback;
    }

    private static async Task OnPushAuthorization(PushedAuthorizationContext context)
    {
        var message = context.ProtocolMessage;
        
        var clientAssertion = await BuildClientAssertion(context);
        message.ClientAssertionType = clientAssertion.Type;
        message.ClientAssertion = clientAssertion.Value;
    }

    private static async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
    {
        var tokenEndpointRequest = context.TokenEndpointRequest.ExpectNotNull();

        var clientAssertion = await BuildClientAssertion(context);
        tokenEndpointRequest.ClientAssertionType = clientAssertion.Type;
        tokenEndpointRequest.ClientAssertion = clientAssertion.Value;
    }

    private static async Task<ClientAssertion> BuildClientAssertion(BaseContext<OpenIdConnectOptions> context)
    {
        // Reuse IClientAssertionService from Duende.AccessTokenManagement for generating client assertion
        var clientAssertionBuilder = context.HttpContext.RequestServices.GetRequiredService<IClientAssertionService>();
        return (await clientAssertionBuilder.GetClientAssertionAsync()).ExpectNotNull();
    }
}