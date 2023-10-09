using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.JsInterop;

public interface IMyIsolatedModule
{
    /// <summary>
    /// Send all pointer events to an element.
    /// </summary>
    /// <param name="element">The element that should receive all pointer events</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    ValueTask CapturePointerEvents(ElementReference element);

    /// <summary>
    /// Releases pointer events from an element.
    /// </summary>
    /// <param name="element">The element to release pointer events from.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    ValueTask ReleasePointerEvents(ElementReference element);
}