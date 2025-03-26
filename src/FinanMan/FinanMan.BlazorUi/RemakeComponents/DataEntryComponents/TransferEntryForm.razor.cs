using FinanMan.BlazorUi.RemakeComponents.PanelComponents;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.RemakeComponents.DataEntryComponents;

public partial class TransferEntryForm
{
    private readonly ILookupListState _lookupListState;
    private readonly ITransactionEntryService<TransferEntryViewModel> _transferEntryService;

    public TransferEntryForm(ILookupListState lookupListState, ITransactionEntryService<TransferEntryViewModel> transferEntryService)
    {
        _lookupListState = lookupListState;
        _transferEntryService = transferEntryService;

        // In a real implementation, these would be populated from the LookupListState
        _accounts =
        [
            new() { ValueText = "1", DisplayText = "My Checking Account" },
            new() { ValueText = "2", DisplayText = "My Savings Account" }
        ];

        // Mock account overviews
        _accountOverviews =
        [
            new()
            {
                Name = "My Checking Account",
                CurrentBalance = 10000,
                BalanceChange = -240,
                AdjustedBalance = 9760,
                IsSelected = true
            },
            new()
            {
                Name = "My Savings Account",
                CurrentBalance = 10000,
                BalanceChange = 240,
                AdjustedBalance = 10240,
                IsSelected = true
            }
        ];

        // Initialize with default values
        _transfer.TransactionDate = DateTime.Today;
        _transfer.Amount = 240; // Example value
        _transfer.SourceAccountValueText = "1"; // Default from Checking
        _transfer.TargetAccountValueText = "2"; // Default to Savings
    }

    private TransferEntryViewModel _transfer = new();
    private List<AccountOverviewPanel.AccountOverviewItem> _accountOverviews = new();

    private List<AccountLookupViewModel> _accounts;

    protected override async Task OnInitializedAsync()
    {
        await _lookupListState.InitializeAsync();
    }

    private Task HandleTransferSubmitted()
    {
        Console.WriteLine("Transfer submitted");
        // In a real implementation, this would save the transfer
        return Task.CompletedTask;
    }

}
