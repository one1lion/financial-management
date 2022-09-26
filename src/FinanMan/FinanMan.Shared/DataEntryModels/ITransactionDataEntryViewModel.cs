using FinanMan.Database.Models.Tables;
using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

public interface ITransactionDataEntryViewModel
{
    TransactionType TransactionType { get; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public int? AccountId { get; set; }
    public string? Memo { get; set; }
}

public static class ITransactionDataEntryViewModelExtensions
{
    public static Transaction ToEntityModel(this ITransactionDataEntryViewModel model) {
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
                    DepositReasonId = depositEntryViewModel.DepositReasonId!.Value,
                    Amount = depositEntryViewModel.Amount!.Value
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

        if(model.IsDeposit)
        {
            viewModel = new DepositEntryViewModel()
            {
                TransactionDate = model.TransactionDate,
                PostedDate = model.PostingDate,
                AccountId = model.AccountId,
                DepositReasonValueText = model.Deposit?.DepositReasonId.ToString(),
                Memo = model.Memo,
                Amount = model.Deposit?.Amount
            };
        }
        else if(model.IsPayment)
        {
            viewModel = new PaymentEntryViewModel()
            {
                TransactionDate = model.TransactionDate,
                PostedDate = model.PostingDate,
                AccountId = model.AccountId,
                Memo = model.Memo,
                PayeeValueText = model.Payment?.PayeeId.ToString()//,
                //LineItems = 
            };
        }
        else if(model.IsTransfer)
        {
            viewModel = new TransferEntryViewModel()
            {
                TransactionDate = model.TransactionDate,
                PostedDate = model.PostingDate,
                AccountId = model.AccountId,
                Memo = model.Memo,
                Amount = model.Transfer?.Amount
            };
        }
        else
        {
            throw new NotImplementedException();
        }

        return viewModel;
    }
}