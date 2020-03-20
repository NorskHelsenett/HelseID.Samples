using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HelseId.Core.BFF.Sample.Client.Infrastructure.AutomaticTokenManagement
{
    public static class AutomaticTokenManagementBuilderExtensions
    {
        /// <summary>
        /// Add services to automatically refreshes access token
        /// </summary>
        /// <remarks>
        /// Borrowed/modified from https://github.com/IdentityServer/IdentityServer4/tree/master/samples/Clients/src/MvcHybridAutomaticRefresh
        /// </remarks>
        public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder, Action<AutomaticTokenManagementOptions> options)
        {
            builder.Services.Configure(options);
            return builder.AddAutomaticTokenManagement();
        }

        /// <summary>
        /// Add services to automatically refreshes access token
        /// </summary>
        /// <remarks>
        /// Borrowed/modified from https://github.com/IdentityServer/IdentityServer4/tree/master/samples/Clients/src/MvcHybridAutomaticRefresh
        /// </remarks>
        public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder)
        {
            builder.Services.AddHttpClient<TokenEndpointService>();
            builder.Services.AddTransient<AutomaticTokenManagementCookieEvents>();
            builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, AutomaticTokenManagementConfigureCookieOptions>();

            return builder;
        }
    }
}