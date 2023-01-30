using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public abstract class TransactionViewModelValidator<TDataEntryViewModel> : AbstractValidator<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    public TransactionViewModelValidator()
    {
        AddBaseRules();
    }
    
    protected void AddBaseRules()
    {
        When(x => x.PostedDate.HasValue, () =>
        {
            RuleFor(x => x.PostedDate!.Value.Date)
                .GreaterThanOrEqualTo(x => x.TransactionDate.HasValue ? x.TransactionDate.Value.Date : DateTime.MinValue)
                .WithMessage("The posted date must be on or after the Transaction Date.");
        });
    }
}
