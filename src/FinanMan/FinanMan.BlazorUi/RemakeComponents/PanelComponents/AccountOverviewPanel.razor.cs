namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class AccountOverviewPanel
{
    [Parameter] public List<AccountOverviewItem>? Accounts { get; set; }

    public class AccountOverviewItem
    {
        public string Name { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
        public decimal BalanceChange { get; set; }
        public decimal AdjustedBalance { get; set; }
        public bool IsSelected { get; set; }
    }
}
