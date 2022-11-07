using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModelValidator : TransactionViewModelValidator<PaymentEntryViewModel>
{
    public PaymentEntryViewModelValidator()
    {
        When(x => x.PostedDate is not null, () =>
        {
            RuleFor(x => x.PostedDate)
                .NotNull()
                .GreaterThanOrEqualTo(x => x.TransactionDate)
                .WithMessage("The posted date must be on or after the Payment Date.");
        });
    }
}
