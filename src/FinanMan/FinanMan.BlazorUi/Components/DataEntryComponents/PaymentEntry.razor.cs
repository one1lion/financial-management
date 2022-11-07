using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class PaymentEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionEntryService<PaymentEntryViewModel> PaymentEntryService { get; set; } = default!;

    private PaymentEntryViewModel _newPayment = new();
    private PaymentDetailViewModel _newLineItem = new();
    private List<ILookupItemViewModel<int, AccountViewModel>>? _accounts;
    private List<ILookupItemViewModel<int, LuLineItemType>>? _lineItemTypes;
    private List<ILookupItemViewModel<int, PayeeViewModel>>? _payees;

    private EditForm? _paymentEntryEditForm;
    private EditForm? _lineItemEntryEditForm;

    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;
    private InputDate<DateTime?>? _transDateInput;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.Initialize();
        await Task.Delay(2000);
        _accounts = LookupListState.GetLookupItems<int, AccountViewModel>().ToList();
        _payees = LookupListState.GetLookupItems<int, PayeeViewModel>().ToList();
        _lineItemTypes = LookupListState.GetLookupItems<int, LuLineItemType>().ToList();
    }

    private async Task HandlePaymentSubmitted()
    {
        if (_paymentEntryEditForm?.EditContext is null) { return; }
        if (!_paymentEntryEditForm.EditContext.Validate())
        {
            return;
        }

        _currentResponse = default;
        _submitting = true;
        _currentResponse = await PaymentEntryService.AddTransactionAsync(_newPayment);
        if (!_currentResponse.WasError)
        {
            _newPayment = new();
            if (_transDateInput is not null && _transDateInput.Element.HasValue)
            {
                await _transDateInput.Element.Value.FocusAsync();
            }
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
        _newPayment.LineItems.Add(_newLineItem);
        _newLineItem = new();
    }
}