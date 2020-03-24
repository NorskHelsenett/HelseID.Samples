using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace HelseId.Core.BFF.Sample.WebCommon.Middleware
{
    public class UnhandledExceptionLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger = Log.ForContext<UnhandledExceptionLoggerMiddleware>();

        public UnhandledExceptionLoggerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next.Invoke(context);
            }
            catch (Exception ex)
            {
                //SerilogUserInfoEnricher.EnrichWithUserInfo(context);
                this._logger.Error(ex, "Unhandled exception");
                throw;
            }
        }
    }

    public static class UnhandledExceptionLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUnhandledExceptionLogger(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UnhandledExceptionLoggerMiddleware>();
        }
    }
}