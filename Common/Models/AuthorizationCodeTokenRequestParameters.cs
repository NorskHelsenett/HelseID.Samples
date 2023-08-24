namespace HelseId.Samples.Common.Models;

public  class AuthorizationCodeTokenRequestParameters : TokenRequestParameters
{
    public AuthorizationCodeTokenRequestParameters(string code, string codeVerifier, string redirectUri, ICollection<string> resource)
    {
        Code = code;
        CodeVerifier = codeVerifier;
        RedirectUri = redirectUri;
        Resource = resource;
    }
    
    public string Code { get; }
    
    public string CodeVerifier { get; }
    
    public string RedirectUri { get; }
    
    public ICollection<string> Resource { get; }
}
