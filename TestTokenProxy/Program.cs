using HelseID.Samples.Configuration;

const string allowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// We need to enable options for CORS from the Swagger UI: 
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins($"https://localhost:{ConfigurationValues.SampleApiPort}").AllowAnyHeader();
        });
});

builder.WebHost.ConfigureKestrel(option =>
{
    option.ListenLocalhost(ConfigurationValues.TestTokenProxyServerPort, listenOptions => listenOptions.UseHttps());
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(allowSpecificOrigins);
app.MapControllers();
app.Run();
