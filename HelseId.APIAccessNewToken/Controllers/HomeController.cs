using HelseId.APIAccessNewToken.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HelseId.APIAccessNewToken.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Settings _settings;

        public HomeController(ILogger<HomeController> logger, Settings settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize] // Starts the configured authentication scheme when the user clicks Login
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // The action calls the Sample API, authenticates the user and loads the API data if the user is authenticated
        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            // Gets the access token from the logged in user
            var token = await HttpContext.GetTokenAsync("access_token");

            // Initializes a new client that can asynchronously interact with web resources
            var client = new HttpClient();

            // Sets the access token as a header value in the HTTP request
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Loads data from the given API (ApiUrl), if the client is allowed access to the API
            var result = await client.GetStringAsync(_settings.ApiUrl);

            // The data is stored in a ViewBag, that can be displayed in the view file
            ViewBag.Json = JArray.Parse(result.ToString());
            ViewBag.AccessToken = token.ToString();

            return View();
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
