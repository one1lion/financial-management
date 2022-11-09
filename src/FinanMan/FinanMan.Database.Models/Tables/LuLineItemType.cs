using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class LuLineItemType : LookupItemBase
{
    public LuLineItemType() : base(LookupListType.LineItemTypes) { }
    public virtual ICollection<PaymentDetail> TransactionDetails { get; set; } = new HashSet<PaymentDetail>();
}
