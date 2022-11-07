using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class Payee : IHasLookupListType
{
    [NotMapped]
    public LookupListType ListType => LookupListType.AccountType;
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    public virtual ICollection<LuCategory> Categories { get; set; } = new HashSet<LuCategory>();
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
}
