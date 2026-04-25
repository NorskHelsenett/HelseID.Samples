using System.CommandLine;

namespace HelseId.SampleApiForTokenExchange;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await new Startup(new Settings()).BuildWebApplication().RunAsync();
    }
}
