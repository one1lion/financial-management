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
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ITransactionEntryService<DepositEntryViewModel> DepositEntryService { get; set; } = default!;

    private DepositEntryViewModel _newDeposit = new();
    private List<AccountViewModel>? _accounts;
    private List<LookupItemViewModel<LuDepositReason>>? _depositReasons;
    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;

    private InputDate<DateTime?>? _transDateInput;

    protected override Task OnInitializedAsync()
    {
        LookupListState.PropertyChanged += HandleLookupListStateChanged;
        PopulateLookups();
        return LookupListState.Initialize();
    }

    private async void HandleLookupListStateChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ILookupListState.Initialized) && LookupListState.Initialized)
        {
            PopulateLookups();
        }
        await InvokeAsync(StateHasChanged);
    }

    private void PopulateLookups()
    {
        _accounts = LookupListState.GetLookupItems<AccountViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().ToList();
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
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;
    }
}
