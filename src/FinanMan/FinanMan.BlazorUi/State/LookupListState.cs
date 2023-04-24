using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.State;

public class LookupListState : BaseNotifyPropertyChanges, ILookupListState
{
    private readonly ILookupListService _lookupService;

    public LookupListState(ILookupListService lookupService)
    {
        _lookupService = lookupService;
    }

    public List<ILookupItemViewModel> LookupItemCache { get; } = new();

    private bool _initialized;
    private bool _initializing;

    public bool Initialized { get => _initialized; set => SetField(ref _initialized, value); }
    public bool Initializing { get => _initializing; set => SetField(ref _initializing, value); }

    public async Task InitializeAsync()
    {
        if (_initialized || _initializing) {
            while(!_initialized && _initializing)
            {
                await Task.Delay(200);
            }
            return; 
        }
        _initializing = true;
        
        var accountsResp = await _lookupService.GetLookupItemsAsync<AccountLookupViewModel>();
        var accountTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuAccountType>>();
        var categoriesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuCategory>>();
        var depositReasonsResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuDepositReason>>();
        var lineItemTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuLineItemType>>();
        var payeesResp = await _lookupService.GetLookupItemsAsync<PayeeLookupViewModel>();
        var recurrenceTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<RecurrenceType, LuRecurrenceType>>();

        // API request to get list items
        if (!accountsResp.WasError && (accountsResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(accountsResp.Data);
        }
        if (!accountTypesResp.WasError && (accountTypesResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(accountTypesResp.Data);
        }
        if (!categoriesResp.WasError && (categoriesResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(categoriesResp.Data);
        }
        if (!depositReasonsResp.WasError && (depositReasonsResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(depositReasonsResp.Data);
        }
        if (!lineItemTypesResp.WasError && (lineItemTypesResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(lineItemTypesResp.Data);
        }
        if (!payeesResp.WasError && (payeesResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(payeesResp.Data);
        }
        if (!recurrenceTypesResp.WasError && (recurrenceTypesResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(recurrenceTypesResp.Data);
        }

        _initialized = true;
        _initializing = false;
    }

    public IEnumerable<TLookupItem> GetLookupItems<TLookupItem>()
         where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        return LookupItemCache.OfType<TLookupItem>();
    }
}
