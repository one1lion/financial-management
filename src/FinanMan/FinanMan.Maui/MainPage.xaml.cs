using FinanMan.Shared.StateInterfaces;
using System.Globalization;

namespace FinanMan.Maui;

public partial class MainPage : ContentPage, IDisposable
{
    private readonly static string[] _rtlCultureCodes = new string[] { "ar", "fa", "he", "ur" };

    private readonly IUiState _uiState;
    public MainPage(IUiState uiState)
    {
        _uiState = uiState;
        _uiState.ActiveLanguageChanged += HandleActiveLangaugeChanged;
        
        InitializeComponent();
    }

    private Task HandleActiveLangaugeChanged()
    {
        var culture = CultureInfo.CreateSpecificCulture(_uiState.ActiveLanguage);
        var isRtl = _rtlCultureCodes.Any(c => c == culture.TwoLetterISOLanguageName);
        FlowDirection = isRtl ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _uiState.ActiveLanguageChanged -= HandleActiveLangaugeChanged;
    }
}
