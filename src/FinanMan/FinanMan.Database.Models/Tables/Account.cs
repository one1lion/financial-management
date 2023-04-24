using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class Account : LookupItemBase
{
    public Account() : base(LookupListType.Accounts) { }

    public int AccountTypeId { get; set; }
    public DateTime? ClosedDate { get; set; }

    public virtual LuAccountType AccountType { get; set; } = default!;
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    public virtual ICollection<Transfer> Transfers { get; set; } = new HashSet<Transfer>();
}
