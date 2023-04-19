using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;
public partial class PaymentDetailsTable
{
    [Parameter] public required PaymentEntryViewModel Payment { get; set; }

    private Task HandleDeletePaymentDetail(PaymentDetailViewModel paymentDetail)
    {
        Payment.LineItems.Remove(paymentDetail);
        return Task.CompletedTask;
    }
}
