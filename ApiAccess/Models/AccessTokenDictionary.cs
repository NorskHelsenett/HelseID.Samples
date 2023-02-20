namespace HelseId.Samples.ApiAccess.Models;

public class AccessTokenDictionary : Dictionary<string, AccessTokenWithExpiration>
{
    public new void Add(string apiAudience, AccessTokenWithExpiration accessToken)
    {
        base[apiAudience] = accessToken;
    }

    public new AccessTokenWithExpiration this[string apiAudience]
    {
        get { return base[apiAudience]; }
        set { base[apiAudience] = value; }
    }
}