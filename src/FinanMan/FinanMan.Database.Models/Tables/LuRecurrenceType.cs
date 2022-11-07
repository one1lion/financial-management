using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class LuRecurrenceType : LookupItemBase<RecurrenceType>
{
    [NotMapped]
    public override LookupListType ListType => LookupListType.RecurrenceTypes;
    public string DisplayText { get; set; } = default!;

    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
}
