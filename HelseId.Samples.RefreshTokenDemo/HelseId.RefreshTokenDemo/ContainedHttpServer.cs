using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HelseId.RefreshTokenDemo
{
    public class ContainedHttpServer : IDisposable
    {
        const int DefaultTimeout = 60 * 5; // 5 mins (in seconds)

        IWebHost _host;
        TaskCompletionSource<string> _source = new TaskCompletionSource<string>();
        private readonly string _callbackUrl;
        private readonly Dictionary<string, Action<HttpContext>> _routes;

        public ContainedHttpServer(string host,
            string callbackUrl,
            Dictionary<string, Action<HttpContext>> routes)
        {
            _callbackUrl = callbackUrl;
            _routes = routes;

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(host)
                .Configure(Configure)
                .Build();
            _host.Start();
        }

        public void Dispose()
        {
            Task.Run(async () =>
            {
                await Task.Delay(500);
                _host.Dispose();
            });
        }

        void Configure(IApplicationBuilder app)
        {
            app.Run(async ctx =>
            {
                if (_routes.ContainsKey(ctx.Request.Path.Value))
                {
                    _routes[ctx.Request.Path.Value](ctx);
                }
                else if (ctx.Request.Path.Equals(_callbackUrl))
                {

                    if (ctx.Request.Method == "GET")
                    {
                        SetResult(ctx.Request.QueryString.Value, ctx);
                    }
                    else if (ctx.Request.Method == "POST")
                    {
                        if (!ctx.Request.ContentType.Equals("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
                        {
                            ctx.Response.StatusCode = 415;
                        }
                        else
                        {
                            using (var sr = new StreamReader(ctx.Request.Body, Encoding.UTF8))
                            {
                                var body = await sr.ReadToEndAsync();
                                SetResult(body, ctx);
                            }
                        }
                    }
                    else
                    {
                        ctx.Response.StatusCode = 405;
                    }
                }
                else
                {
                    ctx.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            });
        }

        private void SetResult(string value, HttpContext ctx)
        {
            try
            {
                ctx.Response.StatusCode = 200;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h1>You can now return to the application.</h1>");
                ctx.Response.Body.Flush();

                _source.TrySetResult(value);
            }
            catch
            {
                ctx.Response.StatusCode = 400;
                ctx.Response.ContentType = "text/html";
                ctx.Response.WriteAsync("<h1>Invalid request.</h1>");
                ctx.Response.Body.Flush();
            }
        }

        public Task<string> WaitForCallbackAsync(int timeoutInSeconds = DefaultTimeout)
        {
            Task.Run(async () =>
            {
                await Task.Delay(timeoutInSeconds * 1000);
                _source.TrySetCanceled();
            });

            return _source.Task;
        }
    }
}
