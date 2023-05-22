using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi.SharedComponents.SampleComponents;
public partial class SampleIsolatedJsComponent
{
    [Inject, AllowNull] private IJSRuntime JsRuntime { get; set; }
    private Task<IJSObjectReference>? _module;
    private Task<IJSObjectReference> Module => _module ??= JsRuntime.InvokeAsync<IJSObjectReference>("import", "/_content/FinanMan.BlazorUi.SharedComponents/js/MyInterop.js").AsTask();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var module = await Module;
                await module.InvokeVoidAsync("showAlert", "Hello from C#!");
            }
            catch (Exception ex)
            {
                Debug.Assert(ex is null);
            }
        }
    }   
}
