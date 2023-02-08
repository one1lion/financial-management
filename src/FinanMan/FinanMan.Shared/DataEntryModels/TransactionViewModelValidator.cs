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
        // TODO: Make sure the property name associated with the validation error is just PostedDate (it is showing up as PostedDate.Value.Date)
        When(x => x.PostedDate.HasValue, () =>
        {
            RuleFor(x => x.PostedDate!.Value.Date)
                .GreaterThanOrEqualTo(x => x.TransactionDate.HasValue ? x.TransactionDate.Value.Date : DateTime.MinValue)
                .WithName(nameof(ITransactionDataEntryViewModel.PostedDate))
                .WithMessage("The posted date must be on or after the Transaction Date.");
        });
    }
}
