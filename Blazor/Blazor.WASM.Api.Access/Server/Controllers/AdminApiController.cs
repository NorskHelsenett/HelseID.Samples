using Blazor.WASM.Api.Access.Shared.AdminApiModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Blazor.WASM.Api.Access.Server.Controllers
{//Not in use, using built-in simple HTTP forwarder: MapRemoteBffApiEndpoint
    [ApiController]
    [Route("[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post(HelseIdConfigurationOwner helseIdConfigurationOwner)
        {
            // create HTTP client
            var httpClient = _httpClientFactory.CreateClient("apiClient");

            // call remote API
            var response = await httpClient.PostAsJsonAsync($"/api/ConfigurationOwnersApi",helseIdConfigurationOwner, new CancellationToken());
  
            return response;
        }

        [HttpGet]
        public async Task<HelseIdConfigurationOwner[]> Get()
        {
            // create HTTP client
            var httpClient = _httpClientFactory.CreateClient("apiClient");

            // call remote API
            var response = await httpClient.GetAsync($"/api/ConfigurationOwnersApi");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {

                HelseIdConfigurationOwner[] confOwners = new HelseIdConfigurationOwner[1];

                return confOwners;
                

            }
            else
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();



                JArray obj = JArray.Parse(responseBody.ToString());
                var stringResult = obj.SelectToken("").ToString();
                HelseIdConfigurationOwner[] confOwners = JsonConvert.DeserializeObject<HelseIdConfigurationOwner[]>(stringResult);
                return confOwners;
            }
         

            
        }
    }
}
