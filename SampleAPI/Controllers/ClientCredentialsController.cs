using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

// TODO: fix this link!
/// <summary>
/// This API (controller) supports the Client Credentials JWK Sample
/// https://github.com/NorskHelsenett/HelseID.Samples/tree/master/HelseId.Samples.ClientCredentials.Jwk
/// </summary>
namespace HelseId.SampleAPI.Controllers
{
    [Route("api/client_credentials")]
    [ApiController]
    [Authorize(Policy = "client_credentials_policy", AuthenticationSchemes = "client_credentials_scheme")]
    public class ClientCredentialsController : ControllerBase
    {
        public ClientCredentialsController()
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            // Get the claims of the current principal (a system user, not a personal user)
            var claims = HttpContext.User.Claims.Select(c => new { c.Type, c.Value }); ;

            // Return the claims in Json format
            return new JsonResult(claims);
        }
    }
}
