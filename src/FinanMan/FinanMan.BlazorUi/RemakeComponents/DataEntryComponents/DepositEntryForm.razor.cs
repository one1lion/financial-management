using FinanMan.BlazorUi.RemakeComponents.PanelComponents;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.RemakeComponents.DataEntryComponents;

public partial class DepositEntryForm
{
    private readonly ILookupListState _lookupListState;
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositEntryService;

    public DepositEntryForm(ILookupListState lookupListState, ITransactionEntryService<DepositEntryViewModel> depositEntryService)
    {
        _lookupListState = lookupListState;
        _depositEntryService = depositEntryService;

        // In a real implementation, these would be populated from the LookupListState
        _accounts =
        [
            new () { ValueText = "1", DisplayText = "My Checking Account" },
            new () { ValueText = "2", DisplayText = "My Savings Account" }
        ];

        _depositReasons =
        [
            new() { ValueText = "1", DisplayText = "Salary" },
            new() { ValueText = "2", DisplayText = "Interest" },
            new() { ValueText = "3", DisplayText = "Gift" }
        ];

        // Mock account overviews
        _accountOverviews =
        [
            new()
            {
                Name = "My Checking Account",
                CurrentBalance = 10000,
                BalanceChange = 3250,
                AdjustedBalance = 13250,
                IsSelected = true
            },
            new()
            {
                Name = "My Savings Account",
                CurrentBalance = 10000,
                BalanceChange = 0,
                AdjustedBalance = 10000,
                IsSelected = false
            }
        ];

        // Initialize with default values
        _deposit.TransactionDate = DateTime.Today;
        _deposit.Amount = 3250; // Example value
        _deposit.DepositReasonValueText = "1"; // Default to Salary
    }

    private DepositEntryViewModel _deposit = new();
    private List<AccountOverviewPanel.AccountOverviewItem> _accountOverviews = new();

    private List<AccountLookupViewModel> _accounts;
    private List<LookupItemViewModel<LuDepositReason>> _depositReasons;

    protected override async Task OnInitializedAsync()
    {
        await _lookupListState.InitializeAsync();

    }

    private Task HandleDepositSubmitted()
    {
        Console.WriteLine("Deposit submitted");
        // In a real implementation, this would save the deposit
        return Task.CompletedTask;
    }

}
