using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HelseId.Core.BFF.Sample.Client.Infrastructure;
using HelseId.Core.BFF.Sample.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelseId.Core.BFF.Sample.Client.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult Map(HttpResponseMessage responseMessage)
        {
            return new ApiHttpResponseMessageResult(responseMessage);
        }

        protected async Task<IActionResult> Forward(IApiClient apiClient, CancellationToken cancellationToken = default)
        {
            return Map(await apiClient.Forward(Request, cancellationToken));
        }
    }
}