using FinanMan.BlazorUi.Extensions;
using FinanMan.Shared.Extensions;
using FinanMan.SharedClient.Extensions;

namespace FinanMan.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7176/") });

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif

        builder.Services.AddFinanManLocalization();
        builder.Services.AddStateManagement();
        builder.Services.AddClientServices();

        return builder.Build();
    }
}