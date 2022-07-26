﻿namespace FinanMan.Database.Models.Tables;

public partial class Payment
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int PayeeId { get; set; }

    public virtual Payee Payee { get; set; } = default!;
    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new HashSet<PaymentDetail>();
    public virtual Transaction Transaction { get; set; } = default!;
}
