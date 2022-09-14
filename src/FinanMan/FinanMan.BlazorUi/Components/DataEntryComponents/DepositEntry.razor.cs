using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class DepositEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;

    private DepositEntryViewModel _newDeposit = new();
    private List<ILookupItemViewModel<int, AccountViewModel>>? _accounts;
    private List<ILookupItemViewModel<int, LuDepositReason>>? _depositReasons;
    protected override async Task OnInitializedAsync()
    {
        await LookupListState.Initialize();
        await Task.Delay(2000);
        _accounts = LookupListState.GetLookupItems<int, AccountViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<int, LuDepositReason>().ToList();
    }
}
