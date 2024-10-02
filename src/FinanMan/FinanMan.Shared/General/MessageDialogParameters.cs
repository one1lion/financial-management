using Microsoft.AspNetCore.Components;

namespace FinanMan.Shared.General;
public struct MessageDialogParameters
{
    public RenderFragment? Title { get; set; }
    public RenderFragment Message { get; set; }
    public RenderFragment OkButtonText { get; set; }
}
