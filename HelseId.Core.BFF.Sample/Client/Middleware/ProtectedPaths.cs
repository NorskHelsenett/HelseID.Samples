using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HelseId.Core.BFF.Sample.Client.Middleware
{
    /// <summary>
    /// Middleware that prevents unauthorized access to paths (by default all)
    /// </summary>
    public class ProtectPaths
    {
        private readonly RequestDelegate _next;
        private readonly string _policyName;
        private readonly List<PathString> _excludedPaths;
        private string _accessDeniedPath;

        public ProtectPaths(RequestDelegate next, ProtectPathsOptions options)
        {
            _next = next;
            _policyName = options.Policy;
            _excludedPaths = options.Exclusions ?? new List<PathString>();
            _accessDeniedPath = options.AccessDeniedPath;
        }

        public async Task Invoke(HttpContext httpContext,
            IAuthorizationService authorizationService)
        {
            var path = httpContext.Request.Path;
            if (!_excludedPaths.Any(p => path.StartsWithSegments(p)))
            {
                if (!httpContext.User.Identity!.IsAuthenticated)
                {
                    var redirectUri = httpContext.Request.GetEncodedPathAndQuery();
                    await httpContext.ChallengeAsync(new AuthenticationProperties { RedirectUri = redirectUri });
                    return;
                }

                var authorizationResult =
                    await authorizationService.AuthorizeAsync(httpContext.User, null, _policyName);
                if (!authorizationResult.Succeeded)
                {
                    httpContext.Response.Redirect(_accessDeniedPath);
                    return;
                }
            }

            await _next(httpContext);
        }
    }

    public class ProtectPathsOptions
    {
        public ProtectPathsOptions(string policy, string accessDeniedPath)
        {
            Policy = policy;
            AccessDeniedPath = accessDeniedPath;
        }

        /// <summary>
        /// Policy required to access paths
        /// </summary>
        public string Policy { get; }

        /// <summary>
        /// Paths that don't require authentication
        /// </summary>
        public List<PathString>? Exclusions { get; set; }

        /// <summary>
        /// Path redirected to if user is not authorized to access paths
        /// </summary>
        public string AccessDeniedPath { get; }
    }

    public static class ProtectPathsExtensions
    {
        public static IApplicationBuilder UseProtectPaths(this IApplicationBuilder builder, ProtectPathsOptions options)
        {
            return builder.UseMiddleware<ProtectPaths>(options);
        }
    }
}