using FinanMan.Shared.StateInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinanMan.Wpf;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IDisposable
{
    private static App App => (App)Application.Current;
    private readonly IUiState _uiState;

    public MainWindow()
    {
        var services = App.ServiceProvider;
        Resources.Add("services", services);
        InitializeComponent();
        //NoName.InvalidateVisual();
        _uiState = services.GetRequiredService<IUiState>();
        _uiState.InitialUiLoadComplete += HandleInitialUiLoaded;
        _uiState.SomeNum = 33;
    }

    protected override void OnLocationChanged(EventArgs e)
    {
        base.OnLocationChanged(e);
        // Try invoking this in a DragStart type of event.  This collapses
        // the active select element if there is one
        var uiState = App.ServiceProvider.GetRequiredService<IUiState>();
        uiState.CollapseAllSelectLists();

        // This will notify the Blazor Web View control that the window position has changed
        // which will make sure that newly expanded select elements will appear in the right
        // place.
        BlazWebView.WebView.UpdateWindowPos();
    }
       
    private Task HandleInitialUiLoaded()
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _uiState.InitialUiLoadComplete -= HandleInitialUiLoaded;

    }
}
