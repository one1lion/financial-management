using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class LuAccountType : LookupItemBase
{
    public LuAccountType() : base(LookupListType.AccountTypes) { }
    public bool IncreaseOnPayment { get; set; }
}
