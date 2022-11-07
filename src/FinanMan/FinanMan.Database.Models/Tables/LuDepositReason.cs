using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public class LuDepositReason : LookupItemBase
{
    [NotMapped]
    public override LookupListType ListType => LookupListType.DepositReasons;
    public virtual ICollection<Deposit> Deposits { get; set; } = new HashSet<Deposit>();
}