using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public class DepositEntryViewModelValidator : TransactionViewModelValidator<DepositEntryViewModel>
{
    public DepositEntryViewModelValidator()
    {
        When(x => x.PostedDate is not null, () =>
        {
            RuleFor(x => x.PostedDate)
                .NotNull()
                .GreaterThanOrEqualTo(x => x.TransactionDate)
                .WithMessage("The posted date must be on or after the Transaction Date.");
        });
    }
}
