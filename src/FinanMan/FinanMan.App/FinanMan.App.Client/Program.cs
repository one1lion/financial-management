using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FinanMan.BlazorUi.Extensions;
using FinanMan.BlazorUi.SharedComponents.Extensions;
using FinanMan.Shared.Extensions;
using FinanMan.SharedClient.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddFinanManLocalization()
    .AddStateManagement()
    .AddClientServices()
    .AddJavaScriptModules();

await builder.Build().RunAsync();
