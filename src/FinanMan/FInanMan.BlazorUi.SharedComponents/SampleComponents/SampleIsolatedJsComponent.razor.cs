﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.SharedComponents.SampleComponents;

/// <summary>
/// A shareable Isolated JavaScript component that will render html markup
/// </summary>
public partial class SampleIsolatedJsComponent
{
    [Inject, AllowNull] private IJSRuntime JsRuntime { get; set; }
    private Task<IJSObjectReference>? _module;
    private Task<IJSObjectReference> Module => _module ??= JsRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/FinanMan.BlazorUi.SharedComponents/js/MyInterop/MyInterop.js").AsTask();

    private string? _errMsg;
    
    private async Task HandleClick()
    {
        try
        {
            var module = await Module;
            await module.InvokeVoidAsync("showAlert", "Hello from C#!");
        }
        catch (Exception ex)
        {
            _errMsg = ex.Message;
        }
    }
}
