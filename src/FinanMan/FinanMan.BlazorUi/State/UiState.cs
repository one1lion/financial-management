using FinanMan.Shared.General;
using System.Globalization;

namespace FinanMan.BlazorUi.State;

public class UiState : BaseNotifyPropertyChanges, IUiState
{
    private const string _defaultLanguage = "en-US";

    #region Backing fields
    private RenderFragment? _flyoutContent;
    private bool _flyoutVisible;
    private string _activeLanguage = "en-US";
    private bool _messageDialogVisible;
    private MessageDialogParameters _messageDialogParameters;
    #endregion Backing Fields

    public RenderFragment? FlyoutContent { get => _flyoutContent; private set => _flyoutContent = value; }
    public bool FlyoutVisible { get => _flyoutVisible; private set => SetField(ref _flyoutVisible, value); }
    public string ActiveLanguage { get => _activeLanguage; }

    public event Func<Task>? ActiveLanguageChanged;
    public event Func<Task>? CollapseSelectLists;
    public event Func<Task>? InitialUiLoadComplete;

    public int SomeNum { get; set; }

    public bool InitialUiLoaded { get; private set; }
    public bool MessageDialogVisible { get => _messageDialogVisible; set => SetField(ref _messageDialogVisible, value); }
    public MessageDialogParameters MessageDialogParameters { get => _messageDialogParameters; set => SetField(ref _messageDialogParameters, value); }

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

    public void CollapseAllSelectLists()
    {
        CollapseSelectLists?.Invoke();
    }

    public void DisplayFlyout(RenderFragment? content)
    {
        FlyoutContent = content;
        if (content is null)
        {
            CollapseFlyout();
            return;
        }
        FlyoutVisible = true;
    }

    public void CollapseFlyout()
    {
        FlyoutVisible = false;
    }

    public void RaiseInitialUiLoadComplete()
    {
        if (InitialUiLoaded) { return; }
        InitialUiLoaded = true;
        InitialUiLoadComplete?.Invoke();
    }

    public void ShowMessageDialog(RenderFragment message, RenderFragment? title = null, RenderFragment? okButtonCaption = null)
    {
        okButtonCaption ??= builder =>
        {
            var curElem = 0;
            builder.OpenElement(curElem++, "text");
            builder.AddContent(curElem++, "OK");
            builder.CloseElement();
        };
        MessageDialogParameters = new MessageDialogParameters
        {
            Message = message,
            Title = title,
            OkButtonText = okButtonCaption
        };
        MessageDialogVisible = true;
    }
}
