using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi.SharedComponents.ModalComponents;
public partial class TemplatedQuestionDialog
{
    [Parameter] public RenderFragment? TitleContent { get; set; }
    [Parameter] public RenderFragment QuestionContent { get; set; }
    [Parameter] public string ConfirmButtonText { get; set; } = string.Empty;
    [Parameter] public string CancelButtonText { get; set; } = string.Empty;
    [Parameter] public string ConfirmButtonCssClasses { get; set; } = string.Empty;
    [Parameter] public string CancelButtonCssClasses { get; set; } = string.Empty;
    [Parameter] public bool ConfirmButtonDisabled { get; set; }
    [Parameter] public bool CancelButtonDisabled { get; set; }
    [Parameter] public bool ConfirmButtonVisible { get; set; }
    [Parameter] public bool CancelButtonVisible { get; set; }
    [Parameter] public bool Show { get; set; }
    [Parameter] public bool ShowDismiss { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnConfirmClicked { get; set; }
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
