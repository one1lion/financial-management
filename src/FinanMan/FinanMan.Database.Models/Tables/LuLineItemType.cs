using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class LuLineItemType : LookupItemBase
{
    [NotMapped]
    public override LookupListType ListType => LookupListType.LineItemTypes;
    public virtual ICollection<PaymentDetail> TransactionDetails { get; set; } = new HashSet<PaymentDetail>();
}
