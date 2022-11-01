// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;

namespace PersontjenestenDotNetDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Credentials
            var clientCredential = new ClientCredentials();
            var token = await clientCredential.GetJwt();

            var apiClient = new ApiClient("https://et.persontjenesten.test.nhn.no");

            //hent event
            var eventApiLegalBasis = new EventWithLegalBasisApi();
            eventApiLegalBasis.Client = apiClient;
            eventApiLegalBasis.Configuration.AccessToken = token;
            var latestEvent = eventApiLegalBasis.ApiLegalBasisEventLatestGet();

           Console.WriteLine($"{JsonConvert.SerializeObject(latestEvent)}");
        }
    }
}
