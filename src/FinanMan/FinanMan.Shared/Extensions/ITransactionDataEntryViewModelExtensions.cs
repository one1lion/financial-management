using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.Shared.Extensions;

public static class ITransactionDataEntryViewModelExtensions
{
    public static Transaction ToEntityModel(this ITransactionDataEntryViewModel model, bool includeNavProperties = true)
    {
        var transaction = new Transaction()
        {
            Id = model.TransactionId,
            TransactionDate = model.TransactionDate!.Value,
            Account = includeNavProperties ? new Account()
            {
                Id = model.AccountId!.Value,
                Name = model.AccountName ?? string.Empty
            } : null!,
            AccountId = model.AccountId!.Value,
            Memo = model.Memo,
            PostingDate = model.PostedDate,
            DateEntered = model.TransactionId == 0 ? DateTime.UtcNow : model.DateEntered
        };

        switch (model)
        {
            case DepositEntryViewModel depositEntryViewModel:
                transaction.Deposit = new()
                {
                    Id = depositEntryViewModel.DepositId,
                    TransactionId = model.TransactionId,
                    DepositReasonId = depositEntryViewModel.DepositReasonId ?? 0,
                    DepositReason = includeNavProperties ? new LuDepositReason()
                    {
                        Id = depositEntryViewModel.DepositReasonId ?? 0,
                        Name = depositEntryViewModel.DepositReasonDisplayText ?? string.Empty
                    } : null!,
                    Amount = depositEntryViewModel.Amount ?? 0,
                    Transaction = null!
                };
                break;
            case PaymentEntryViewModel paymentEntryViewModel:
                transaction.Payment = new()
                {
                    Id = paymentEntryViewModel.PaymentId,
                    TransactionId = model.TransactionId,
                    PayeeId = paymentEntryViewModel.PayeeId ?? 0,
                    Payee = includeNavProperties ? new Payee()
                    {
                        Id = paymentEntryViewModel.PayeeId ?? 0,
                        Name = paymentEntryViewModel.PayeeName ?? string.Empty
                    } : null!,
                    PaymentDetails = paymentEntryViewModel.LineItems.ToEntityModel(includeNavProperties).ToList(),
                    Transaction = null!
                };
                break;
            case TransferEntryViewModel transferEntryViewModel:
                transaction.Transfer = new()
                {
                    Id = transferEntryViewModel.TransferId,
                    TransactionId = model.TransactionId,
                    TargetAccountId = transferEntryViewModel.TargetAccountId ?? 0,
                    TargetAccount = includeNavProperties ? new Account()
                    {
                        Id = transferEntryViewModel.TargetAccountId ?? 0,
                        Name = transferEntryViewModel.TargetAccountName ?? string.Empty
                    } : null!,
                    Amount = transferEntryViewModel.Total,
                    Transaction = null!
                };
                break;
            default:
                throw new NotImplementedException();
        }

        return transaction;
    }

