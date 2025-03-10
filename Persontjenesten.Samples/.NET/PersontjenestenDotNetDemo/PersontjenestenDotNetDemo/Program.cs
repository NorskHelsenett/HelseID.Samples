using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                httpClient.DefaultRequestHeaders.Add("Api-Version", "2");
            })
            .UseHelseIdDPoP();

        serviceCollection.AddSingleton(provider =>
        {
            var persontjenestenHttpClient = provider
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientNamePersontjenesten);

            return new Event_withLegalBasisClient(persontjenestenHttpClient);
        });

        serviceCollection.AddHostedService<MySimpleTestService>();
    });

await hostApplicationBuilder.Build().RunAsync();

public class MySimpleTestService : BackgroundService
{
    private readonly Event_withLegalBasisClient _persontjenestenEventClient;

    public MySimpleTestService(Event_withLegalBasisClient persontjenestenEventClient)
    {
        _persontjenestenEventClient = persontjenestenEventClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var latestEventDocument = await _persontjenestenEventClient.LatestAsync();
        Console.WriteLine($"Sequence number: {latestEventDocument.SequenceNumber}");
    }
}
