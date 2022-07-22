namespace FinanMan.Database.Models.Tables;

public partial class Transfer
{
    public int Id { get; set; }
    public int FromTransactionId { get; set; }
    public int ToTransactionId { get; set; }

    public virtual Transaction FromTransaction { get; set; } = default!;
    public virtual Transaction ToTransaction { get; set; } = default!;
}
