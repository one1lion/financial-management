using FinanMan.BlazorUi.State;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.BlazorUi;
public partial class App : IDisposable
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] private IUiState UiState { get; set; } = default!;

    protected override void OnInitialized()
    {
    }

    public void Dispose()
    {
    }
}
