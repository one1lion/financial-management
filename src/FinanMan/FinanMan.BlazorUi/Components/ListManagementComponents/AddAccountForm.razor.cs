using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.ListManagementComponents;
public partial class AddAccountForm
{
    [Inject, AllowNull] private IAccountService AccountService { get; set; }
    [Inject, AllowNull] private ILookupListState LookupListState { get; set; }
    [Inject, AllowNull] private ITransactionEntryService<DepositEntryViewModel> TransactionService { get; set; }
    [Parameter] public EventCallback<ResponseModel<AccountEntryViewModel>> OnAccountCreated { get; set; }
    [Parameter] public EventCallback<ResponseModel<AccountEntryViewModel>> OnPartialSuccess { get; set; }
    [Parameter] public EventCallback<ResponseModel<AccountEntryViewModel>> OnSuccess { get; set; }
    [Parameter] public EventCallback<ResponseModel<AccountEntryViewModel>> OnFailure { get; set; }

    private readonly AccountEntryViewModel _account = new();
    private List<LookupItemViewModel<LuAccountType>>? _accountTypes;
    [AllowNull]
    private List<LookupItemViewModel<LuDepositReason>> _depositReasons;

    private EditForm? _form;
    private readonly ResponseModel<AccountEntryViewModel> _accountEntryResponse = new();

    protected override async Task OnInitializedAsync()
    {
        _accountEntryResponse.Data = _account;
        await LookupListState.InitializeAsync();
        _accountTypes = LookupListState.GetLookupItems<LookupItemViewModel<LuAccountType>>().ToList();
        _depositReasons = LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().ToList();
    }

    public async Task<bool> SubmitFormAsync()
    {
        _accountEntryResponse.ClearErrors();
        _accountEntryResponse.Data ??= _account;

        if (_form?.EditContext is null)
        {
            _accountEntryResponse.AddError("The Add Account form was not initialized properly.");
            if (OnFailure.HasDelegate)
            {
                await OnFailure.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        if (!_form.EditContext.Validate())
        {
            // TODO: Update to use FluentValidation
            _accountEntryResponse.AddErrors(_form.EditContext.GetValidationMessages().ToList());
            if (OnFailure.HasDelegate)
            {
                await OnFailure.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        var model = _account.ToLookupViewModel();
        // Send the request to create the Account item
        // Return type should probably be the AccountLookupViewModel instead
        var retResp = await AccountService.CreateAccountAsync(model);

        if(retResp is null)
        {
            _accountEntryResponse.AddError("Something about the response was null and we expected something...I think");
            if(OnFailure.HasDelegate)
            {
                await OnFailure.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        if (retResp.WasError)
        {
            _accountEntryResponse.AddErrors(retResp);
            if (OnFailure.HasDelegate)
            {
                await OnFailure.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        _account.Id = retResp.RecordId;
        if (OnAccountCreated.HasDelegate)
        {
            await OnAccountCreated.InvokeAsync(_accountEntryResponse);
        }

        // If the account was created successfully, add the starting balance to mark the starting balance
        if (!_depositReasons.Any(x => x.DisplayText == "Starting Balance"))
        {
            _accountEntryResponse.AddError("The account was created successfully; however, the \"Starting Balance\" deposit reason was not found in the lookup list, so the starting balance could not be set. If the starting balance is not 0, please manually create a transaction using the Open Date and Starting Balance to set the starting balance.");
            if(OnPartialSuccess.HasDelegate)
            {
                await OnPartialSuccess.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        // Use the TransactionService to create the starting balance
        var transResp = await TransactionService.AddTransactionAsync(new DepositEntryViewModel
        {
            AccountId = _account.Id,
            Amount = _account.StartingBalance,
            DepositReasonValueText = _depositReasons.First(x => x.DisplayText == "Starting Balance").ValueText,
            Memo = "Starting Balance",
            TransactionDate = _account.DateOpened,
            PostedDate = _account.DateOpened
        });

        if(transResp is null)
        {
            _accountEntryResponse.AddError("The account was created; however, the starting balance could not be set. If the starting balance is not 0, please manually create a transaction using the Open Date and Starting Balance to set the starting balance.");
            if(OnPartialSuccess.HasDelegate)
            {
                await OnPartialSuccess.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        if(transResp.WasError)
        {
            _accountEntryResponse.AddErrors(transResp);
            if(OnPartialSuccess.HasDelegate)
            {
                await OnPartialSuccess.InvokeAsync(_accountEntryResponse);
            }
            return false;
        }

        if(OnSuccess.HasDelegate)
        {
            await OnSuccess.InvokeAsync(_accountEntryResponse);
        }
        return true;
    }
}
