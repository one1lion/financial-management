namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class TransferPreviewPanel : TransactionPreviewPanel
{
    [Parameter] public string FromAccountName { get; set; } = "Checking";
    [Parameter] public string ToAccountName { get; set; } = "Savings";
    [Parameter] public decimal? Amount { get; set; }
    [Parameter] public decimal FromBalance { get; set; }
    [Parameter] public decimal ToBalance { get; set; }

    protected override void OnInitialized()
    {
        Title = "Transfer Preview";
    }
}
