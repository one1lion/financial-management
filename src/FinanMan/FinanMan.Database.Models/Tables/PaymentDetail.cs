namespace FinanMan.Database.Models.Tables;

public partial class PaymentDetail
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public int LineItemTypeId { get; set; }
    public string? Detail { get; set; }
    public double Amount { get; set; }

    public virtual Payment Payment { get; set; } = default!;
    public virtual LuLineItemType LineItemType { get; set; } = default!;
}
