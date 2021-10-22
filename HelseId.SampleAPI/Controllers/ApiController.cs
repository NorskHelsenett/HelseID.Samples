using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace HelseId.SampleAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy="requireScopeAndUser")] // Authentication is needed to access the API
    public class ApiController: ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get()
        {
            // Get the claims of the logged in user
            var claims = User.Claims.Select(c => new { c.Type, c.Value });

            // Output the claims to the console as log information
            _logger.LogInformation("claims: {claims}", claims); 

            // Return the claims in Json format
            return new JsonResult(claims);
        }
    }
}
