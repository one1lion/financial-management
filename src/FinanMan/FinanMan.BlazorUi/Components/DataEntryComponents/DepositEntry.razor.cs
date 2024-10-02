using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class DepositEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ITransactionEntryService<DepositEntryViewModel> DepositEntryService { get; set; } = default!;

    [Parameter] public DepositEntryViewModel? Deposit { get; set; }

    private DepositEntryViewModel _editDeposit = new();
    private List<AccountLookupViewModel>? _accounts;
    private List<LookupItemViewModel<LuDepositReason>>? _depositReasons;
    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;

#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Field 'PaymentEntry._pmtDateInput' is never assigned to, and will always have its default value null
    private InputDate<DateTime?>? _depDateInputRef;
#pragma warning restore CS0649 // Field 'PaymentEntry._pmtDateInput' is never assigned to, and will always have its default value null
#pragma warning restore IDE0044 // Add readonly modifier

    protected override async Task OnInitializedAsync()
    {
        LookupListState.PropertyChanged += HandleLookupListStateChanged;
        await LookupListState.InitializeAsync();
        PopulateLookups();
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (parameters.TryGetValue<DepositEntryViewModel>(nameof(Deposit), out var deposit))
        {
            _editDeposit = deposit;
        }
        return base.SetParametersAsync(ParameterView.Empty);
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
        _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
        _depositReasons = LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().ToList();
    }

    private async Task HandleDepositSubmitted(EditContext editContext)
    {
        if (_submitting) { return; }

        _currentResponse = default;
        _submitting = true;
        _editDeposit.UpdateAccountName(_accounts ?? new());
        _currentResponse = await DepositEntryService.AddTransactionAsync(_editDeposit);
        if (!_currentResponse.WasError)
        {
            _editDeposit = new();
            if (_depDateInputRef is not null && _depDateInputRef.Element.HasValue)
            {
                await _depDateInputRef.Element.Value.FocusAsync();
            }
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;
    }
}
