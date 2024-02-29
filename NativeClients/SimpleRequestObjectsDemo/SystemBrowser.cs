using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.OidcClient.Browser;

namespace HelseId.Samples.SimpleRequestObjectsDemo;

// This class opens up the system browser in order to log in a user and get the authorization code back
public class SystemBrowser : IBrowser
{
    private int Port { get; }

    public SystemBrowser(int port)
    {
        Port = port;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken)
    {
        using var listener = new LoopbackHttpListener(Port);
        OpenBrowser(options.StartUrl);

        try
        {
            var result = await listener.WaitForCallbackAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = "Empty response." };
            }

            return new BrowserResult { Response = result, ResultType = BrowserResultType.Success };
        }
        catch (TaskCanceledException ex)
        {
            return new BrowserResult { ResultType = BrowserResultType.Timeout, Error = ex.Message };
        }
        catch (Exception ex)
        {
            return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = ex.Message };
        }
    }

    private static void OpenBrowser(string url)
    {
        try
        {
            Process.Start(url);
        }
        catch
        {
            // hack because of this: https://github.com/dotnet/corefx/issues/10361
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                throw;
            }
        }
    }
}
