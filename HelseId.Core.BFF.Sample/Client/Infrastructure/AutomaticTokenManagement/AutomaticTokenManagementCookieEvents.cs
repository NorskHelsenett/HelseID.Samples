using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace HelseId.Core.BFF.Sample.Client.Infrastructure.AutomaticTokenManagement
{
    public class AutomaticTokenManagementCookieEvents : CookieAuthenticationEvents
    {
        private readonly TokenEndpointService _service;
        private readonly AutomaticTokenManagementOptions _options;
        private readonly ILogger _logger;
        private readonly ISystemClock _clock;

        private static readonly ConcurrentDictionary<string, bool> PendingRefreshTokenRequests =
            new ConcurrentDictionary<string, bool>();

        public AutomaticTokenManagementCookieEvents(
            TokenEndpointService service,
            IOptions<AutomaticTokenManagementOptions> options,
            ILogger<AutomaticTokenManagementCookieEvents> logger,
            ISystemClock clock)
        {
            _service = service;
            _options = options.Value;
            _logger = logger;
            _clock = clock;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            await _options.CookieEvents.ValidatePrincipal(context);

            var tokens = context.Properties.GetTokens()?.ToList();
            if (tokens == null || !tokens.Any())
            {
                _logger.LogDebug("No tokens found in cookie properties. SaveTokens must be enabled for automatic token refresh.");
                return;
            }

            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                _logger.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return;
            }

            var expiresAt = tokens.SingleOrDefault(t => t.Name == "expires_at");
            if (expiresAt == null)
            {
                _logger.LogWarning("No expires_at value found in cookie properties.");
                return;
            }

            var dtExpires = DateTimeOffset.Parse(expiresAt.Value, CultureInfo.InvariantCulture);
            var dtRefresh = dtExpires.Subtract(_options.RefreshBeforeExpiration);

            if (dtRefresh < _clock.UtcNow)
            {
                var shouldRefresh = PendingRefreshTokenRequests.TryAdd(refreshToken.Value, true);
                if (shouldRefresh)
                {
                    try
                    {
                        var response = await _service.RefreshTokenAsync(refreshToken.Value);

                        if (response.IsError)
                        {
                            _logger.LogWarning("Error refreshing token: {error}", response.Error);
                            context.RejectPrincipal();
                            return;
                        }

                        context.Properties.UpdateTokenValue("access_token", response.AccessToken);
                        context.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                        var newExpiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
                        context.Properties.UpdateTokenValue("expires_at", newExpiresAt.ToString("o", CultureInfo.InvariantCulture));

                        await context.HttpContext.SignInAsync(context.Principal, context.Properties);
                    }
                    finally
                    {
                        PendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);
                    }
                }
            }
        }

        public override async Task SigningOut(CookieSigningOutContext context)
        {
            await _options.CookieEvents.SigningOut(context);

            if (_options.RevokeRefreshTokenOnSignout == false) return;

            var result = await context.HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                _logger.LogDebug("Can't find cookie for default scheme. Might have been deleted already.");
                return;
            }

            var tokens = result.Properties.GetTokens()?.ToList();
            if (tokens == null || !tokens.Any())
            {
                _logger.LogDebug("No tokens found in cookie properties. SaveTokens must be enabled for automatic token revocation.");
                return;
            }

            var refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                _logger.LogWarning("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return;
            }

            var response = await _service.RevokeTokenAsync(refreshToken.Value);

            if (response.IsError)
            {
                _logger.LogWarning("Error revoking token: {error}", response.Error);
                return;
            }
        }

        public override Task RedirectToAccessDenied(RedirectContext<CookieAuthenticationOptions> context)
        {
            return _options.CookieEvents.RedirectToAccessDenied(context);
        }

        public override Task RedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            return _options.CookieEvents.RedirectToLogin(context);
        }

        public override Task RedirectToLogout(RedirectContext<CookieAuthenticationOptions> context)
        {
            return _options.CookieEvents.RedirectToLogout(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            return _options.CookieEvents.RedirectToReturnUrl(context);
        }

        public override Task SignedIn(CookieSignedInContext context)
        {
            return _options.CookieEvents.SignedIn(context);
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            return _options.CookieEvents.SigningIn(context);
        }
    }
}
