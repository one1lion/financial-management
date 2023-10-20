using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class PaymentEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionEntryService<PaymentEntryViewModel> PaymentEntryService { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;

    [Parameter] public PaymentEntryViewModel? Payment { get; set; }

    private PaymentEntryViewModel _editPayment = new();
    private PaymentDetailViewModel _newLineItem = new();
    private List<AccountLookupViewModel>? _accounts;
    private List<LookupItemViewModel<LuLineItemType>>? _lineItemTypes;
    private List<PayeeLookupViewModel>? _payees;

    private EditForm? _paymentEntryEditForm;
    private EditForm? _lineItemEntryEditForm;

    private ResponseModelBase<int>? _currentResponse;
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Field 'PaymentEntry._pmtDateInput' is never assigned to, and will always have its default value null
    private InputDate<DateTime?>? _pmtDateInputRef;
#pragma warning restore CS0649 // Field 'PaymentEntry._pmtDateInput' is never assigned to, and will always have its default value null
#pragma warning restore IDE0044 // Add readonly modifier
    private bool _submitting;
    private bool _showAddAccount;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.InitializeAsync();
        _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
        _payees = LookupListState.GetLookupItems<PayeeLookupViewModel>().ToList();
        _lineItemTypes = LookupListState.GetLookupItems<LookupItemViewModel<LuLineItemType>>().ToList();
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (parameters.TryGetValue<PaymentEntryViewModel>(nameof(Payment), out var payment))
        {
            _editPayment = payment;
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }

    private async Task HandlePaymentSubmitted()
    {
        if (_paymentEntryEditForm?.EditContext is null) { return; }
        if (!_paymentEntryEditForm.EditContext.Validate())
        {
            return;
        }

        if (_submitting) { return; }

        _currentResponse = default;
        _submitting = true;
        var lookups = (_accounts ?? new()).OfType<ILookupItemViewModel>().Union(_payees ?? new());
        _editPayment.UpdateAccountName(lookups);
        _currentResponse = await PaymentEntryService.AddTransactionAsync(_editPayment);
        if (!_currentResponse.WasError)
        {
            _editPayment = new();
            if (_pmtDateInputRef is not null && _pmtDateInputRef.Element.HasValue)
            {
                await _pmtDateInputRef.Element.Value.FocusAsync();
            }
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;

        return;
    }

    private void HandleLineItemSubmitted()
    {
        if (_lineItemEntryEditForm?.EditContext is null) { return; }
        if (!_lineItemEntryEditForm.EditContext.Validate())
        {
            return;
        }
        _editPayment.LineItems.Add(_newLineItem);
        _newLineItem = new();
    }
}