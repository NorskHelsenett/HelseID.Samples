using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace HelseId.Core.BFF.Sample.Client.Services
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> Get(
            string path,
            string? accessToken = null,
            CancellationToken cancellationToken = default
        );

        Task<T> Get<T>(
            string path,
            string? accessToken = null,
            CancellationToken cancellationToken = default
        );

        Task<HttpResponseMessage> Forward(HttpRequest request, CancellationToken cancellationToken = default);
    }

    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Get(
            string path,
            string? accessToken = null,
            CancellationToken cancellationToken = default
        )
        {
            var response = await _httpClient.GetAsync(path, cancellationToken);
            return response;
        }

        public async Task<T> Get<T>(string path, string? accessToken = null, CancellationToken cancellationToken = default)
        {
            var response = await Get(path, accessToken, cancellationToken);
            response.EnsureSuccessStatusCode();

            var contentStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(contentStream, cancellationToken: cancellationToken);
        }

        public async Task<HttpResponseMessage> Forward(
            HttpRequest request,
            CancellationToken cancellationToken = default
        )
        {
            HttpContent content;
            using (var memoryStream = new MemoryStream())
            {
                await request.Body.CopyToAsync(memoryStream, cancellationToken: cancellationToken);
                content = new ByteArrayContent(memoryStream.ToArray());
            }

            if (request.ContentType != null)
            {
                content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            }

            if (request.ContentLength != null)
            {
                content.Headers.ContentLength = request.ContentLength;
            }

            var path = request.GetEncodedPathAndQuery();
            if (!path.StartsWith("/api/"))
            {
                throw new ArgumentException("Request contains invalid path", nameof(request));
            }

            path = path.Substring("/api".Length);

            var requestMsg = new HttpRequestMessage(new HttpMethod(request.Method), path)
            {
                Content = content,
            };
            var response = await _httpClient.SendAsync(requestMsg, cancellationToken);
            return response;
        }
    }
}