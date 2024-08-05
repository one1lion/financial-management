using System.Diagnostics.CodeAnalysis;

using FinanMan.BlazorUi.SharedComponents.JsInterop;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace FinanMan.BlazorUi.SharedComponents.DraggableComponents;

/// <summary>
/// A container that can be dragged around the screen.
/// </summary>
public partial class DraggableContainer
{
    [Inject, AllowNull] private IMyIsolatedModule MyIsolatedModule { get; set; }
    /// <summary>
    /// The content to be displayed in the draggable container.
    /// </summary>
    [Parameter, AllowNull] public RenderFragment ChildContent { get; set; }
    /// <summary>
    /// Additional CSS classes to be applied to the container.
    /// </summary>
    [Parameter] public string? AdditionalCssClasses { get; set; }

    /// <summary>
    /// The action to be performed when the container is dragged.
    /// </summary>
    public event Func<Task>? OnDragStart;
    /// <summary>
    /// The action to be performed when the container is dropped.
    /// </summary>
    public event Func<Task>? OnDragEnd;

    private readonly string _id = Guid.NewGuid().ToString();

    private string PositionStyles => _left.HasValue ? $"position: absolute;left: {_left}px; top: {_top}px; transform: unset" : string.Empty;
    private bool applyDragging => _dragging && _left.HasValue;

    private double? _left;
    private double? _top;

    private bool _dragging;

    private double? _offsetLeft;
    private double? _offsetTop;

    private ElementReference _draggableContainerElemRef;

    private async Task HandleMouseDown(MouseEventArgs e)
    {
        await MyIsolatedModule.CapturePointerEvents(_draggableContainerElemRef);
        _dragging = true;
        _offsetLeft = e.OffsetX;
        _offsetTop = e.OffsetY;
        OnDragStart?.Invoke();
    }

    private Task HandleMouseUp(MouseEventArgs e)
    {
        _dragging = false;
        OnDragEnd?.Invoke();
        return Task.CompletedTask;
    }

    private Task HandleDragOver(DragEventArgs e)
    {
        if (!_dragging) { return Task.CompletedTask; }
        _left = e.ClientX - _offsetLeft;
        _top = e.ClientY - _offsetTop;
        return InvokeAsync(StateHasChanged);
    }

    private async Task HandleDragEnd()
    {
        _dragging = false;
        OnDragEnd?.Invoke();
        await InvokeAsync(StateHasChanged);
    }
}
