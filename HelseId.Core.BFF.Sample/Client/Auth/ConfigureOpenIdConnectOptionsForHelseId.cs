using System;
using System.Net.Http;
using System.Threading.Tasks;
using Duende.AccessTokenManagement;
using HelseId.Core.BFF.Sample.WebCommon.Extensions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HelseId.Core.BFF.Sample.Client.Auth;

/// <summary>
/// Configures OpenIdConnectOptions for supporting Pushed Authorization Requests and Client Assertions,
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
        // Handles Pushed Authorization Request
        options.Events.OnRedirectToIdentityProvider = CreateCallback(options.Events.OnRedirectToIdentityProvider);

        // Handles Client Assertion
        options.Events.OnAuthorizationCodeReceived = CreateCallback(options.Events.OnAuthorizationCodeReceived);
    }

    private Func<RedirectContext, Task> CreateCallback(Func<RedirectContext, Task> inner)
    {
        async Task Callback(RedirectContext context)
        {
            if (inner != null)
            {
                await inner.Invoke(context);
            }

            if (!context.Handled)
            {
                await OnRedirectToIdentityProvider(context);
            }
        }

        return Callback;
    }

    private Func<AuthorizationCodeReceivedContext, Task> CreateCallback(Func<AuthorizationCodeReceivedContext, Task> inner)
    {
        async Task Callback(AuthorizationCodeReceivedContext context)
        {
            if (inner != null)
            {
                await inner.Invoke(context);
            }

            await OnAuthorizationCodeReceived(context);
        }

        return Callback;
    }

    private async Task OnRedirectToIdentityProvider(RedirectContext context)
    {
        var acrValues = ""; // TODO

        // Pushed Authorization Request is not supported by the middleware, so do it ourselves
        var parResult = await PushAuthorizationParameters(context, acrValues);

        // // Replace standard parameters with PAR parameters
        context.ProtocolMessage.Parameters.Clear();
        context.ProtocolMessage.ClientId = context.Options.ClientId;
        context.ProtocolMessage.RequestUri = parResult.RequestUri;

        // // Tell the middleware that we will handle the redirect ourselves
        context.HandleResponse();

        RedirectToAuthorizeEndpoint(context);
    }

    private static async Task OnAuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
    {
        // Reuse IClientAssertionService from Duende.AccessTokenManagement for generating client assertion
        var clientAssertionBuilder = context.HttpContext.RequestServices.GetRequiredService<IClientAssertionService>();
        var clientAssertion = (await clientAssertionBuilder.GetClientAssertionAsync()).ExpectNotNull();

        var tokenEndpointRequest = context.TokenEndpointRequest.ExpectNotNull();
        tokenEndpointRequest.ClientAssertionType = clientAssertion.Type;
        tokenEndpointRequest.ClientAssertion = clientAssertion.Value;
    }

    private static async Task<PushedAuthorizationResponse> PushAuthorizationParameters(RedirectContext context, string? acrValues)
    {
        // Construct the state parameter and add it to the protocol message so that we can include it in the pushed authorization request
        context.Properties.Items.Add(OpenIdConnectDefaults.RedirectUriForCodePropertiesKey, context.ProtocolMessage.RedirectUri);
        context.ProtocolMessage.State = context.Options.StateDataFormat.Protect(context.Properties);
        context.Properties.Items.Remove(OpenIdConnectDefaults.RedirectUriForCodePropertiesKey);

        var oidcConfig = await context.Options.ConfigurationManager.ExpectNotNull().GetConfigurationAsync(default);

        var clientAssertionBuilder = context.HttpContext.RequestServices.GetRequiredService<IClientAssertionService>();
        var clientAssertion = (await clientAssertionBuilder.GetClientAssertionAsync()).ExpectNotNull();

        var par = new PushedAuthorizationRequest
        {
            Address = (string)oidcConfig.AdditionalData["pushed_authorization_request_endpoint"],
            Parameters = new Parameters(context.ProtocolMessage.Parameters),
            ClientCredentialStyle = ClientCredentialStyle.PostBody,
            ClientAssertion = clientAssertion,
        };

        if (!string.IsNullOrWhiteSpace(acrValues))
        {
            par.AcrValues = acrValues;
        }

        var httpClientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
        using var httpClient = httpClientFactory.CreateClient();

        var response = await httpClient.PushAuthorizationAsync(par);

        if (response.IsError)
        {
            throw new Exception($"PAR failure: {response.Json}");
        }

        return response;
    }

    private static void RedirectToAuthorizeEndpoint(RedirectContext context)
    {
        // This code is copied from the ASP.NET handler. We use most of its default behavior related to
        // redirecting to the identity provider, except that we have already pushed the state parameter,
        // so that is left out here.
        // See https://github.com/dotnet/aspnetcore/blob/c85baf8db0c72ae8e68643029d514b2e737c9fae/src/Security/Authentication/OpenIdConnect/src/OpenIdConnectHandler.cs#L364

        var message = context.ProtocolMessage;
        if (string.IsNullOrEmpty(message.IssuerAddress))
        {
            throw new InvalidOperationException(
                "Cannot redirect to the authorization endpoint, the configuration may be missing or invalid: IssuerAddress is missing");
        }

        var redirectUri = message.CreateAuthenticationRequestUrl();
        if (!Uri.IsWellFormedUriString(redirectUri, UriKind.Absolute))
        {
            throw new InvalidOperationException($"The redirect URI is not well-formed. The URI is: '{redirectUri}'.");
        }

        context.Response.Redirect(redirectUri);
    }
}