    public static Transaction Patch(this Transaction transaction, ITransactionDataEntryViewModel model)
    {
        transaction.TransactionDate = model.TransactionDate!.Value;
        if (transaction.AccountId != model.AccountId)
        {
            transaction.AccountId = model.AccountId!.Value;
            transaction.Account = null!;
        }
        transaction.Memo = model.Memo;
        transaction.PostingDate = model.PostedDate;

        switch (model)
        {
            case DepositEntryViewModel depositEntryViewModel:
                if (transaction.Deposit is null) { break; }

                if (transaction.Deposit.DepositReasonId != depositEntryViewModel.DepositReasonId && depositEntryViewModel.DepositReasonId.HasValue)
                {
                    transaction.Deposit.DepositReasonId = depositEntryViewModel.DepositReasonId.Value;
                }
                transaction.Deposit.Amount = depositEntryViewModel.Amount ?? 0;
                break;
            case PaymentEntryViewModel paymentEntryViewModel:
                if (transaction.Payment is null) { break; }

                if (transaction.Payment.PayeeId != paymentEntryViewModel.PayeeId && paymentEntryViewModel.PayeeId.HasValue)
                {
                    transaction.Payment.PayeeId = paymentEntryViewModel.PayeeId.Value;
                }

                var existLineItems = transaction.Payment.PaymentDetails;

                var paymentDetails = paymentEntryViewModel.LineItems.ToEntityModel(false).ToList();
                var lisToAdd = paymentDetails.Where(x => x.Id == 0).ToList();
                var lisToRemove = existLineItems.Where(x => !paymentDetails.Any(y => y.Id == x.Id)).ToArray();
                var lisToUpdate = existLineItems.Where(x => paymentDetails.Any(y => y.Id == x.Id)).ToList();

                foreach (var liToAdd in lisToAdd)
                {
                    transaction.Payment.PaymentDetails.Add(liToAdd);
                }

                foreach (var liToRemove in lisToRemove)
                {
                    transaction.Payment.PaymentDetails.Remove(liToRemove);
                }

                foreach (var liToUpdate in lisToUpdate)
                {
                    liToUpdate.Patch(paymentEntryViewModel.LineItems.First(x => x.Id == liToUpdate.Id));
                }

                break;
            case TransferEntryViewModel transferEntryViewModel:
                if (transaction.Transfer is null) { break; }

                if (transaction.Transfer.TargetAccountId != transferEntryViewModel.TargetAccountId && transferEntryViewModel.TargetAccountId.HasValue)
                {
                    transaction.Transfer.TargetAccountId = transferEntryViewModel.TargetAccountId.Value;
                }

                transaction.Transfer.Amount = transferEntryViewModel.Total;
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
                    DepositId = model.Deposit?.Id ?? 0,
                    AccountId = model.AccountId,
                    AccountName = model.Account.Name,
                    TargetAccountValueText = model.Account.Name,
                    DepositReasonValueText = model.Deposit?.DepositReasonId.ToString(),
                    DepositReasonDisplayText = model.Deposit?.DepositReason?.Name,
                    Amount = model.Deposit?.Amount
                };
                break;
            case TransactionType.Payment:
                viewModel = new PaymentEntryViewModel()
                {
                    PaymentId = model.Payment?.Id ?? 0,
                    AccountName = model.Account.Name,
                    PayeeName = model.Payment?.Payee?.Name,
                    PayeeValueText = model.Payment?.PayeeId.ToString(),
                    LineItems = model.Payment?.PaymentDetails?.ToViewModel()?.ToList() ?? new List<PaymentDetailViewModel>()
                    // TODO: Populate categories
                };
                break;
            case TransactionType.Transfer:
                viewModel = new TransferEntryViewModel()
                {
                    TransferId = model.Transfer?.Id ?? 0,
                    AccountName = model.Account.Name,
                    TargetAccountName = model.Transfer?.TargetAccount?.Name,
                    TargetAccountValueText = model.Transfer?.TargetAccountId.ToString(),
                    Amount = model.Transfer?.Amount
                };
                break;
            default:
                throw new NotImplementedException();
        }

        // Populate the common properties
        viewModel.TransactionId = model.Id;
        viewModel.DateEntered = model.DateEntered;
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

public static class PaymentDetailExtensions
{
    public static PaymentDetail ToEntityModel(this PaymentDetailViewModel model, bool includeNavProperties = true) => new()
    {
        Id = model.Id,
        LineItemTypeId = model.LineItemTypeId ?? 0,
        LineItemType = includeNavProperties ? new LuLineItemType()
        {
            Id = model.LineItemTypeId ?? 0,
            Name = model.LineItemTypeName ?? string.Empty,
            SortOrder = model.SortOrder
        } : null!,
        Amount = model.Amount ?? 0,
        Detail = model.Detail
    };

    public static IEnumerable<PaymentDetail> ToEntityModel(this IEnumerable<PaymentDetailViewModel> model, bool includeNavProperties = true) =>
        model.Select(x => x.ToEntityModel(includeNavProperties));

    public static PaymentDetailViewModel ToViewModel(this PaymentDetail model) => new()
    {
        Id = model.Id,
        LineItemTypeValueText = model.LineItemTypeId.ToString(),
        LineItemTypeName = model.LineItemType?.Name,
        SortOrder = model.LineItemType?.SortOrder ?? 0,
        Amount = model.Amount,
        Detail = model.Detail
    };

    public static IEnumerable<PaymentDetailViewModel> ToViewModel(this IEnumerable<PaymentDetail> model) =>
        model.Select(x => x.ToViewModel());

    public static PaymentDetail Patch(this PaymentDetail model, PaymentDetailViewModel viewModel)
    {
        model.LineItemTypeId = viewModel.LineItemTypeId ?? 0;
        model.LineItemType = new LuLineItemType()
        {
            Id = viewModel.LineItemTypeId ?? 0,
            Name = viewModel.LineItemTypeName ?? string.Empty,
            SortOrder = viewModel.SortOrder
        };
        model.Amount = viewModel.Amount ?? 0;
        model.Detail = viewModel.Detail;

        return model;
    }
}
