using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class Account
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int AccountTypeId { get; set; }

    public virtual LuAccountType AccountType { get; set; } = default!;
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
