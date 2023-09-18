using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace FinanMan.BlazorUi.SharedComponents.DraggableComponents;

/// <summary>
/// A container that can be dragged around the screen.
/// </summary>
public partial class DraggableContainer
{
    [Inject, AllowNull] private IJSRuntime JsRuntime { get; set; }

    /// <summary>
    /// The content to be displayed in the draggable container.
    /// </summary>
    [Parameter, AllowNull] public RenderFragment ChildContent { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();

    private string PositionStyles => _left.HasValue ? $"position: absolute;left: {_left}px; top: {_top}px; transform: unset" : string.Empty;
    private bool applyDragging => _dragging && _left.HasValue;

    private double? _left;
    private double? _top;

    private bool _dragging;

    private double? _offsetLeft;
    private double? _offsetTop;

    private ElementReference _draggableContainerElemRef;
    private Task<IJSObjectReference>? _module;
    private Task<IJSObjectReference> Module => _module ??= JsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FinanMan.BlazorUi.SharedComponents/js/DraggableComponents/DraggableContainer.js").AsTask();

    private async Task HandleMouseDown(MouseEventArgs e)
    {
        var module = await Module;
        await module.InvokeVoidAsync("capturePointerEvents", _draggableContainerElemRef);
        _dragging = true;
        _offsetLeft = e.OffsetX;
        _offsetTop = e.OffsetY;
    }

    private Task HandleMouseUp(MouseEventArgs e)
    {
        _dragging = false;
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
        await InvokeAsync(StateHasChanged);
    }
}
