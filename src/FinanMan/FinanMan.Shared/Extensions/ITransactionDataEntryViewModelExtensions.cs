using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.Shared.Extensions;

public static class ITransactionDataEntryViewModelExtensions
{
    public static Transaction ToEntityModel(this ITransactionDataEntryViewModel model)
    {
        var transaction = new Transaction()
        {
            TransactionDate = model.TransactionDate!.Value,
            AccountId = model.AccountId!.Value,
            Memo = model.Memo,
            PostingDate = model.PostedDate,
            DateEntered = DateTime.UtcNow
        };

        switch (model)
        {
            case DepositEntryViewModel depositEntryViewModel:
                transaction.Deposit = new()
                {
                    DepositReasonId = depositEntryViewModel.DepositReasonId ?? 0,
                    Amount = depositEntryViewModel.Amount ?? 0
                };
                break;
            case PaymentEntryViewModel paymentEntryViewModel:
                break;
            case TransferEntryViewModel transferEntryViewModel:
                break;
            default:
                throw new NotImplementedException();
        }

        return transaction;
    }

    public static ITransactionDataEntryViewModel ToViewModel(this Transaction model)
    {
        ITransactionDataEntryViewModel viewModel;

        switch (model.TransactionType)
        {
            case TransactionType.Deposit:
                viewModel = new DepositEntryViewModel()
                {
                    TransactionDate = model.TransactionDate,
                    PostedDate = model.PostingDate,
                    AccountId = model.AccountId,
                    AccountName = model.Account?.Name,
                    DepositReasonValueText = model.Deposit?.DepositReasonId.ToString(),
                    Memo = model.Memo,
                    Amount = model.Deposit?.Amount
                };
                break;
            case TransactionType.Payment:
                viewModel = new PaymentEntryViewModel()
                {
                    TransactionDate = model.TransactionDate,
                    PostedDate = model.PostingDate,
                    AccountId = model.AccountId,
                    AccountName = model.Account?.Name,
                    PayeeName = model.Payment?.Payee?.Name,
                    Memo = model.Memo,
                    PayeeValueText = model.Payment?.PayeeId.ToString(),
                    LineItems = model.Payment?.PaymentDetails?.ToViewModel()?.ToList() ?? new List<PaymentDetailViewModel>()
                };
                break;
            case TransactionType.Transfer:
                viewModel = new TransferEntryViewModel()
                {
                    TransactionDate = model.TransactionDate,
                    PostedDate = model.PostingDate,
                    AccountId = model.AccountId,
                    AccountName = model.Account?.Name,
                    TargetAccountName = model.Transfer?.TargetAccount?.Name,
                    Memo = model.Memo,
                    Amount = model.Transfer?.Amount
                };
                break;
            default:
                throw new NotImplementedException();
        }

        return viewModel;
    }

    public static IEnumerable<ITransactionDataEntryViewModel> ToViewModel(this IEnumerable<Transaction> model) =>
        model.Select(x => x.ToViewModel());
    public static IEnumerable<TViewModel> ToViewModel<TViewModel>(this IEnumerable<Transaction> model)
        where TViewModel : ITransactionDataEntryViewModel =>
        model.ToViewModel().OfType<TViewModel>();
}

public static class LineItemViewModelExtensions
{
    public static PaymentDetail ToEntityModel(this PaymentDetailViewModel model) => new()
    {
        LineItemTypeId = model.LineItemTypeId ?? 0,
        Amount = model.Amount ?? 0,
        Detail = model.Detail,
    };

    public static PaymentDetailViewModel ToViewModel(this PaymentDetail model) => new()
    {
        LineItemTypeValueText = model.LineItemTypeId.ToString(),
        Amount = model.Amount,
        Detail = model.Detail,
    };

    public static IEnumerable<PaymentDetailViewModel> ToViewModel(this IEnumerable<PaymentDetail> model) =>
        model.Select(x => x.ToViewModel());
}
