using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.SharedComponents.ModalComponents;

/// <summary>
/// A shareable Question Dialog component that will render html markup
/// </summary>
public partial class TemplatedQuestionDialog
{
    /// <summary>
    /// The content to display in the modal's header
    /// </summary>
    [Parameter] public RenderFragment? TitleContent { get; set; }
    /// <summary>
    /// The content to display in the modal's body
    /// </summary>
    [Parameter, EditorRequired, AllowNull] public RenderFragment QuestionContent { get; set; }
    /// <summary>
    /// The text to use for the confirm button
    /// </summary>
    [Parameter] public string ConfirmButtonText { get; set; } = string.Empty;
    /// <summary>
    /// The text to use for the cancel button
    /// </summary>
    [Parameter] public string CancelButtonText { get; set; } = string.Empty;
    /// <summary>
    /// Additional css classes to apply to the confirm button
    /// </summary>
    [Parameter] public string ConfirmButtonCssClasses { get; set; } = string.Empty;
    /// <summary>
    /// Additional css classes to apply to the cancel button
    /// </summary>
    [Parameter] public string CancelButtonCssClasses { get; set; } = string.Empty;
    /// <summary>
    /// Whether or not the confirm button is disabled
    /// </summary>
    [Parameter] public bool ConfirmButtonDisabled { get; set; }
    /// <summary>
    /// Whether or not the cancel button is disabled
    /// </summary>
    [Parameter] public bool CancelButtonDisabled { get; set; }
    /// <summary>
    /// Whether or not the confirm button is visible
    /// </summary>
    [Parameter] public bool ConfirmButtonVisible { get; set; }
    /// <summary>
    /// Whether or not the cancel button is visible
    /// </summary>
    [Parameter] public bool CancelButtonVisible { get; set; }
    /// <summary>
    /// Whether or not the modal is shown
    /// </summary>
    [Parameter] public bool Show { get; set; }
    /// <summary>
    /// Whether or not to show the dismiss button
    /// </summary>
    [Parameter] public bool ShowDismiss { get; set; }
    /// <summary>
    /// The event callback for when the show property changes
    /// </summary>
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    /// <summary>
    /// The event callback for when the confirm button is clicked
    /// </summary>
    [Parameter] public EventCallback OnConfirmClicked { get; set; }
    /// <summary>
    /// The event callback for when the cancel button is clicked
    /// </summary>
    [Parameter] public EventCallback OnCancelClicked { get; set; }

    private async Task HandleConfirmClicked()
    {
        if (OnConfirmClicked.HasDelegate)
        {
            await OnConfirmClicked.InvokeAsync();
        }
        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }
    }

    private async Task HandleCancelClicked()
    {
        Show = false;
        if (OnCancelClicked.HasDelegate)
        {
            await OnCancelClicked.InvokeAsync();
        }
        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }
    }
}
