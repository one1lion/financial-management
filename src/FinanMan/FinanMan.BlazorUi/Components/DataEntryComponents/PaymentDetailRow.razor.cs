using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;
public partial class PaymentDetailRow
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;

    [Parameter] public required PaymentDetailViewModel PaymentDetail { get; set; }
    [Parameter] public EventCallback<PaymentDetailViewModel> PaymentDetailChanged { get; set; }
    [Parameter] public EventCallback<PaymentDetailViewModel> OnDeletePaymentDetailClicked { get; set; }


    private PaymentDetailViewModel _editPaymentDetail = new();
    private List<LookupItemViewModel<LuLineItemType>> _lineItemTypes = new();
    private bool _editing;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.InitializeAsync();
        _lineItemTypes = LookupListState.GetLookupItems<LookupItemViewModel<LuLineItemType>>().ToList();
        await InvokeAsync(StateHasChanged);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
        if (parameters.TryGetValue<PaymentDetailViewModel>(nameof(PaymentDetail), out var paymentDetail))
        {
            _editPaymentDetail = (PaymentDetailViewModel)paymentDetail.Clone();
        }
        await base.SetParametersAsync(ParameterView.Empty);
    }

    private Task HandleEditClicked()
    {
        _editPaymentDetail = (PaymentDetailViewModel)PaymentDetail.Clone();
        _editing = true;
        return Task.CompletedTask;
    }

    private Task HandleDeleteClicked()
    {
        if (OnDeletePaymentDetailClicked.HasDelegate)
        {
            return OnDeletePaymentDetailClicked.InvokeAsync(PaymentDetail);
        }
        return Task.CompletedTask;
    }

    private Task HandleSaveClicked()
    {
        PaymentDetail.LineItemTypeValueText = _editPaymentDetail.LineItemTypeValueText;
        PaymentDetail.Amount = _editPaymentDetail.Amount;
        PaymentDetail.Detail = _editPaymentDetail.Detail;
        _editing = false;
        return Task.CompletedTask;
    }

    private Task HandleCancelClicked()
    {
        _editing = false;
        return Task.CompletedTask;
    }

}
