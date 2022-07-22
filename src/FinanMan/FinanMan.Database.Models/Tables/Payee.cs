namespace FinanMan.Database.Models.Tables;

public partial class Payee
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual ICollection<LuCategory> Categories { get; set; } = new HashSet<LuCategory>();
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
}
