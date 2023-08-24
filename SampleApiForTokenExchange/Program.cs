using System.CommandLine;

namespace HelseId.SampleApiForTokenExchange;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.Title = "Sample API";
        var useDPoPOption = new Option<bool>(
            aliases: new [] {"--use-dpop", "-dp"},
            description: "If set, the application will use the demonstrating proof-of-possesion mechanism for sender-constraining the token sent to the example API.",
            getDefaultValue: () => false);

        var rootCommand = new RootCommand("An API that does token exchange")
        {
            useDPoPOption
        };
        
        rootCommand.SetHandler((useDPoP) =>
        {
            var settings = new Settings(useDPoP);
            new Startup(settings).BuildWebApplication().Run();
        }, useDPoPOption);

        await rootCommand.InvokeAsync(args);
    }
}