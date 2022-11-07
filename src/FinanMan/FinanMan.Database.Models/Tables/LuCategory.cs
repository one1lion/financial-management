using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class LuCategory : LookupItemBase
{
    [NotMapped]
    public override LookupListType ListType => LookupListType.Category;
    public virtual ICollection<Payee> Payees { get; set; } = new HashSet<Payee>();
}
