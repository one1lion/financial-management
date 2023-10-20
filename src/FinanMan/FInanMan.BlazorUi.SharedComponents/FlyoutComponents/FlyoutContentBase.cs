using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

/// <summary>
/// The base class for flyout content
/// </summary>
public abstract class FlyoutContentBase : ComponentBase, IDisposable
{
    /// <summary>
    /// The parent <see cref="Flyout"/> component
    /// </summary>
    [CascadingParameter(Name = "FlyoutParent")] public Flyout? FlyoutParent { get; set; }

    /// <summary>
    /// The method indicating whether or not the flyout can be closed
    /// </summary>
    /// <returns>
    /// True if the flyout can be closed, false otherwise
    /// </returns>
    public virtual Task<bool> CanCloseAsync() => Task.FromResult(true);
    
    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (FlyoutParent is not null)
        {
            FlyoutParent.CanClose = CanCloseAsync;
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }
    
    /// <inheritdoc/>
    public void Dispose()
    {
        if (FlyoutParent?.CanClose == CanCloseAsync)
        {
            FlyoutParent.CanClose = null;
        }
    }
}
