namespace FinanMan.Shared.DataEntryModels;

public class DepositViewModel
{
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public int? TargetAccountId { get; set; }
    public int? DepositReasonId { get; set; }
    public string? Memo { get; set; }
    public double? Amount { get; set; }

    public bool Get(DepositViewModel deposit)
    {
        // do some work that doesn't change the instanced member's properties
        //TransactionDate = DateTime.Now; // If this was a const function, this line should error
        //DoSomeOtherWork(); // If this was a const function, since DoSomeOtherWOrk modifies the value in member property, this line should error
        return true;
    }

    public void DoSomeOtherWork()
    {
        
    }
}
