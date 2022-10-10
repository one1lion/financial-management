using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.Shared.StateInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class DepositEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionEntryService<DepositEntryViewModel> DepositEntryService { get; set; } = default!;

    private DepositEntryViewModel _newDeposit = new();
    private List<ILookupItemViewModel<int, AccountViewModel>>? _accounts;
    private List<ILookupItemViewModel<int, LuDepositReason>>? _depositReasons;
    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.Initialize();
        await Task.Delay(2000);
        _accounts = LookupListState.GetLookupItems<int, AccountViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<int, LuDepositReason>().ToList();
    }

    private async Task HandleDepositSubmitted(EditContext editContext)
    {
        _currentResponse = default;
        _submitting = true;
        _currentResponse = await DepositEntryService.AddTransactionAsync(_newDeposit);
        _submitting = false;
    }
}
