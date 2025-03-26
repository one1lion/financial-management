namespace FinanMan.BlazorUi.RemakeComponents.PanelComponents;

public partial class TransactionPreviewPanel
{
    [Parameter] public string Title { get; set; } = "Transaction Preview";
    [Parameter] public RenderFragment? ChildContent { get; set; }
}