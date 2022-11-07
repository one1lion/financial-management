using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanMan.Database.Models.Tables;

public partial class LuAccountType : LookupItemBase
{
    [NotMapped]
    public override LookupListType ListType => LookupListType.AccountType;
}
