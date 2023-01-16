using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.AccountSummaryModels;

public class AccountSummaryViewModel
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } = default!;
    public decimal CurrentBalance { get; set; }
    public decimal Pending { get; set; }

    public string AccountBalanceDisplay => $"{CurrentBalance}{(Pending != 0 ? $" ({Pending})" : string.Empty)}";
    public decimal AdjustedBalance => CurrentBalance + Pending;
}

public static class AccountSummaryViewModelExtensions
{
    public static AccountSummaryViewModel ToAccountSummaryModel(this Account account)
    {
        // TODO: Update the account entity to include the flag for whether deposits add to or subtract from the total and use that flag to determine whether to use -1 or 1
        var depositMultiplier = account.AccountType.IncreaseOnPayment ? -1m : 1m;
        var postedTransactions = account.Transactions.Where(x => x.PostingDate.HasValue).ToList();
        var pendingTransactions = account.Transactions.Where(x => !x.PostingDate.HasValue).ToList();

        var postedDepositsTotal = postedTransactions.Where(x => x.TransactionType == TransactionType.Deposit).Sum(y => y.Total);
        var pendingDepositsTotal = pendingTransactions.Where(x => x.TransactionType == TransactionType.Deposit).Sum(y => y.Total);
        var postedPaymentsTotal = postedTransactions.Where(x => x.TransactionType == TransactionType.Payment).Sum(y => y.Total);
        var pendingPaymentsTotal = pendingTransactions.Where(x => x.TransactionType == TransactionType.Payment).Sum(y => y.Total);
        var postedTransferOutTotal = postedTransactions.Where(x => x.TransactionType == TransactionType.Transfer).Sum(y => y.Total);
        var pendingTransferOutTotal = pendingTransactions.Where(x => x.TransactionType == TransactionType.Transfer).Sum(y => y.Total);
        var postedTransferInTotal = account.Transfers.Where(x => x.Transaction.PostingDate.HasValue).Sum(y => y.Amount);
        var pendingTransferInTotal = account.Transfers.Where(x => !x.Transaction.PostingDate.HasValue).Sum(y => y.Amount);

        var accountSummary = new AccountSummaryViewModel()
        {
            AccountId = account.Id,
            AccountName = account.Name,
            CurrentBalance = depositMultiplier * (postedDepositsTotal - postedPaymentsTotal - postedTransferOutTotal + postedTransferInTotal),
            Pending = depositMultiplier * (pendingDepositsTotal - pendingPaymentsTotal - pendingTransferOutTotal + pendingTransferInTotal)
        };
        return accountSummary;
    }

}