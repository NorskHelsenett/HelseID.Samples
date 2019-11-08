using HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample;
using IdentityModel.Client;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccess.Sample.Controllers
{
    public class AuthController : Controller
    {
        private readonly Settings _settings;

        public AuthController(Settings settings)
        {            
            _settings = settings;
        }

        private const string KeyName = "HelseId_DCR_Key";
       
        public async Task<IActionResult> Token(string code)
        {

            var hc = new HttpClient();           
            var disco = await hc.GetDiscoveryDocumentAsync(_settings.Authority);
            
            var tokenRequest = new AuthorizationCodeTokenRequest { Address = disco.TokenEndpoint, ClientId = _settings.ApiClientId, RedirectUri = _settings.RedirectUri, ClientSecret = _settings.ApiClientSecret, Code = code };
         
            var tokenResponse = await hc.RequestAuthorizationCodeTokenAsync(tokenRequest);
            var accessToken = tokenResponse.AccessToken;
            
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var response = await client.GetStringAsync(_settings.Api);
           
            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = new JwtBuilder().Decode(accessToken);

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            var hc = new HttpClient();
            
            var disco = await hc.GetDiscoveryDocumentAsync(_settings.Authority);

            var requestUrl = new RequestUrl(disco.AuthorizeEndpoint);
            var authorizeUrl = requestUrl.CreateAuthorizeUrl(_settings.ApiClientId, _settings.ResponseType, _settings.ApiScope, _settings.RedirectUri, nonce: "3123131313", prompt: "none", responseMode: "form_post");

            return Redirect(authorizeUrl);
        }



     

    }
}