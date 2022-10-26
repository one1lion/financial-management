using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

public abstract class FlyoutContentBase : ComponentBase, IDisposable
{
    [CascadingParameter(Name = "FlyoutParent")] public Flyout? FlyoutParent { get; set; }

    public virtual Task<bool> CanCloseAsync() => Task.FromResult(true);
    
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (FlyoutParent is not null)
        {
            FlyoutParent.CanClose = CanCloseAsync;
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }
    
    public void Dispose()
    {
        if (FlyoutParent?.CanClose == CanCloseAsync)
        {
            FlyoutParent.CanClose = null;
        }
    }
}
