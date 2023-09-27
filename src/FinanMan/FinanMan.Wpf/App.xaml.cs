using FinanMan.BlazorUi.Extensions;
using FinanMan.BlazorUi.State;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.StateInterfaces;
using FinanMan.SharedClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace FinanMan.Wpf;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; } = default!;
    public App()
    {
        var services = new ServiceCollection();

        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7176") });

        services
            .AddScoped<ILookupListState, LookupListState>()
            .AddScoped<ITransactionsState, TransactionsState>()
            .AddSingleton<IUiState, UiState>()
            .AddFinanManLocalization()
            .AddClientServices();

        ServiceProvider = services.BuildServiceProvider();

        Resources.Add("services", ServiceProvider);
    }
}
