using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using Spectre.Console.Json;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using HelseId.Samples.Common.JwtTokens;
using HelseId.Samples.TestTokenDemo.Configuration;
using HelseId.Samples.TestTokenDemo.TttModels.Request;
using HelseId.Samples.TestTokenDemo.TttModels.Response;
using Spectre.Console.Rendering;

namespace HelseId.Samples.TestTokenDemo;

// Demo code for using the Test Token Service (TTT). Should only be used for testing.
// The examples can be adapted to your needs. Check the full request model for all options.
// Documentation can be found at: https://utviklerportal.nhn.no/informasjonstjenester/helseid/tilgang-til-helseid/test-token-tjenesten/docs/test-token-tjenesten_enmd/
internal abstract class Program
{
    private static async Task<int> Main(string[] args)
    {
        var config = GetConfig();

        if (string.IsNullOrWhiteSpace(config.ApiKey))
        {
            await Console.Error.WriteLineAsync("No ApiKey specified. Add ApiKey to appsettings.json.");
            return 1;
        }

        var font = FigletFont.Load("starwars.flf");

        AnsiConsole.Write(
            new FigletText(font, "HelseID")
                .LeftJustified()
                .Color(Color.Aqua));

        using var tttHttpClient = new HttpClient();

        tttHttpClient.DefaultRequestHeaders.Add("X-Auth-Key", config.ApiKey);

        await DemoSuperman(tttHttpClient, config);
        // await DemoPersontjenesten(tttHttpClient, config);
        // await DemoTillitsrammeverkWithDpop(tttHttpClient, config);

        return 0;
    }

    // Generating access token for fake API/audience
    private static async Task DemoSuperman(HttpClient tttHttpClient, TttConfig config)
    {
        var model = new TestTokenRequest
        {
            Audience = "the_universe",
            ClientClaimsParameters = new ClientClaimsParameters
            {
                ClientId = "superman",
                ClientName = "Superman client",
                OrgnrParent = "123456789",
                Scope = ["save", "the_world"]
            },
            UserClaimsParameters = new UserClaimsParameters
            {
                Name = "Clark Kent",
                Pid = "20080012345",
                SecurityLevel = "1337"
            },
            SetSubject = true,
        };

        var (accessToken, dpopToken) = await GetAccessToken(config, tttHttpClient, model);

        PrintTokens(accessToken, dpopToken);
    }

