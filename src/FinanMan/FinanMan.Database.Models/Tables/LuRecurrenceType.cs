using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class LuRecurrenceType : LookupItemBase<RecurrenceType>
{
    public LuRecurrenceType() : base(LookupListType.RecurrenceTypes) { }

    public string DisplayText { get; set; } = default!;

    public virtual ICollection<ScheduledTransaction> ScheduledTransactions { get; set; } = new HashSet<ScheduledTransaction>();
}
