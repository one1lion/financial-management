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
        : Transfer is not null ? TransactionType.Transfer
        : TransactionType.Unknown;

    [NotMapped]
    public decimal Total => TransactionType switch
    {
        TransactionType.Deposit => Deposit?.Amount ?? 0m,
        TransactionType.Payment => Payment?.Total ?? 0m,
        TransactionType.Transfer => Transfer?.Amount ?? 0m,
        _ => 0m
    };

    public virtual Account Account { get; set; } = default!;
    public virtual Deposit? Deposit { get; set; }
    public virtual Payment? Payment { get; set; }
    public virtual Transfer? Transfer { get; set; }
}
