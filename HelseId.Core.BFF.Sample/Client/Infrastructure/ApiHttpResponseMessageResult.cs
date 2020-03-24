using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HelseId.Core.BFF.Sample.Client.Infrastructure
{
    /// <summary>
    /// ActionResult that transforms a response message from API to a appropriate client response
    /// </summary>
    public class ApiHttpResponseMessageResult : IActionResult
    {
        private readonly HttpResponseMessage _responseMessage;
        private const string CustomHeaderPrefix = "API";

        public ApiHttpResponseMessageResult(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int) _responseMessage.StatusCode;

            TransformLocationHeaders(context.HttpContext.Response);

            if (_responseMessage.Content == null)
            {
                return;
            }

            foreach (var (name, values) in _responseMessage.Content.Headers)
            {
                context.HttpContext.Response.Headers.TryAdd(name, new StringValues(values.ToArray()));
            }

            var customHeaders = _responseMessage.Headers.Where(x => x.Key.StartsWith(CustomHeaderPrefix));
            foreach (var (name, values) in customHeaders)
            {
                context.HttpContext.Response.Headers.TryAdd(name, new StringValues(values.ToArray()));
            }

            await using var stream = await _responseMessage.Content.ReadAsStreamAsync();
            await stream.CopyToAsync(context.HttpContext.Response.Body);
            await context.HttpContext.Response.Body.FlushAsync();
        }

        /// <summary>
        /// Transform location headers from Takt API to URLs that refer to the client API.
        /// </summary>
        /// <param name="response">Response to set headers on</param>
        private void TransformLocationHeaders(HttpResponse response)
        {
            if (!_responseMessage.Headers.TryGetValues("Location", out var locationValues))
            {
                return;
            }

            var values = locationValues
                .Where(location => location.StartsWith("/"))
                .Select(location => $"/api{location}")
                .ToArray();
            if (values.Length == 0)
            {
                return;
            }

            response.Headers.TryAdd("location", new StringValues(values));
        }
    }
}