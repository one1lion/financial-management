using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public class LuDepositReason : LookupItemBase
{
    public virtual ICollection<Deposit> Deposits { get; set; } = new HashSet<Deposit>();
}