    // Generating access token for use with Persontjenesten
    private static async Task DemoPersontjenesten(HttpClient tttHttpClient, TttConfig config)
    {
        var model = new TestTokenRequest
        {
            Audience = "nhn:hgd-persontjenesten-api",
            ClientClaimsParameters = new ClientClaimsParameters
            {
                Scope = ["nhn:hgd-persontjenesten-api/read-no-legal-basis"],
                ClientId = "fake",
                OrgnrParent = "123123123",
            },

            // Test different invalid token cases:
            // SetInvalidIssuer = true,
            // SignJwtWithInvalidSigningKey = true,
            // ExpirationParameters = new ExpirationParameters
            // {
            //     SetExpirationTimeAsExpired = true,
            // },
        };

        var (accessToken, _) = await GetAccessToken(config, tttHttpClient, model);

        PrintTokens(accessToken);

        await ApiPost(
            "https://et.persontjenesten.test.nhn.no/api/no-legal-basis/person/get-by-nin?informationParts=Name&includeHistory=false",
            new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("nin", "05898597468")
            }),
            accessToken
        );
    }

    // Generate token with tillitsrammeverk and DPoP
    private static async Task DemoTillitsrammeverkWithDpop(HttpClient tttHttpClient, TttConfig config)
    {
        var model = new TestTokenRequest
        {
            CreateDPoPTokenWithDPoPProof = true,
            DPoPProofParameters = new DPoPProofParameters
            {
                HtmClaimValue = "GET",
                HtuClaimValue = "https://innlogging.st1.kjernejournal-test.no/api/session/ping",

                // Test different invalid DPoP cases:
                // InvalidDPoPProofParameters = InvalidDPoPProofParameters.DontSetHtuClaimValue,
                // InvalidDPoPProofParameters = InvalidDPoPProofParameters.SetIatValueInThePast,
                // InvalidDPoPProofParameters = InvalidDPoPProofParameters.DontSetAthClaimValue,
                // InvalidDPoPProofParameters = InvalidDPoPProofParameters.SetAlgHeaderToASymmetricAlgorithm,
                // InvalidDPoPProofParameters = InvalidDPoPProofParameters.SetAnInvalidSignature,
            },
            Audience = "nhn:kjernejournal",
            ClientClaimsParameters = new ClientClaimsParameters
            {
                Scope =
                    ["nhn:kjernejournal/innlogging", "nhn:kjernejournal/tillitsrammeverk", "nhn:kjernejournal/api"],
                ClientId = "fake",
                OrgnrParent = "958935420",
                OrgnrChild = "894234482",
            },
            UserClaimsParameters = new UserClaimsParameters
            {
                Name = "Julenissen",
                SecurityLevel = "4",
                Pid = "05898597468",
            },
            SetPidPseudonym = true,
            GetHprNumberFromHprregisteret = true,
            CreateTillitsrammeverkClaims = true,
            TillitsrammeverkClaimsParameters = new TillitsrammeverkClaimsParameters()
            {
                PractitionerAuthorizationCode = "AA",
                PractitionerLegalEntityId = "958935420",
                PractitionerLegalEntityName = "Fake entity",
                PractitionerPointOfCareId = "894234482",
                PractitionerPointOfCareName = "Fake poc"
            },
        };

        var (accessToken, dpopToken) = await GetAccessToken(config, tttHttpClient, model);
        PrintTokens(accessToken, dpopToken);

        await ApiGet(
            "https://innlogging.st1.kjernejournal-test.no/api/session/ping",
            accessToken,
            dpopToken,
            // Header required by Kjernejournal
            extraHeaders: new Dictionary<string, string> { ["X-SOURCE-SYSTEM"] = "fake" }
        );
    }

    private static async Task<(string AccessToken, string? DpopToken)> GetAccessToken(TttConfig config,
        HttpClient httpClient, TestTokenRequest model)
    {
        var response = await httpClient.PostAsJsonAsync(config.TttUri, model, options: JsonSerializerOptions);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        var tttResponse = await response.Content.ReadFromJsonAsync<TestTokenResponse>();

        if (tttResponse == null)
        {
            throw new Exception("Response was deserialized to null");
        }

        if (tttResponse.IsError)
        {
            throw new Exception($"Received error response from TTT: {tttResponse.ErrorResponse.ErrorMessage}");
        }

        return (tttResponse.SuccessResponse.AccessTokenJwt, tttResponse.SuccessResponse.DPoPProof);
    }

    private static TttConfig GetConfig()
    {
        var builder =
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Local.json", optional: true);

        IConfiguration configuration = builder.Build();

        var config = configuration.Get<TttConfig>();

        if (config == null)
        {
            throw new Exception("Config is null.");
        }

        return config;
    }

    private static void PrintTokens(string accessToken, string? dpopToken = null)
    {
        if (dpopToken != null)
        {
            PrintDpopToken(dpopToken);
        }

        PrintAccessToken(accessToken);
    }

    private static void PrintAccessToken(string accessToken)
    {
        PrintToken(accessToken, "Access token payload");
    }

    private static void PrintDpopToken(string dpopToken)
    {
        PrintToken(dpopToken, "DPoP token payload");
    }

    private static void PrintToken(string token, string heading)
    {
        var payload = JwtDecoder.Decode(token);
        PrintBorderedContent(CreateRenderableJsonText(payload), heading);
    }

    private static async Task ApiGet(string apiUri, string accessToken, string? dpopToken = null,
        Dictionary<string, string>? extraHeaders = null)
    {
        var message = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(apiUri),
        };


        if (extraHeaders != null)
        {
            foreach (var (key, value) in extraHeaders)
            {
                message.Headers.Add(key, value);
            }
        }

        using var httpClient = CreateApiClient(accessToken, dpopToken);

        var response = await httpClient.SendAsync(message);
        await PrintResponse(response);
    }

    private static async Task ApiPost(string apiUri, HttpContent content, string accessToken,
        string? dpopToken = null)
    {
        using var httpClient = CreateApiClient(accessToken, dpopToken);
        var response = await httpClient.PostAsync(apiUri, content);
        await PrintResponse(response);
    }

    private static HttpClient CreateApiClient(string accessToken, string? dpopToken,
        IDictionary<string, string>? extreaHeaders = null)
    {
        var httpClient = new HttpClient();
        if (dpopToken != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DPoP", accessToken);
            httpClient.DefaultRequestHeaders.Add("DPoP", dpopToken);
        }
        else
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        return httpClient;
    }

    private static async Task PrintResponse(HttpResponseMessage response)
    {
        var status = response.StatusCode;
        var body = await response.Content.ReadAsStringAsync();

        PrintBorderedContent(status != HttpStatusCode.OK ? new Text(status.ToString()) : CreateRenderableJsonText(body),
            "API response");
    }

    private static void PrintBorderedContent(IRenderable content, string header)
    {
        AnsiConsole.Write(
            new Panel(content)
                .Header(header)
                .Collapse()
                .RoundedBorder()
                .BorderColor(Color.Yellow));
    }

    private static JsonText CreateRenderableJsonText(string json)
    {
        return new JsonText(json)
            .MemberColor(Color.Aqua)
            .StringColor(Color.HotPink)
            .NumberColor(Color.MediumOrchid);
    }

    static Program()
    {
        JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        };

        JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions;
}