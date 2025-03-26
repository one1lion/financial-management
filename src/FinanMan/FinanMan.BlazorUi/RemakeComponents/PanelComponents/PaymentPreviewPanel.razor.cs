namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class PaymentPreviewPanel : TransactionPreviewPanel
{
    [Parameter] public string AccountName { get; set; } = "Checking Account";
    [Parameter] public string PayeeName { get; set; } = "Vendor/Payee";
    [Parameter] public decimal Amount { get; set; }

    protected override void OnInitialized()
    {
        Title = "Payment Preview";
    }
}