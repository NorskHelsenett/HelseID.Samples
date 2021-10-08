
namespace HelseId.ClienAuthentication
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
    }
}

