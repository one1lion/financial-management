using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class Payee : LookupItemBase
{
    public Payee() : base(LookupListType.Payees) { }

    public virtual ICollection<LuCategory> Categories { get; set; } = new HashSet<LuCategory>();
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
}
