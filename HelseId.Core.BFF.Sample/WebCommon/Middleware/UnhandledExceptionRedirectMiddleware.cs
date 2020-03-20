using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace HelseId.Core.BFF.Sample.WebCommon.Middleware
{
    public class RedirectOnExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _redirectPage;
        private readonly ILogger _logger = Log.ForContext<RedirectOnExceptionMiddleware>();

        public RedirectOnExceptionMiddleware(RequestDelegate next, string redirectPage)
        {
            _next = next;
            _redirectPage = redirectPage;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.Verbose(ex, "Handling exception by redirecting to {page}", _redirectPage);
                context.Response.Redirect(_redirectPage);
            }
        }
    }

    public static class RedirectOnExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseRedirectOnException(
            this IApplicationBuilder builder,
            string redirectPage
        )
        {
            return builder.UseMiddleware<RedirectOnExceptionMiddleware>(redirectPage);
        }
    }
}