using FinanMan.Shared.StateInterfaces;

namespace FinanMan.Maui;

public partial class App : Application
{
    public App(IUiState uiState)
    {
        InitializeComponent();

        MainPage = new MainPage(uiState);
    }
}
