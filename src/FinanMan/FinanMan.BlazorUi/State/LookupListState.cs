using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.Shared.StateInterfaces;

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

    public async Task Initialize()
    {
        if (_initialized || _initializing) { return; }
        _initializing = true;

        //var accountsTask = _lookupService.GetLookupItemsAsync<AccountViewModel>();
        //var accountTypesTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuAccountType>>();
        //var categoriesTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuCategory>>();
        //var depositReasonsTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuDepositReason>>();
        //var lineItemTypesTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuLineItemType>>();
        //var payeesTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<Payee>>();
        //var recurrenceTypesTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuRecurrenceType>>();

        //var lookupTasks = new Task[]
        //{
        //    accountsTask,
        //    accountTypesTask,
        //    categoriesTask,
        //    depositReasonsTask,
        //    lineItemTypesTask,
        //    payeesTask,
        //    recurrenceTypesTask
        //};

        //await Task.WhenAll(lookupTasks);

        //var accountsResp = accountsTask.Result;
        //var accountTypesResp = accountTypesTask.Result;
        //var categoriesResp = categoriesTask.Result;
        //var depositReasonsResp = depositReasonsTask.Result;
        //var lineItemTypesResp = lineItemTypesTask.Result;
        //var payeesResp = payeesTask.Result;
        //var recurrenceTypesResp = recurrenceTypesTask.Result;

        var accountsResp = await _lookupService.GetLookupItemsAsync<AccountViewModel>();
        var accountTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuAccountType>>();
        var categoriesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuCategory>>();
        var depositReasonsResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuDepositReason>>();
        var lineItemTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuLineItemType>>();
        var payeesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<Payee>>();
        var recurrenceTypesResp = await _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuRecurrenceType>>();

        // API request to get list items
        if (!accountsResp.WasError && (accountsResp.Data?.Any() ?? false))
        {
            LookupItemCache.AddRange(accountsResp.Data.Select(x => new LookupItemViewModel<AccountViewModel>()
            {
                Id = x.Id,
                DisplayText = x.AccountName,
                ValueText = x.Id.ToString(),
                LastUpdated = DateTime.UtcNow,
                Item = x
            }));
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

    public IEnumerable<ILookupItemViewModel<TKey, TLookupItem>> GetLookupItems<TKey, TLookupItem>()
         where TLookupItem : class, IHasLookupListType
    {
        return LookupItemCache.OfType<LookupItemViewModel<TKey, TLookupItem>>();
    }
}
