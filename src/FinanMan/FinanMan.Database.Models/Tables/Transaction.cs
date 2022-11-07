using System.ComponentModel.DataAnnotations.Schema;
using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class Transaction
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime DateEntered { get; set; } = DateTime.UtcNow;
    public DateTime TransactionDate { get; set; }
    public DateTime? PostingDate { get; set; }
    public string? Memo { get; set; }
    public DateTime? PurgeDate { get; set; }

    [NotMapped]
    public TransactionType TransactionType => Deposit is not null ? TransactionType.Deposit
        : Payment is not null ? TransactionType.Payment
        : TransactionType.Transfer;

    public virtual Account Account { get; set; } = default!;
    public virtual Deposit? Deposit { get; set; }
    public virtual Payment? Payment { get; set; }
    public virtual Transfer? Transfer { get; set; }
}
