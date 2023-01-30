using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public class TransferEntryViewModelValidator : TransactionViewModelValidator<TransferEntryViewModel>
{
    public TransferEntryViewModelValidator() : base() { }
}
