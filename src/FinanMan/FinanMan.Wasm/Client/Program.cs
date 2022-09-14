using FinanMan.BlazorUi;
using FinanMan.BlazorUi.Extensions;
using FinanMan.Shared.Extensions;
using FinanMan.SharedClient.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddFinanManLocalization();
builder.Services.AddStateManagement();

builder.Services.AddClientServices();


await builder.Build().RunAsync();
