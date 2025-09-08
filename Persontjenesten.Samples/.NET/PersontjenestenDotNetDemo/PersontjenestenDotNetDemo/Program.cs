using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PersontjenestenDotNetDemo;
using PersontjenestenDotNetDemo.ExternalApi.Persontjenesten;

const string httpClientNamePersontjenesten = "Persontjenesten";

var hostApplicationBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection
            .AddOptions<HelseIdOptions>()
            .BindConfiguration(HelseIdOptions.SectionKey);  

        serviceCollection
            .AddHttpClient(httpClientNamePersontjenesten, httpClient =>
            {
                httpClient.BaseAddress = new("http://et.persontjenesten.test.nhn.no");
                httpClient.DefaultRequestHeaders.Add("Api-Version", "3");
            })
            .UseHelseIdDPoP();

        serviceCollection.AddSingleton<IDiscoveryCache>(serviceProvider =>
        {
            var factory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var options = serviceProvider.GetRequiredService<IOptions<HelseIdOptions>>();
            return new DiscoveryCache(options.Value.Authority, () => factory.CreateClient());
        });

        serviceCollection.AddSingleton(provider =>
        {
            var persontjenestenHttpClient = provider
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientNamePersontjenesten);

            return new Event_FullAccessClient(persontjenestenHttpClient);
        });

        serviceCollection.AddHostedService<MySimpleTestService>();
    });

await hostApplicationBuilder.Build().RunAsync();

public class MySimpleTestService : BackgroundService
{
    private readonly Event_FullAccessClient _persontjenestenEventClient;

    public MySimpleTestService(Event_FullAccessClient persontjenestenEventClient)
    {
        _persontjenestenEventClient = persontjenestenEventClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var latestEventDocument = await _persontjenestenEventClient.LatestAsync();
        Console.WriteLine($"Sequence number: {latestEventDocument.SequenceNumber}");
    }
}
