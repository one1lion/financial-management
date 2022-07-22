using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class ScheduledTransaction
{
    public int Id { get; set; }
    public RecurrenceType RecurrenceTypeId { get; set; }
    public int DayNum { get; set; }
    public int AccountId { get; set; }
    public int PayeeId { get; set; }
    public string Memo { get; set; } = default!;

    public virtual Account Account { get; set; } = default!;
    public virtual Payee Payee { get; set; } = default!;
    public virtual LuRecurrenceType RecurrenceType { get; set; } = default!;
}