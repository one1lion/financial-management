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
            Id = model.TransactionId,
            TransactionDate = model.TransactionDate!.Value,
            Account = new Account()
            {
                Id = model.AccountId!.Value,
                Name = model.AccountName ?? string.Empty
            },
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
                    TransactionId = model.TransactionId,
                    DepositReasonId = depositEntryViewModel.DepositReasonId ?? 0,
                    Amount = depositEntryViewModel.Amount ?? 0
                };
                break;
            case PaymentEntryViewModel paymentEntryViewModel:
                transaction.Payment = new()
                {
                    TransactionId = model.TransactionId,
                    PayeeId = paymentEntryViewModel.PayeeId ?? 0,
                    PaymentDetails = paymentEntryViewModel.LineItems.ToEntityModel().ToList()
                };
                break;
            case TransferEntryViewModel transferEntryViewModel:
                transaction.Transfer = new()
                {
                    TransactionId = model.TransactionId,
                    TargetAccountId = transferEntryViewModel.TargetAccountId ?? 0,
                    Amount = transferEntryViewModel.Total
                };
                break;
            default:
                throw new NotImplementedException();
        }

        return transaction;
    }

    public static IEnumerable<Transaction> ToEntityModel(this IEnumerable<ITransactionDataEntryViewModel> model) =>
        model.Select(x => x.ToEntityModel());

    public static ITransactionDataEntryViewModel ToViewModel(this Transaction model)
    {
        ITransactionDataEntryViewModel viewModel;

        switch (model.TransactionType)
        {
            case TransactionType.Deposit:
                viewModel = new DepositEntryViewModel()
                {
                    AccountId = model.AccountId,
                    AccountName = model.Account.Name,
                    TargetAccountValueText = model.Account.Name,
                    DepositReasonValueText = model.Deposit?.DepositReasonId.ToString(),
                    Amount = model.Deposit?.Amount
                };
                break;
            case TransactionType.Payment:
                viewModel = new PaymentEntryViewModel()
                {
                    AccountName = model.Account.Name,
                    PayeeName = model.Payment?.Payee?.Name,
                    PayeeValueText = model.Payment?.PayeeId.ToString(),
                    LineItems = model.Payment?.PaymentDetails?.ToViewModel()?.ToList() ?? new List<PaymentDetailViewModel>()
                };
                break;
            case TransactionType.Transfer:
                viewModel = new TransferEntryViewModel()
                {
                    AccountName = model.Account.Name,
                    TargetAccountName = model.Transfer?.TargetAccount?.Name,
                    Amount = model.Transfer?.Amount
                };
                break;
            default:
                throw new NotImplementedException();
        }

        // Populate the common properties
        viewModel.TransactionId = model.Id;
        viewModel.TransactionDate = model.TransactionDate;
        viewModel.PostedDate = model.PostingDate;
        viewModel.AccountId = model.AccountId;
        viewModel.AccountName = model.Account?.Name;
        viewModel.Memo = model.Memo;

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

    public static IEnumerable<PaymentDetail> ToEntityModel(this IEnumerable<PaymentDetailViewModel> model) =>
        model.Select(x => x.ToEntityModel());
    
    public static PaymentDetailViewModel ToViewModel(this PaymentDetail model) => new()
    {
        LineItemTypeValueText = model.LineItemTypeId.ToString(),
        Amount = model.Amount,
        Detail = model.Detail,
    };

    public static IEnumerable<PaymentDetailViewModel> ToViewModel(this IEnumerable<PaymentDetail> model) =>
        model.Select(x => x.ToViewModel());
}
