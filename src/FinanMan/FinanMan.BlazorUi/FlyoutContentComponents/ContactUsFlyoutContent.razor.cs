using FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

namespace FinanMan.BlazorUi.FlyoutContentComponents;

public partial class ContactUsFlyoutContent : FlyoutContentBase
{
    private bool _allowClose;

    public override Task<bool> CanCloseAsync()
    {
        return Task.FromResult(_allowClose);
    }

    public new void Dispose()
    {
        if (FlyoutParent?.CanClose == CanCloseAsync)
        {
            FlyoutParent.CanClose = null;
        }
        base.Dispose();
    }
}