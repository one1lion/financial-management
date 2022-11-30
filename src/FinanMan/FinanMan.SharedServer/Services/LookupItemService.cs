using FinanMan.Database;
using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FinanMan.SharedServer.Services;

public class LookupItemService : ILookupListService
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public LookupItemService(IDbContextFactory<FinanManContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItemsAsync<TLookupItemViewModel>(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModel<List<TLookupItemViewModel>>();
        var typeInst = new TLookupItemViewModel();

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        IQueryable<TLookupItemViewModel> queryable = default!;
        
        switch (typeInst.ListType)
        {
            case LookupListType.Accounts:
                queryable = context.Accounts.Select(x => new AccountViewModel()
                {
                    Id = x.Id,
                    AccountName = x.Name,
                    SortOrder = x.Id,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.AccountTypes:
                queryable = context.AccountTypes.Select(x => new LookupItemViewModel<LuAccountType>()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.SortOrder,
                    LastUpdated = x.LastUpdated,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.Categories:
                queryable = context.Categories.Select(x => new LookupItemViewModel<LuCategory>()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.SortOrder,
                    LastUpdated = x.LastUpdated,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.DepositReasons:
                queryable = context.DepositReasons.Select(x => new LookupItemViewModel<LuDepositReason>()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.SortOrder,
                    LastUpdated = x.LastUpdated,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.LineItemTypes:
                queryable = context.LineItemTypes.Select(x => new LookupItemViewModel<LuLineItemType>()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.SortOrder,
                    LastUpdated = x.LastUpdated,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.Payees:
                queryable = context.Payees.Select(x => new PayeeViewModel()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.Id,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            case LookupListType.RecurrenceTypes:
                queryable = context.RecurrenceTypes.Select(x => new LookupItemViewModel<RecurrenceType, LuRecurrenceType>()
                {
                    Id = x.Id,
                    DisplayText = x.Name,
                    ValueText = x.Id.ToString(),
                    SortOrder = x.SortOrder,
                    LastUpdated = x.LastUpdated,
                    Item = x
                })
                .OfType<TLookupItemViewModel>();
                break;
            default:
                retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
                return retResp;
        }
        // Sort by SortOrder
        // Apply Page if any
        retResp.Data = queryable.ToList();
        return retResp;
    }

    public Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        throw new NotImplementedException();
    }
}
