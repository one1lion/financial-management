namespace FinanMan.Database.Models.Tables;

public partial class Deposit
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int DepositReasonId { get;  set; }
    public decimal Amount { get; set; }

    public virtual LuDepositReason DepositReason { get; set; } = default!;
    public virtual Transaction Transaction { get; set; } = default!;
}
