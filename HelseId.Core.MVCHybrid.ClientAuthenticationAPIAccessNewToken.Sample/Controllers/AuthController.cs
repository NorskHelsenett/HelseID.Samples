using HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample;
using IdentityModel.Client;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
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
        private IDiscoveryCache _cache;

        public AuthController(Settings settings, IDiscoveryCache cache)
        {            
            _settings = settings;
            _cache = cache;
        }

        private const string KeyName = "HelseId_DCR_Key";


        [Authorize]
        public async Task<IActionResult> Index()
        {


            _cache = new DiscoveryCache(_settings.Authority);
            var disco = await _cache.GetAsync();
           

            var requestUrl = new RequestUrl(disco.AuthorizeEndpoint);
            var authorizeUrl = requestUrl.CreateAuthorizeUrl(
                _settings.ApiClientId,
                _settings.ResponseType,
                _settings.ApiScope,
                _settings.RedirectUri,
                _settings.Nonce,
                prompt: "none",
                responseMode: "form_post");

            return Redirect(authorizeUrl);
        }

        public async Task<IActionResult> Token(string code)
        {

            var hc = new HttpClient();

            _cache = new DiscoveryCache(_settings.Authority);
            var disco = await _cache.GetAsync();

            var tokenRequest = new AuthorizationCodeTokenRequest { Address = disco.TokenEndpoint, ClientId = _settings.ApiClientId, RedirectUri = _settings.RedirectUri, ClientSecret = _settings.ApiClientSecret, Code = code };
         
            var tokenResponse = await hc.RequestAuthorizationCodeTokenAsync(tokenRequest);
            var accessToken = tokenResponse.AccessToken;
            
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var response = await client.GetStringAsync(_settings.ApiUrl);
           
            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = new JwtBuilder().Decode(accessToken);

            return View();
        }

       



     

    }
}