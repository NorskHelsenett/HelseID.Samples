using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HelseId.Samples.SimpleResourceIndicatorsDemo;

// This class functions as a makeshift web host, getting the redirect response from the browser.
// If the status code is 200, the query string containing the authorization code is set on the
// task completion source, which can then be used by the program to obtain tokens from HelseID
public class LoopbackHttpListener : IDisposable
{
    const int DefaultTimeout = 60 * 5; // 5 mins (in seconds)
    IWebHost _host;
    TaskCompletionSource<string> _source = new ();

    public LoopbackHttpListener(int port)
    {
        var url = $"http://localhost:{port}/";

        _host = new WebHostBuilder()
            .UseKestrel()
            .UseUrls(url)
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
            if (ctx.Request.Method == "GET")
            {
                await SetResultAsync(ctx.Request.QueryString.Value, ctx);
            }
            else
            {
                ctx.Response.StatusCode = 405;
            }
        });
    }

    private async Task SetResultAsync(string value, HttpContext ctx)
    {
        _source.TrySetResult(value);

        try
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync("<h1>You can now return to the application.</h1>");
            await ctx.Response.Body.FlushAsync();
        }
        catch
        {
            ctx.Response.StatusCode = 400;
            ctx.Response.ContentType = "text/html";
            await ctx.Response.WriteAsync("<h1>Invalid request.</h1>");
            await ctx.Response.Body.FlushAsync();
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
