namespace FinanMan.BlazorUi.JsInterop;
public static class SuperDukaSoftInterop
{
    public static ValueTask CollapseSelectLists(IJSRuntime jsRuntime) 
        => jsRuntime.InvokeVoidAsync("dukaSoftFuncs.collapseSelectLists");

    public static ValueTask HandleDraggableContainerDragStart(IJSRuntime jsRuntime, DragEventArgs e) 
        => jsRuntime.InvokeVoidAsync("dukaSoftFuncs.handleDraggableContainerDragStart", e);

    public static ValueTask<bool> CopyTextToClipboard(IJSRuntime jsRuntime, string textToWrite) 
        => jsRuntime.InvokeAsync<bool>("dukaSoftFuncs.clipboardCopy.writeString", textToWrite);

    public static ValueTask<bool> CopyElementContentToClipboard(IJSRuntime jsRuntime, ElementReference element) 
        => jsRuntime.InvokeAsync<bool>("dukaSoftFuncs.clipboardCopy.writeElementContent", element);

    public static ValueTask<bool> CopyElementContentToClipboard(IJSRuntime jsRuntime, string selector) 
        => jsRuntime.InvokeAsync<bool>("dukaSoftFuncs.clipboardCopy.writeElementContent", selector);
}
