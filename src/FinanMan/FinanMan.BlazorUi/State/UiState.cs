using FinanMan.Shared.General;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.State;

public class UiState : BaseNotifyPropertyChanges, IUiState
{
    private RenderFragment? _flyoutContent;
    private bool _flyoutVisible;

    public RenderFragment? FlyoutContent { get => _flyoutContent; set => SetField(ref _flyoutContent, value); }
    public bool FlyoutVisible { get => _flyoutVisible; set => SetField(ref _flyoutVisible, value); }

}
