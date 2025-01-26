using System.Threading;
using System.Threading.Tasks;
using HelseId.Core.BFF.Sample.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Core.BFF.Sample.Client.Controllers
{
    [ApiController]
    [Route("api/sample")]
    public class SampleController : ApiControllerBase
    {
        private readonly IApiClient _apiClient;

        public SampleController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return await Forward(_apiClient, cancellationToken);

            // Alternatively
            //var sample = await _apiClient.Get<SampleModel>("/sample", cancellationToken: cancellationToken);
            //return Ok(sample);
        }
    }
}
