namespace HelseId.Samples.ClientCredentials;

public static class ConfigurationValues
{
    public static string IssuerUri { get; set; } = "https://helseid-sts.test.nhn.no";

    // Audience and scopes for using the 'regular' sample API:
    public const string SampleApiNameAudience = "nhn:test-public-samplecode";

    public const string ClientCredentialsScopeForSampleApi = $"{SampleApiNameAudience}/client-credentials";

    // Port numbers for the different sample APIs
    public const int SampleApiPort = 5081;

    // URLS and resource parameter (url fragment) for the use of the sample API for the client credentials project
    public const string SampleApiMachineClientResource = "machine-clients/greetings";

    public static readonly string SampleApiUrlForM2M = $"https://localhost:{SampleApiPort}/{SampleApiMachineClientResource}";

    // If used, the child organization number (underenhet) must match a number in the client's whitelist as it's held by HelseID:
    public const string GranfjelldalKommuneOrganizationNumber = "999977774";
    public const string GranfjelldalKommuneChildOrganizationNumber1 = "999977776";
    public const string GranfjelldalKommuneChildOrganizationNumber2 = "999977775";

    // ----------------------------------------------------------------------------------------------------------------------
    // The private key JWK match the public key in HelseID that are attached to the corresponding client configuration:
    // ----------------------------------------------------------------------------------------------------------------------
    // In a production scenario, a private key MUST be secured inside the client, and should NOT be set in the source code.
    // ----------------------------------------------------------------------------------------------------------------------
    public static readonly string ClientCredentialsSampleRsaPrivateKeyJwk = """{ "p": "1JDtA9RzEs-dgZ7YDOZPX4VJLofseq9cs7rTvxF9I6QXo3_0-FIkymdiitIcLrfXtRcmJ8bIDzhHYYUkt_5cYSdCjg90EsbLRyjz7xl4wXgCJsEga-shhjxMqiZ7JS5lMd2FkuHw9fk6iDlokmn6zIMDFgSwCy-avN4Wl8k0tS8", "kty": "RSA", "q": "nQd92kOJqtjgWy6DEeOCAsW18qLYEiXXDYFpWnxNBK9Bx5ao01UmfSGyjeCyrmYrxGdUsyXQRirUlUq5foXhbC6noiVLv2763p2vwdbQdy6UPj5rzjPhmaKBV3MXi-VKB7Hdf6vR6AXk5dBIBTPlmjJor-blngu3QKoIpzisLDM", "d": "NnL8DUu6Ci0BCJXN88RyQ-Og_j4tF3LkzIFg45ehkJRcxSIFU53hrewKqdVkkjFCl898CT_5AqItzjJeW6BGW5nVOVOACu_FNcCFM9_ZevXM5VtrwS5zFmQep19JZ5uFOPcBGjLOU5vd0FoBckZ2YtwvP5p8dHjL3tA0R2nvkSsOBQYCcrPvFwYLlMSxEfbvKZWUAIX3wtKOmfOw3daa5fYkpUxBXz2XcMYhXZE87vUA0qG_1d6K0r51DEayuej9D11lcIxCWXWi1LAT7FFjikt8nKxeRIZEieViPEjxw8gSWKLGMMy2FZQgLLvDuHmTzuEDPgZVBbCElkl7l8dMYQ", "e": "AQAB", "use": "sig", "qi": "09EacJNthPVKNoea8Nj5PC37iilrc2mlVQtrXobCAkHcByVR7xPZT1cZZ3NSiMpSQu2y4tr_LA62xmnSU3zVgcEi9CfI8h2nGN82wv8SVxtUK9-RcqAohJj0y4UtnhA9atcTfZvlI5RLMP7mdLnkgzZe9Oq7Jr_OMD4IKZhG8-E", "dp": "iBXkd5A6v69FUifEf7Wu2SN2r6B7iCveuH4CdA-ZQwkZzSXtSlEklqRLlT5gppQyOBCC7_I2QHAyWr-nu1fQAq7k0Bgaoq68k2knikqPYaUYE4GO5ShahRrzpfcO3cXvKVZ93oRiBMezbmT6isnos6eogR8tKWwnr4SriC9bXCc", "alg": "RS512", "dq": "m2g6qbSFnswc3qDdnuqmVNAPDh8T8IH6n6cf-Sljn-tDEqCMXPq8qMKcz8U9kVQUpMAPF22o_oiM82OMySb-ve4-gT6gBMl1BrTQqOpMTmeO1zs3vk-iSkaF82I4P3-hEJR7Pktx5ktPChJj9KIz7bNN4CiHvy6hIiIlhjmUS_k", "n": "gmMZ0dOeUyEbEQ0Bz4gGqcYgMfreJ1yIMD_h0HyRjNB0-e1jzb4yjBBkx1pOpPvt02trdyt_nOCmR7DLrHmLrDCmAbpYIytUPNEuz6GJkT31oL3vDmNqrkawQwe3B04FIWohiTQWgnPaZiTViRdajxpJJW6juPVzr75TlWwyimT2bDq-5TxwiLxyRJDUUWnTvjUPkFVHGMwNFQxV9SCrjHSueCF8vQ959Peh-Yxvtvz2T29HgHKf7oh37DCEy-PrWLTgykeEvNhyWclDM4jm5NaqOCE-sbmmRR4wnCWksvGod7aS7IaKzg2_nNihrxAIzIOqEuLXXaUmYPeHKDgsXQ" }""";

    // -----------------------------------------------------------------------------------------------------------------
    // Client IDs:
    // -----------------------------------------------------------------------------------------------------------------
    // In HelseID, the client id is normally set as a GUID, here we have named it for better readability
    // -----------------------------------------------------------------------------------------------------------------
    public const string ClientCredentialsSampleClientId = "helseid-sample-client-credentials-for-multitenant-app";

    // -----------------------------------------------------------------------------------------------------------------
    // Client scopes:
    // -----------------------------------------------------------------------------------------------------------------

    public const string ClientCredentialsSampleScope = $"{ClientCredentialsScopeForSampleApi}";
}