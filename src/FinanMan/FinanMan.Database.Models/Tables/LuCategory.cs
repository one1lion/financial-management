using FinanMan.Database.Models.Shared;

namespace FinanMan.Database.Models.Tables;

public partial class LuCategory : LookupItemBase
{
    public LuCategory() : base(LookupListType.Categories) { }

    public virtual ICollection<Payee> Payees { get; set; } = new HashSet<Payee>();
}
