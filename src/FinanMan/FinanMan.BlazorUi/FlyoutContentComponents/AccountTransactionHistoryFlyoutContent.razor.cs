using FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

namespace FinanMan.BlazorUi.FlyoutContentComponents;
public partial class AccountTransactionHistoryFlyoutContent : FlyoutContentBase
{
    [Parameter] public required string AccountName { get; set; }
    
    private bool _allowClose = true;

    public override Task<bool> CanCloseAsync()
    {
        // TODO: If we add something like a fillable form, don't allow close when filled in and
        //       not submitted.
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
