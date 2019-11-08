using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccessNewToken.Sample
{
    public class Settings
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string Scope { get; set; }
        public string ClientSecret { get; set; }
        public string ResponseType { get; set; }
        public string DefaultChallengeScheme { get; set; }
        public string SignInScheme { get; set; }
        public string Api { get; set; }
        public string RedirectUri { get; set; }
        public string ApiClientSecret { get; set; }
        public string ApiClientId { get; set; }
        public string ApiScope { get; set; }
    }
}
