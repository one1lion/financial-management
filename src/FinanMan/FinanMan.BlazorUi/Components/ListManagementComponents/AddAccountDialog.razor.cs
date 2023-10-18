using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.ListManagementComponents;
public partial class AddAccountDialog
{
    [Inject, AllowNull] private ILookupListState LookupListState { get; set; }
    [Inject, AllowNull] private ITransactionsState TransactionsState { get; set; }

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    private bool _prevShow;
    private bool _accountCreated;
    private bool _submitting;
    private AddAccountForm? _accountFormRef;
    private ResponseModel<AccountEntryViewModel>? _responseModel;

    protected override void OnInitialized()
    {
        _prevShow = Show;
        Console.WriteLine($"Prev Show set to: {_prevShow}");
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (Show != _prevShow)
        {
            await HandleShowChanged();
        }
    }

    private Task HandleShowChanged()
    {
        if (Show == _prevShow) { return Task.CompletedTask; }

        _prevShow = Show;
        if (ShowChanged.HasDelegate)
        {
            return ShowChanged.InvokeAsync(Show);
        }

        return Task.CompletedTask;
    }

    private async Task HandleConfirmClicked()
    {
        if (_accountFormRef is null || _accountCreated) { return; }

        _submitting = true;
        var success = await _accountFormRef.SubmitFormAsync();
        if (success)
        {
            Show = false;
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;
    }

    private async Task HandleAccountCreated(ResponseModel<AccountEntryViewModel> responseModel)
    {
        _accountCreated = true;
        _responseModel = responseModel;
        // Update the cached lookup lists
        await LookupListState.RefreshListAsync<AccountLookupViewModel>();
    }
}
