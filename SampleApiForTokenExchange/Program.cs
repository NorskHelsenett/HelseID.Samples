using System.CommandLine;

namespace HelseId.SampleApiForTokenExchange;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Sample API";
        var rootCommand = new RootCommand("An API that does token exchange");

        rootCommand.SetHandler(() =>
        {
            var settings = new Settings();
            new Startup(settings).BuildWebApplication().Run();
        });

        await rootCommand.InvokeAsync(args);
    }
}
