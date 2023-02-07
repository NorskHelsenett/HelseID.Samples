using Blazor.WASM.Api.Access.Shared.AdminApiModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Blazor.WASM.Api.Access.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

                HelseIdConfigurationOwner[] confOwners = Array.Empty<HelseIdConfigurationOwner>();

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
