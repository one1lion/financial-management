using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Shared;
public partial class TopBar
{
    [Inject, AllowNull] private IUiState UiState { get; set; }
}