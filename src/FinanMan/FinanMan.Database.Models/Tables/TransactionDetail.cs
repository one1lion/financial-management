namespace FinanMan.Database.Models.Tables;

public partial class TransactionDetail
{
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int LineItemTypeId { get; set; }
    public string? Description { get; set; }
    public double Amount { get; set; }
    
    public virtual Transaction Transaction { get; set; } = default!;
    public virtual LuLineItemType LineItemType { get; set; } = default!;
}
