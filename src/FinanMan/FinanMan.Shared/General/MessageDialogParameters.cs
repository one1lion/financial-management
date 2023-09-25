using Microsoft.AspNetCore.Components;

namespace FinanMan.Shared.General;
public struct MessageDialogParameters
{
    public string? Title { get; set; }
    public string Message { get; set; }
    public string OkButtonText { get; set; }
}
