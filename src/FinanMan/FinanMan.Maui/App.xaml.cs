using FinanMan.Shared.StateInterfaces;
using System.Globalization;

namespace FinanMan.Maui;

public partial class App : Application
{
    public App(IUiState uiState)
    {
        InitializeComponent();
        
        MainPage = new MainPage(uiState);
    }

}
