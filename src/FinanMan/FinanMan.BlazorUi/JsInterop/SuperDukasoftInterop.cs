using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi.JsInterop;
public static class SuperDukasoftInterop
{
    public static ValueTask CollapseSelectLists(IJSRuntime jsRuntime)
    {
        return jsRuntime.InvokeVoidAsync("dukaSoftFuncs.collapseSelectLists");
    }
}
