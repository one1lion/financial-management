using FinanMan.BlazorUi.State;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.StateInterfaces;
using FinanMan.SharedClient.Extensions;
using Microsoft.Extensions.Logging;

namespace FinanMan.Maui;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var baseAddress = "https://localhost:7176";

#if !DEBUG 
        // TODO: Overwrite the baseAddress with the address of the server
#endif

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });


        builder.Services
            .AddScoped<ILookupListState, LookupListState>()
            .AddScoped<ITransactionsState, TransactionsState>()
            .AddSingleton<IUiState, UiState>()
            .AddFinanManLocalization()
            .AddClientServices();


        return builder.Build();
    }
}
