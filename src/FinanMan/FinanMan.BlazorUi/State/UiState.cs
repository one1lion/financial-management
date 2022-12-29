using FinanMan.Shared.General;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.State;

public class UiState : BaseNotifyPropertyChanges, IUiState
{
    #region Backing fields
    private RenderFragment? _flyoutContent;
    private bool _flyoutVisible;
    #endregion Backing Fields

    public RenderFragment? FlyoutContent { get => _flyoutContent; set => SetField(ref _flyoutContent, value); }
    public bool FlyoutVisible { get => _flyoutVisible; set => SetField(ref _flyoutVisible, value); }

}
