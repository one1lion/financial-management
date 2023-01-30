using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModelValidator : TransactionViewModelValidator<PaymentEntryViewModel>
{
    public PaymentEntryViewModelValidator() : base() { }
}
