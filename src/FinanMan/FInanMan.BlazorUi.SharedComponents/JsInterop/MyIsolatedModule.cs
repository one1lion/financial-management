using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FinanMan.BlazorUi.SharedComponents.JsInterop;

/// <summary>
/// Represents an isolated module for capturing and releasing pointer events for a specified element.
/// </summary>
public class MyIsolatedModule : IMyIsolatedModule
{
    private ValueTask<IJSObjectReference>? _module;
    private readonly IJSRuntime _jsRuntime;

    private ValueTask<IJSObjectReference> Module => _module ??= _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/FinanMan.BlazorUi.SharedComponents/js/JsInterop/MyInterop.js");

    /// <summary>
    /// Instantiates a new instance of the <see cref="MyIsolatedModule"/> class.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript Runtime</param>
    public MyIsolatedModule(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Send all pointer events to an element.
    /// </summary>
    /// <param name="element">The element that should receive all pointer events</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async ValueTask CapturePointerEvents(ElementReference element)
    {
        var module = await Module;
        await module.InvokeVoidAsync("capturePointerEvents", element);
    }

    /// <summary>
    /// Releases pointer events from an element.
    /// </summary>
    /// <param name="element">The element to release pointer events from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async ValueTask ReleasePointerEvents(ElementReference element)
    {
        var module = await Module;
        await module.InvokeVoidAsync("releasePointerEvents", element);
    }
}
