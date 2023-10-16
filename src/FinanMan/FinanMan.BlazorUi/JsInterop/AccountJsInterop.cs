using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi.JsInterop;
public static class AccountJsInterop
{
    public static ValueTask<string> SomeFunction(IJSRuntime jsRuntime, string withParams)
    {
        return jsRuntime.InvokeAsync<string>("accountJsFuncs.someFunction", withParams);
    }
    
    public static ValueTask SomeFunction2(IJSRuntime jsRuntime, string withParams)
    {
        return jsRuntime.InvokeVoidAsync("accountJsFuncs.someFunction2", withParams);
    }
}
