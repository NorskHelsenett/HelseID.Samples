using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using HelseId.APIAccessNewToken.Models;
using Newtonsoft.Json.Linq;
using IdentityModel.OidcClient;
using System.Text.Json;

namespace HelseId.APIAccessNewToken.Controllers
{
    public class AuthController: Controller
    {
        private readonly Settings _settings;
        private IDiscoveryCache _cache;

        public AuthController(Settings settings, IDiscoveryCache cache)
        {
            _settings = settings;
            _cache = cache;
        }

        // The action creates and redirects the user to an authorization url
        [Authorize]
        public async Task<IActionResult> Index()
        {

            // Setup the oidc client for authentication against HelseID
            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.Authority,
                RedirectUri = _settings.RedirectUri,
                Scope = _settings.Scope,
                ClientId = _settings.ClientId,
            });

            // In production the cookie must be encrypted
            var state = await oidcClient.PrepareLoginAsync();
            Response.Cookies.Append("state", JsonSerializer.Serialize(state));

            // The action redirects the user to the authorization url
            return Redirect(state.StartUrl);
        }

        // The action requests a new token by using the authorization code and shows the API response
        public async Task<IActionResult> Token()
        {

            var disco = await _cache.GetAsync();

            // Setup the oidc client for authentication against HelseID
            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = _settings.Authority,
                RedirectUri = _settings.RedirectUri,
                Scope = _settings.Scope,
                ClientId = _settings.ClientId,
                ClientAssertion = new ClientAssertion() {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = BuildClientAssertion.Generate(_settings.ClientId, disco.TokenEndpoint, "jwk.json")
                }
                });

            var state = JsonSerializer.Deserialize<AuthorizeState>(Request.Cookies["state"]);
            var tokenResponse = await oidcClient.ProcessResponseAsync(Request.QueryString.ToString(), state);

       
            //New access token retrieved from the token response
            var accessToken = tokenResponse.AccessToken;

            var client = new HttpClient();
            // Sets the new access token as bearer 
            client.SetBearerToken(accessToken);
            // Accesses the API with the new access token
            var response = await client.GetStringAsync(_settings.ApiUrl);

            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = accessToken.ToString();

            return View();
        }
    }
}