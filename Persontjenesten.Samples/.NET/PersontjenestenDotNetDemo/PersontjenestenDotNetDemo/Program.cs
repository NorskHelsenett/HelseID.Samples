using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        serviceCollection
            .AddHttpClient(httpClientNamePersontjenesten, httpClient =>
            {
                httpClient.BaseAddress = new("http://et.persontjenesten.test.nhn.no");
                httpClient.DefaultRequestHeaders.Add("Api-Version", "3");
            })
            .UseHelseIdDPoP();

        serviceCollection.AddSingleton(provider =>
        {
            var persontjenestenHttpClient = provider
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(httpClientNamePersontjenesten);

            return new Event_withFullAccessClient(persontjenestenHttpClient);
        });

        serviceCollection.AddHostedService<MySimpleTestService>();
    });

await hostApplicationBuilder.Build().RunAsync();

public class MySimpleTestService : BackgroundService
{
    private readonly Event_withFullAccessClient _persontjenestenEventClient;

    public MySimpleTestService(Event_withFullAccessClient persontjenestenEventClient)
    {
        _persontjenestenEventClient = persontjenestenEventClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var latestEventDocument = await _persontjenestenEventClient.LatestAsync("3");
        Console.WriteLine($"Sequence number: {latestEventDocument.SequenceNumber}");
    }
}
