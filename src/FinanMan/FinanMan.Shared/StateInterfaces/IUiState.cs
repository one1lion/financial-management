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

    bool InitialUiLoaded { get; }

    void SetLanguage(string language);
    void CollapseAllSelectLists();
    void RaiseInitialUiLoadComplete();
    void ShowFlyout(RenderFragment? content);
    void CollapseFlyout();
}
