using FinanMan.BlazorUi.State;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;
public partial class PaymentDetailsTable
{
    [Parameter] public required PaymentEntryViewModel Payment { get; set; }
}
