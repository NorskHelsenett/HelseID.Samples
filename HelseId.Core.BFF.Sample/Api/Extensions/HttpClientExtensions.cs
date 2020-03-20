using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HelseId.Core.BFF.Sample.Api.Extensions
{
    public static class HttpClientExtensions
    {
        public static void SetBasicAuthentication(this HttpClient client, string username, string password)
        {
            if (username.Contains(':'))
            {
                throw new ArgumentException(ArgumentColonExceptionMessage, nameof(username));
            }

            if (password.Contains(':'))
            {
                throw new ArgumentException(ArgumentColonExceptionMessage, nameof(password));
            }

            var parameter = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
        }

        private const string ArgumentColonExceptionMessage = "Value cannot include the ':' character";
    }
}
