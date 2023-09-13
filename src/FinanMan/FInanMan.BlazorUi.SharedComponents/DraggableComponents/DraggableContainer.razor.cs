using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.SharedComponents.DraggableComponents;

/// <summary>
/// A container that can be dragged around the screen.
/// </summary>
public partial class DraggableContainer
{
    /// <summary>
    /// The content to be displayed in the draggable container.
    /// </summary>
    [Parameter, AllowNull] public RenderFragment ChildContent { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();

    private string PositionStyles => _left.HasValue ? $"position: absolute;left: {_left}px; top: {_top}px; transform: unset" : string.Empty;

    private double? _left;
    private double? _top;

    private bool _dragging;
    
    private double? _offsetLeft;
    private double? _offsetTop;

    private CancellationTokenSource? _cts;

    private Task HandleMouseDown(MouseEventArgs e)
    {
        _offsetLeft = e.OffsetX;
        _offsetTop = e.OffsetY;
        return Task.CompletedTask;
    }

    private Task HandleDragStart(DragEventArgs e)
    {
        _dragging = true;
        _left = e.ClientX - _offsetLeft;
        _top = e.ClientY - _offsetTop;
        return InvokeAsync(StateHasChanged);
    }
    private Task HandleDragOver(DragEventArgs e)
    {
        _left = e.ClientX - _offsetLeft;
        _top = e.ClientY - _offsetTop;
        return InvokeAsync(StateHasChanged);
    }

    private async Task HandleDragEnd()
    {
        _dragging = false;
        await InvokeAsync(StateHasChanged);
    }
}
