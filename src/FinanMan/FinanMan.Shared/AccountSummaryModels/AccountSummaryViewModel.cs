using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.AccountSummaryModels;

public class AccountSummaryViewModel
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    public double CurrentBalance { get; set; }
    public double Pending { get; set; }

    public string AccountBalanceDisplay => $"{CurrentBalance}{(Pending != 0 ? $" ({Pending})" : string.Empty)}";
    public double AdjustedBalance => CurrentBalance + Pending;
}

public static class AccountSummaryViewModelExtensions
{
    public static AccountSummaryViewModel ToAccountSummaryModel(this Account account)
    {

        // TODO: Update the account entity to include the flag for whether payments add to or subtract from the total
        // TODO: Separate out pending vs posted
        var depositsTotal = account.Transactions.Sum(y => y.Deposit?.Amount ?? 0);
        var paymentsTotal = account.Transactions.Sum(y => y.Payment?.PaymentDetails.Sum(x => x.Amount) ?? 0);
        var transferOutTotal = account.Transactions.Sum(y => y.Transfer?.Amount ?? 0);
        var transferInTotal = account.Transfers.Sum(y => y.Amount);
        var accountSummary = new AccountSummaryViewModel()
        {
            AccountId = account.Id,
            AccountName = account.Name,
            CurrentBalance = depositsTotal - paymentsTotal - transferOutTotal + transferInTotal,
        };
        return accountSummary;
    }

}