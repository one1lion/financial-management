using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class Transfer
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int TargetAccountId { get; set; }

    public virtual Transaction Transaction { get; set; } = default!;
    public virtual Account TargetAccount { get; set; } = default!;
}
