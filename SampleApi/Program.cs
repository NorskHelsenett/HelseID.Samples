using System.CommandLine;
using HelseId.SampleApi.Configuration;
using HelseID.Samples.Configuration;

namespace HelseId.SampleAPI;

// This file is used for bootstrapping the example. Nothing of interest here.
public static class Program
{
    public static void Main(string[] args)
    {
        new Startup().BuildWebApplication().Run();
    }
}