using FinanMan.Shared.General;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Text.RegularExpressions;

namespace FinanMan.BlazorUi.State;

public class UiState : BaseNotifyPropertyChanges, IUiState
{
    private const string _defaultLanguage = "en-US";

    #region Backing fields
    private RenderFragment? _flyoutContent;
    private bool _flyoutVisible;
    private string _activeLanguage = "en-US";
    #endregion Backing Fields

    public RenderFragment? FlyoutContent { get => _flyoutContent; set => SetField(ref _flyoutContent, value); }
    public bool FlyoutVisible { get => _flyoutVisible; set => SetField(ref _flyoutVisible, value); }
    public string ActiveLanguage { get => _activeLanguage; }

    public event Func<Task>? ActiveLanguageChanged;

    public void SetLanguage(string language)
    {
        try
        {
            var culture = CultureInfo.CreateSpecificCulture(language);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
        catch (CultureNotFoundException)
        {
            language = _defaultLanguage;
            var culture = CultureInfo.CreateSpecificCulture(_defaultLanguage);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        _activeLanguage = language;

        ActiveLanguageChanged?.Invoke();
    }
}
