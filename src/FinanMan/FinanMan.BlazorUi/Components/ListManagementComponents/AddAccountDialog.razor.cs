using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.ListManagementComponents;
public partial class AddAccountDialog
{
    [Inject, AllowNull] private ILookupListState LookupListState { get; set; }

    [Parameter]
    public bool Show
    {
        get => _prevShow;
        set
        {
            if (value == _prevShow) { return; }

            _prevShow = value;
            if (ShowChanged.HasDelegate)
            {
                ShowChanged.InvokeAsync(value);
            }
        }
    }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    private bool _prevShow;
    private bool _accountCreated;
    private bool _submitting;
    private AddAccountForm? _accountFormRef;
    private ResponseModel<AccountEntryViewModel>? _responseModel;

    private async Task HandleConfirmClicked()
    {
        if(_accountFormRef is null || _accountCreated) { return; }

        _submitting = true;
        var success = await _accountFormRef.SubmitFormAsync();
        if (success)
        {
            Show = false;
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
