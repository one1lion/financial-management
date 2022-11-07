using FluentValidation;

namespace FinanMan.Shared.DataEntryModels;

public abstract class TransactionViewModelValidator<TDataEntryViewModel> : AbstractValidator<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{ }    
