using Blazor.WASM.Api.Access.Server;
using IdentityModel;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using DuendeIdentityModel = IdentityModel;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

string JwkPrivateKey = " {\"d\":\"gqPTHTnumQognGT7dSq18OoB9FOrh92ptNdIHhRDTwGYe04i6b9GHL0LZTnaP4cMjKA-HV7ycveo1arDu3LwMkaIWH3adcz_UtqouEbSCx76uhpO7cPJC63usfZmBN5sI7cKWFOOfxuLqdLvlNyXnSyvPA5hFF_q4Scw6rm8bvNKuj4_Te7Ly0h5NSdxMG-RaSAMEqujFgEJzBFYifYoIrXNcHAUSD_uoerQsHZvV7p-YomGeYg-KlbuI8FDf0gbqHNY4TJTyMhIS5pgIqjur15-g7xsU7XA8CVGh9xyMAOiIekCbiEupu97iN1oMN03iF1xIKzMnP8JY8mLLwE6Md61N8-XGn1PkajZ8rkhqu8yTavjst4Yg5EXwLz2PE6p8ISXbvWhQf2c5exQ2-T1Kfxd9d9MDiSXyrYiglQCo9OvWs_2pA5tNuerplxHZ_jvKykybYutcwbVTOuJN03mTT9h5VWDIuIvGPZvDIHhWoE0fBPSEoZwZ43pyiouUZ3wCql3-9fXUfTpKk5dWk3RMMLhKRrH5n9AhWZcNPvMQq5y6gyvqG3xdLim2WjnZhEKmDdw4dVgZMt0HBdTfbn0f4ytXZFYmRboZxqu2oGbr9cE9m9HLC-9j4KHpWteM24y11fq6R4-jloQ8xcNGtMWMQSMqT8PC4fwq2JlNY1cZM0\",\"dp\":\"jCzmoI-0sEptVULO1tVB4FTrMpGPYOkX-hvI3AZVqWbr9wdbh-rORRNXYYzxM2YnNYjySv-B4tctpy0SaGR-VX_gFaHpS8piblczEuWUrjwQ0CHU7mgwPKjTKMw914OQ88evivXx3QMyoN_q9_mdjNeDIIbN9Xv4C1a8szwVoVoBauhlxvb6r2ZOUvw24fdjlv7oI3aF5K2Xx-NAkWb-OHDJSACZzFTh8vtiXmr6IKKuhYlDpv4RBsaF8fnHXCP5Y0L1g2ZZLWQXPqbCqVVe3HjFzUvLQ37hkVvVP2CsCFsNaUOpulwu62zL01kLTorTL3ee7cn8cqe7DDM8JPvYfQ\",\"dq\":\"N2jN30wevdVUNdHpSEAR9_PsI0hQJbEkWV9VUHHfvuHpJd-V1nVDNn5h497vpcsQhtiDfrL28Vq2O0-L3UnWjqOTcc--rF6en1aNaL0fzolNFHUhgCQOsIigTQdzIRvFS-EYfwyGeUyOHCmYa7T_hNrzwOM0cG9I9PCiNJ2OpnrvLaXl3l1bYge6tJIcM110xGG2riWixKPMHt5x7vegCFLp67J0YWXqTkHASeps4tZteIbN9pCCFIIr_dX4ffvlLzZ7b2ZGVZh7y2hS8Bi0cUvtiHl70gopSBzjMz_x34XVx88JxRvbNm4eVEC_Gj9Lq_JEaFU_kuyuGOxVfHq12Q\",\"e\":\"AQAB\",\"kty\":\"RSA\",\"n\":\"7-YaeegkHcJWdCTA-KJruPx0n0-2oWNzoGAnokPTAVVgoo-MmULm_ND3ktWupyCwMnm31gff74u1aNToMa-ATJlvxWW6btlPLruD-_N6ZNFaNY_HBICtLtcNNHNBw8N-wacj7OeRNZrOx-6uD3sSaV_qBul96hm--f8jxYPxV9jCzZoNjvP4An-GyWG0-6psj_W1Qvy_tILr7jRh1fiDEp0bGC1Pcbdi1jZ9tc9LHEhDLRUIE8na_bg6x_eAcBkZIZzzBx3_Z8mqG9YbevYeM2kmOhvRUY0ijU-pcYVekOUYRxCQqRS-M8ML6r7pcU8SWlhBANa2vWjotwVHeCUUQC0avnUkIkJiNvWRlUcCsqCmasMGLokiG_cEp9bO-t0JaNUnc7St30xjn9aDR6gOJgDTBiLJSHz9lhwBj1ADX_n5bhYSPjQb-oHQOnqYIpceOMVipmr7uxADpIlOn37s7UwgqlPAh5xg44EBPerr-BltDgcV3QOIOjY_Wc1bX67iVdCoiSaE1_ZYalRGkJwTU2sHCi6pRQlAelE9gyyrluERoskbXhyuGp7DuBOJOvRs7KRFuxKBD5WqTa-xNYWleug56sAbJDPvsMg670qQcx33q2ZGQeAXGmeO78yOI2hWud4CKDFgi-UOlz2w95jwhhb4X-IH8_slYQlz26azdyU\",\"p\":\"94ae0mwp_jNUcGfE6YAfRWxxHkIl9Eb4JRuY64IknGpfF0SfMXHeM1t64ZVhjJVy9FEBSFdiv9hiGd8KQpGWUFv4emftOPVvLnzbhpsHbzWWmuJ4tffZ9VRpkrWQllLbpRDIxJgArr0vzSPvjiMpOT6BphhoRGIKSVrs4SUvNChbTcbdTOL44lxUi_UWRFSFXLVDw-X2uVm95ylnJvgCiNC1Km8Cec2-24VCKTrQMk2pcxfOHisf1OZ4RWqM8kTgeO6qI-wOUViVj3GkJrzN5O5YB0uBdtyRLwm3Uhv5DeK1_X8lBt6VLhaXcjeyPyRj5N7Bh9lVVByDTbKADL8vlw\",\"q\":\"-ByjTIGKNsiNzZe8u2N1VLCpcKaTT26mR5Fu48pFRy7OiAYU9IKVOt42wGqv-TDgMxeedKx9u1LmkXy7cw0mjwtwzpdFi_OZQaWwXjdr4RhXRsXS5SkwSSu-DW3G6zOJMbd0EtabkBwdiZ1uJZiCTmdHOJVB5nVI6CuAwahSWU1o4w20X3zqykBzPJp-u0D_0n8b6dOolY8XNHLBefmvijs6rQdY_gLdBVF7sPzUpOTt3GSVHkSTkqZiXsQmsIbyaUprJgVHFOYRR4F5rPIoDf_W8BnScd3qWc1nVwVWQcLxnbbYQ0OpSLdj4Xq01y7FE5-8LhDKM0nUlIHThE5mow\",\"qi\":\"s3vrDAprdKW_jvzm3HGdAebSRSRCtFQa5ZE-uk2fGhUFHNh4PxsOpIL47RXZ29JFJSsPA76LkyzzJJglLYJP3ujfJebLKriul74SvYZGc4tjDR-QZyemYXwZBGbNm_j1nvK-Q7FWQMp4pQfUO3am4gn00DgqS5r7ZBCegwOvYBEYv3QqdHXW-fLehR4EepMXfkKWFzC1DsNDn1OqLRtRjVY4bZdjJ2t_JpUmc5cPyJ4G9MmbFsYYc2VAdjElTuFBonI1uREY4pBXAVsa0ZXB26MhkIHgMjlTkoiEzC6rLUl5glUWtyKbhrkQistBBpqZc03O0uP5RnNftKuuZh2ZjQ\",\"kid\":\"a5ef022003dc43c1b5ad229c7acd6bb5\",\"use\":\"sig\"} ";
IdentityModelEventSource.ShowPII = true;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
        .MinimumLevel.Override("IdentityModel", LogEventLevel.Debug)
        .MinimumLevel.Override("Duende.Bff", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
            theme: AnsiConsoleTheme.Code));

    builder.Services.AddControllers();
    builder.Services.AddRazorPages();
    builder.Services.AddBff();

    // registers HTTP client that uses the managed user access token
    builder.Services.AddUserAccessTokenHttpClient("apiClient", configureClient: client =>
    {
        client.BaseAddress = new Uri("https://localhost:44356/");
    });

    builder.Services.Configure<CookiePolicyOptions>(options =>
    {
        options.Secure = CookieSecurePolicy.Always;
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "cookie";
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
        .AddCookie("cookie", options =>
        {
            options.Cookie.Name = "__Host-blazor";
            options.Cookie.SameSite = SameSiteMode.Strict;

        })
        .AddOpenIdConnect("oidc", options =>
        {
             options.Authority = "https://localhost:44366/";
            //options.Authority = "https://helseid-sts.utvikling.nhn.no/";
            options.RequireHttpsMetadata = true;
            
            options.ClientId = "BlazorWasmApiAccess";
            // options.ClientSecret = "rbwzNdV5jVFPMm1n5Z8coGwiY6VUBRaGrfNc-uAM1DExShWSIv36iNUSw_sBhm6y";

            options.ResponseType = "code";            

            

            options.AuthenticationMethod = Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectRedirectBehavior.FormPost;

            // request scopes + refresh tokens
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("helseid://scopes/client/sts_configuration_admin");

            // not mapped by default
           // options.ClaimActions.MapJsonKey("website", "website");

            // keeps id_token smaller
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };

            options.Events.OnAuthorizationCodeReceived = ctx =>
            {
                // Sets the client assertion as a Jwt bearer type
                ctx.TokenEndpointRequest.ClientAssertionType = DuendeIdentityModel.OidcConstants.ClientAssertionTypes.JwtBearer;
                // Asserts a client by using the generated Jwt
                ctx.TokenEndpointRequest.ClientAssertion = BuildClientAssertion(ctx.Options.Authority, ctx.Options.ClientId);

                return Task.CompletedTask;
            };

        });

    builder.Services.AddUserAccessTokenHttpClient("apiClient", configureClient: client =>
    {
        // client.BaseAddress = new Uri("https://helseid-admin.utvikling.nhn.no/");
        client.BaseAddress = new Uri("https://localhost:44356/");
    });
    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
    }

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();
    app.UseAuthentication();
    app.UseBff();
    app.UseAuthorization();

    app.MapBffManagementEndpoints();
    app.MapRazorPages();

    app.MapControllers()
        .RequireAuthorization()
        .AsBffApiEndpoint();

    app.MapFallbackToFile("index.html");

    app.Run();

    string BuildClientAssertion(string authority, string clientId)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, clientId),
                new Claim(JwtClaimTypes.IssuedAt, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()),
                new Claim(JwtClaimTypes.JwtId, Guid.NewGuid().ToString("N")),
        };

        var credentials = new JwtSecurityToken(clientId, authority, claims, DateTime.UtcNow, DateTime.UtcNow.AddSeconds(60), new SigningCredentials(new JsonWebKey(JwkPrivateKey), SecurityAlgorithms.RsaSha512));

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(credentials);
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
