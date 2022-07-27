using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class LuLineItemType : LookupItemBase
{
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new HashSet<TransactionDetail>();
}
