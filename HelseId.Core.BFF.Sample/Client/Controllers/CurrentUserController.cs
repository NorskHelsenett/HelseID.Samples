using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelseId.Core.BFF.Sample.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Core.BFF.Sample.Client.Controllers
{
    [ApiController]
    [Route("api/current-user")]
    public class CurrentUserController : ApiControllerBase
    {
        private readonly IApiClient _apiClient;
        private readonly ICurrentUser _currentUser;

        public CurrentUserController(IApiClient apiClient, ICurrentUser currentUser)
        {
            this._apiClient = apiClient;
            this._currentUser = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult<CurrentUser>> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var claims = HttpContext.User.Claims
                .GroupBy(x => x.Type)
                .ToDictionary(x => x.Key, x => x.Select(c => c.Value).ToList());

            return new CurrentUser
            {
                Name = _currentUser.Name,
                Pid = _currentUser.Id,
                AccessToken = accessToken,
                IdToken = idToken,
                Claims = claims,
            };
        }


        [HttpPut("contactinfo")]
        public async Task<IActionResult> Update(CancellationToken cancellationToken)
        {
            return Map(await _apiClient.Forward(Request, cancellationToken));
        }
    }

    public class CurrentUser
    {
        public string? Name { get; set; }
        public string? Pid { get; set; }
        public string? AccessToken { get; set; }
        public string? IdToken { get; set; }
        public Dictionary<string, List<string>>? Claims { get; set; }
    }
}