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
    public class TestController : Controller
    {

        //Configuration for HelseID - Utvikling enviroment
        //Only Scope needed to access API
        const string Scopes = "willy:newsampleapi/";


        private const string KeyName = "HelseId_DCR_Key";

        public async Task<IActionResult> Token()
        {
            var ms = new MemoryStream();
            await Request.Body.CopyToAsync(ms);
            var data = Encoding.UTF8.GetString(ms.ToArray());
            var parts = data.Split('&');

            var code = parts.Single(p => p.StartsWith("code")).Replace("code=", "");

            var hc = new HttpClient();           
            var disco = await hc.GetDiscoveryDocumentAsync("https://helseid-sts.utvikling.nhn.no");
            
            var tokenRequest = new AuthorizationCodeTokenRequest { Address = disco.TokenEndpoint, ClientId = "NewSample-MVCHybridClientAuthentication", RedirectUri = "https://localhost:44388/Test/Token", ClientSecret = "vbHwjXlKYILNpafLgOwgrviAn8R3XFkHXNnLSaryqe7I8Y03zSLtmg5FICkHZEHC", Code = code };
         
            var tokenResponse = await hc.RequestAuthorizationCodeTokenAsync(tokenRequest);
            var accessToken = tokenResponse.AccessToken;
            
            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            var response = await client.GetStringAsync("http://localhost:5003/api");
           
            ViewBag.Json = JArray.Parse(response.ToString());
            ViewBag.AccessToken = new JwtBuilder().Decode(accessToken);

            return View();
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var hc = new HttpClient();
            
            var disco = await hc.GetDiscoveryDocumentAsync("https://helseid-sts.utvikling.nhn.no");

            var requestUrl = new RequestUrl(disco.AuthorizeEndpoint);
            var authorizeUrl = requestUrl.CreateAuthorizeUrl("NewSample-MVCHybridClientAuthentication", "code", Scopes, "https://localhost:44388/Test/Token", nonce: "3123131313", prompt: "none", responseMode: "form_post");

            return Redirect(authorizeUrl);
        }



     

    }
}