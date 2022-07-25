namespace FinanMan.Database.Models.Tables;

public partial class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public int PayeeId { get; set; }
    public DateTime DateEntered { get; set; } = DateTime.UtcNow;
    public DateTime TransactionDate { get; set; }
    public DateTime? PostingDate { get; set; }
    public string? Memo { get; set; }

    public virtual Account Account { get; set; } = default!;
    public virtual Payee Payee { get; set; } = default!;
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new HashSet<TransactionDetail>();
    
}
