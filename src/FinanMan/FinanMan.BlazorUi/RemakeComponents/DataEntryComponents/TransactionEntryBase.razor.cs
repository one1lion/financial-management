namespace FinanMan.BlazorUi.RemakeComponents.DataEntryComponents;

public partial class TransactionEntryBase
{
    [Parameter] public string Title { get; set; } = "Enter Transaction Details";
    [Parameter] public RenderFragment? FormContent { get; set; }
    [Parameter] public RenderFragment? AccountOverviewContent { get; set; }
    [Parameter] public RenderFragment? TransactionPreviewContent { get; set; }
    [Parameter] public RenderFragment? ReceiptUploadContent { get; set; }
    [Parameter] public bool ShowAccountOverview { get; set; } = true;
    [Parameter] public bool ShowTransactionPreview { get; set; } = true;
    [Parameter] public bool ShowReceiptUpload { get; set; } = true;
}