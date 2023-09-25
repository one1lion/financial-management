using FinanMan.Shared.General;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;

namespace FinanMan.Shared.StateInterfaces;

public interface IUiState : INotifyPropertyChanged, INotifyPropertyChanging
{
    int SomeNum { get; set; }
    event Func<Task>? ActiveLanguageChanged;
    event Func<Task>? CollapseSelectLists;
    event Func<Task>? InitialUiLoadComplete;

    RenderFragment? FlyoutContent { get; }
    bool FlyoutVisible { get; }
    string ActiveLanguage { get; }

    bool MessageDialogVisible { get; set; }
    MessageDialogParameters MessageDialogParameters { get; set; }


    bool InitialUiLoaded { get; }

    void SetLanguage(string language);
    void CollapseAllSelectLists();
    void RaiseInitialUiLoadComplete();
    void DisplayFlyout(RenderFragment? content);
    void CollapseFlyout();
    void ShowMessageDialog(string message, string? title = null, string okButtonText = "OK");
}
