using Blazor.WASM.Api.Access;
using Blazor.WASM.Api.Access.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Radzen.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// authentication state and authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();

// HTTP client configuration
builder.Services.AddTransient<AntiforgeryHandler>();
builder.Services.AddHttpClient("backend", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<AntiforgeryHandler>();
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("backend"));
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<RadzenLayout>();
builder.Services.AddScoped<RadzenText>();
builder.Services.AddScoped<RadzenHeader>();
builder.Services.AddScoped<RadzenSidebarToggle>();
builder.Services.AddScoped<RadzenLabel>();
builder.Services.AddScoped<RadzenPanelMenu>();
builder.Services.AddScoped<RadzenBody>();
builder.Services.AddScoped<RadzenFooter>();

await builder.Build().RunAsync();
