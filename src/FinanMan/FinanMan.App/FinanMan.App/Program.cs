using FinanMan.App.Components;
using FinanMan.BlazorUi.Extensions;
using FinanMan.BlazorUi.SharedComponents.Extensions;
using FinanMan.Shared.Extensions;
using FinanMan.SharedServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services
    .AddFinanManLocalization()
    .AddStateManagement()
    .AddJavaScriptModules()
    .SetupDbContext(builder.Configuration)
    //.AddClientServices()
    .AddServerServices()
    .AddFluentValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(FinanMan.App.Client._Imports).Assembly,
        typeof(FinanMan.BlazorUi._Imports).Assembly);

app.MapControllers();

app.Run();
