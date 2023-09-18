namespace FinanMan.BlazorUi.JsInterop;
public static class SuperDukasoftInterop
{
    public static ValueTask CollapseSelectLists(IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeVoidAsync("dukaSoftFuncs.collapseSelectLists");
    }

    public static ValueTask HandleDraggableContainerDragStart(IJSRuntime jsRuntime, DragEventArgs e)
    {
        return jsRuntime.InvokeVoidAsync("dukaSoftFuncs.handleDraggableContainerDragStart", e);
    }
}
