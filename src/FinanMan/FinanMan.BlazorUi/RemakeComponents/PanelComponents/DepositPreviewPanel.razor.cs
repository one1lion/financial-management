namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class DepositPreviewPanel : TransactionPreviewPanel
{
    [Parameter] public string AccountName { get; set; } = "Checking Account";
    [Parameter] public string SourceName { get; set; } = "Salary";
    [Parameter] public decimal? Amount { get; set; }

    protected override void OnInitialized()
    {
        Title = "Deposit Preview";
    }
}
