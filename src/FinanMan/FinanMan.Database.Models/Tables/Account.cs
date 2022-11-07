using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class Account : IHasLookupListType
{
    [NotMapped]
    public LookupListType ListType => LookupListType.AccountTypes;
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int AccountTypeId { get; set; }

    public virtual LuAccountType AccountType { get; set; } = default!;
    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
    public virtual ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
    public virtual ICollection<Transfer> Transfers { get; set; } = new HashSet<Transfer>();
}
