using FinanMan.BlazorUi.RemakeComponents.PanelComponents;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.RemakeComponents.DataEntryComponents;

public partial class PaymentEntryForm
{
    private readonly ILookupListState _lookupListState;
    private readonly ITransactionEntryService<PaymentEntryViewModel> _paymentEntryService;

    public PaymentEntryForm(ILookupListState lookupListState, ITransactionEntryService<PaymentEntryViewModel> paymentEntryService)
    {
        _lookupListState = lookupListState;
        _paymentEntryService = paymentEntryService;

        // In a real implementation, these would be populated from the LookupListState
        _accounts =
        [
            new() { ValueText = "1", DisplayText = "My Checking Account" },
            new() { ValueText = "2", DisplayText = "My Savings Account" }
        ];

        _lineItemTypes =
        [
            new() { ValueText = "1", DisplayText = "Groceries" },
            new() { ValueText = "2", DisplayText = "Utilities" },
            new() { ValueText = "3", DisplayText = "Entertainment" }
        ];

        _payees =
        [
            new() { ValueText = "1", DisplayText = "Grocery Store" },
            new() { ValueText = "2", DisplayText = "Electric Company" },
            new() { ValueText = "3", DisplayText = "Internet Provider" }
        ];

        // Mock account overviews
        _accountOverviews =
        [
            new ()
            {
                Name = "My Checking Account",
                CurrentBalance = 10000,
                BalanceChange = -240,
                AdjustedBalance = 9760,
                IsSelected = true
            },
            new ()
            {
                Name = "My Savings Account",
                CurrentBalance = 10000,
                BalanceChange = 0,
                AdjustedBalance = 10000,
                IsSelected = false
            }
        ];

        // Initialize with default date
        _payment.TransactionDate = DateTime.Today;
    }

    private PaymentEntryViewModel _payment = new();
    private PaymentDetailViewModel _newLineItem = new();
    private List<AccountOverviewPanel.AccountOverviewItem> _accountOverviews = new();
    private decimal TotalAmount => _payment.LineItems?.Sum(item => item.Amount) ?? 0;

    private List<AccountLookupViewModel> _accounts;
    private List<LookupItemViewModel<LuLineItemType>> _lineItemTypes;
    private List<PayeeLookupViewModel> _payees;

    protected override async Task OnInitializedAsync()
    {
        await _lookupListState.InitializeAsync();
    }

    private Task HandlePaymentSubmitted()
    {
        Console.WriteLine("Payment submitted");
        // In a real implementation, this would save the payment
        return Task.CompletedTask;
    }

    private void HandleAddLineItem()
    {
        if (_newLineItem.Amount != 0 && !string.IsNullOrEmpty(_newLineItem.LineItemTypeValueText))
        {
            _payment.LineItems ??= [];

            _payment.LineItems.Add(new()
            {
                LineItemTypeValueText = _newLineItem.LineItemTypeValueText,
                Detail = _newLineItem.Detail,
                Amount = _newLineItem.Amount
            });

            _newLineItem = new();
        }
    }

    private void HandleRemoveLineItem(PaymentDetailViewModel item)
    {
        _payment.LineItems?.Remove(item);
    }
}
