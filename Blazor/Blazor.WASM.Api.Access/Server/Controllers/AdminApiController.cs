using Blazor.WASM.Api.Access.Shared;
using Blazor.WASM.Api.Access.Shared.AdminApiModels;
using Duende.Bff;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<HelseIdClient> Get(string id)
        {
            // create HTTP client
            var httpClient = _httpClientFactory.CreateClient("apiClient");

            // get current user access token and set it on HttpClient
            var token = await HttpContext.GetUserAccessTokenAsync();
            httpClient.SetBearerToken(token.ToString());
                    

            // call remote API
            var response = await httpClient.GetAsync($"/api/search?t={id}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JObject.Parse(responseBody.ToString());
            var stringResult = obj.SelectToken("clients.result").First.ToString();
            HelseIdClient client = JsonConvert.DeserializeObject<HelseIdClient>(stringResult);

            return client;
        }
    }
}