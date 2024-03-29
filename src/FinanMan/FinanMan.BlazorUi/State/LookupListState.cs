﻿using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using System.Text.Json;

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
        if (_initialized || _initializing)
        {
            while (!_initialized && _initializing)
            {
                await Task.Delay(200);
            }
            return;
        }
        _initializing = true;

        var accountsRespTask = _lookupService.GetLookupItemsAsync<AccountLookupViewModel>();
        var accountTypesRespTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuAccountType>>();
        var categoriesRespTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuCategory>>();
        var depositReasonsRespTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuDepositReason>>();
        var lineItemTypesRespTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<LuLineItemType>>();
        var payeesRespTask = _lookupService.GetLookupItemsAsync<PayeeLookupViewModel>();
        var recurrenceTypesRespTask = _lookupService.GetLookupItemsAsync<LookupItemViewModel<RecurrenceType, LuRecurrenceType>>();

        await Task.WhenAll(
            accountsRespTask,
            accountTypesRespTask,
            categoriesRespTask,
            depositReasonsRespTask,
            lineItemTypesRespTask,
            payeesRespTask,
            recurrenceTypesRespTask);

        var accountsResp = accountsRespTask.Result;
        var accountTypesResp = accountTypesRespTask.Result;
        var categoriesResp = categoriesRespTask.Result;
        var depositReasonsResp = depositReasonsRespTask.Result;
        var lineItemTypesResp = lineItemTypesRespTask.Result;
        var payeesResp = payeesRespTask.Result;
        var recurrenceTypesResp = recurrenceTypesRespTask.Result;

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

    public async Task<ResponseModel<ILookupItemViewModel>> CreateLookupItemAsync<TLookupItem>(TLookupItem lookupItem)
         where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModel<ILookupItemViewModel>()
        {
            Data = lookupItem
        };

        var resp = await _lookupService.CreateLookupItemAsync(lookupItem);
        if (resp.WasError)
        {
            retResp.AddErrors(resp);
            return retResp;
        }
        lookupItem.ValueText = resp.RecordId.ToString();

        var foundItem = GetLookupItems<TLookupItem>().FirstOrDefault(x => x.ValueText == lookupItem.ValueText);
        if (foundItem is not null)
        {
            foundItem.Deleted = false;
        }
        else
        {
            LookupItemCache.Add(lookupItem);
        }
        RaisePropertyChanged(nameof(LookupItemCache));

        return retResp;
    }

    public async Task RefreshListAsync<TLookupItem>()
         where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var mostRecentUpdated = LookupItemCache.OfType<TLookupItem>().Max(x => x.LastUpdated);
        var resp = await _lookupService.GetLookupItemsAsync<AccountLookupViewModel>(asOfDate: mostRecentUpdated);
        if (!(resp.Data?.Any() ?? false) || resp.WasError)
        {
            return;
        }

        // Find all returned items that already exist in the cache and remove them from the cache
        var existingItems = resp.Data.Where(x => LookupItemCache.Any(y => y.ValueText == x.ValueText)).ToList();
        if (existingItems.Any())
        {
            LookupItemCache.RemoveAll(x => existingItems.Any(y => y.ValueText == x.ValueText));
        }

        // Add all returned items to the cache
        LookupItemCache.AddRange(resp.Data);
        RaisePropertyChanged(nameof(LookupItemCache));
    }

    public async Task<ResponseModel<ILookupItemViewModel>> UpdateLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel lookupItem, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var resp = await _lookupService.UpdateLookupItemAsync(lookupItem, ct);
        if (resp.WasError) { return resp; }
        var existLookupItem = GetLookupItems<TLookupItemViewModel>().FirstOrDefault(x => x.ListItemId == lookupItem.ListItemId);
        if (existLookupItem is null)
        {
            var getResp = await _lookupService.GetLookupItemAsync<TLookupItemViewModel>(int.Parse(lookupItem.ValueText), ct);

            if (getResp.WasError || getResp.Data is null)
            {
                resp.AddError("The update was successful, however, the lookup item could not be retrieved nor updated in the local cache.  Please refresh the application to get the updated list of lookup items.");
                resp.AddErrors(getResp);
                return resp;
            }

            LookupItemCache.Add(getResp.Data);
        }
        else
        {
            existLookupItem.DisplayText = lookupItem.DisplayText;
        }

        RaisePropertyChanged(nameof(LookupItemCache));
        return resp;
    }

    public async Task<ResponseModelBase<int>> DeleteLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel lookupItem, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var resp = await _lookupService.DeleteLookupItemAsync<TLookupItemViewModel>(int.Parse(lookupItem.ValueText), ct);
        if (resp.WasError) { return resp; }
        var existLookupItem = GetLookupItems<TLookupItemViewModel>().FirstOrDefault(x => x.ListItemId == lookupItem.ListItemId);
        if (existLookupItem is null)
        {
            var getResp = await GetLookupItem(lookupItem, ct);

            if (getResp.WasError || getResp.Data is null)
            {
                resp.AddError("The delete was successful, however, the lookup item could not be retrieved nor updated in the local cache.  Please refresh the application to get the updated list of lookup items.");
                resp.AddErrors(getResp);
                return resp;
            }

            LookupItemCache.Add(getResp.Data);
        }
        else
        {
            existLookupItem.Deleted = true;
        }

        RaisePropertyChanged(nameof(LookupItemCache));

        return resp;
    }

    private async Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(TLookupItemViewModel lookupItem, CancellationToken ct = default) where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
        => await _lookupService.GetLookupItemAsync<TLookupItemViewModel>(int.Parse(lookupItem.ValueText), ct);
}
