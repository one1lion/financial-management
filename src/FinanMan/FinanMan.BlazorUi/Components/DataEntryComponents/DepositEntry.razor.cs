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

    private InputDate<DateTime?>? _transDateInput;

    protected override async Task OnInitializedAsync()
    {
        await LookupListState.Initialize();
        _accounts = LookupListState.GetLookupItems<int, AccountViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<int, LuDepositReason>().ToList();
    }

    private async Task HandleDepositSubmitted(EditContext editContext)
    {
        _currentResponse = default;
        _submitting = true;
        _currentResponse = await DepositEntryService.AddTransactionAsync(_newDeposit);
        if (!_currentResponse.WasError)
        {
            _newDeposit = new();
            if (_transDateInput is not null && _transDateInput.Element.HasValue)
            {
                await _transDateInput.Element.Value.FocusAsync();
            }
        }
        _submitting = false;
    }
}
