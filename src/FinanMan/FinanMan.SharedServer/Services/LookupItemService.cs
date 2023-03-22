using FinanMan.Database;
using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

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

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        var queryable = GetQueryableLookupList<TLookupItemViewModel>(context);

        if (queryable is null)
        {
            retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
            return retResp;
        }

        // TODO: Sort by SortOrder
        // Apply Page if any
        retResp.Data = queryable.ToList();
        retResp.RecordCount = retResp.Data.Count;
        return retResp;
    }

    public async Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModel<TLookupItemViewModel>();

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        var queryable = GetQueryableLookupList<TLookupItemViewModel>(context);

        if (queryable is null)
        {
            retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
            return retResp;
        }

        retResp.Data = await queryable.FirstOrDefaultAsync(x => x.ValueText == id.ToString(), cancellationToken: ct);
        retResp.RecordCount = retResp.Data is null ? 0 : 1;
        return retResp;
    }

    public async Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);
        var lookupList = GetQueryableLookupList<TLookupItemViewModel>(context);

        if (lookupList is null)
        {
            retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
            return retResp;
        }

        var foundRec = await lookupList
            .FirstOrDefaultAsync(x => x.DisplayText == dataEntryViewModel.DisplayText, cancellationToken: ct);

        if (foundRec is not null)
        {
            retResp.AddError($"A record with the display text '{dataEntryViewModel.DisplayText}' already exists.");
            return retResp;
        }

        using var trans = await context.Database.BeginTransactionAsync(ct);

        try
        {
            var newRec = dataEntryViewModel.ToEntityModel();

            if(newRec is Account ac)
            {
                ac.AccountType = default!;
            }

            await context.AddAsync(newRec, ct);

            retResp.RecordCount = await context.SaveChangesAsync(ct);

            await trans.CommitAsync(ct);

            retResp.RecordId = newRec.GetId();
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
            retResp.AddError($"The request to add the new {typeof(TLookupItemViewModel)} failed. {ex.Message}");
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);
        var lookupList = GetQueryableLookupList<TLookupItemViewModel>(context);

        if (lookupList is null)
        {
            retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
            return retResp;
        }

        var foundRec = await lookupList
            .FirstOrDefaultAsync(x => x.ValueText == dataEntryViewModel.ValueText, cancellationToken: ct);

        if (foundRec is null)
        {
            retResp.AddError($"A record with the Id '{dataEntryViewModel.ValueText}' could not be found exists.");
            return retResp;
        }

        using var trans = await context.Database.BeginTransactionAsync(ct);

        try
        {
            var forUpdate = foundRec.ToEntityModel();

            // Attach and start tracking changes
            context.Attach(forUpdate);

            // Update the properties
            forUpdate.Patch(dataEntryViewModel.ToEntityModel());

            retResp.RecordCount = await context.SaveChangesAsync(ct);

            await trans.CommitAsync(ct);

            retResp.RecordId = forUpdate.GetId();
        }
        catch (Exception ex)
        {
            await trans.RollbackAsync();
            retResp.AddError($"The request to add the new {typeof(TLookupItemViewModel)} failed. {ex.Message}");
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();

        using var context = await _dbContextFactory.CreateDbContextAsync(ct);
        var lookupList = GetQueryableLookupList<TLookupItemViewModel>(context);

        if (lookupList is null)
        {
            retResp.AddError($"Invalid lookup list type: {typeof(TLookupItemViewModel)}");
            return retResp;
        }

        var viewModel = await lookupList.FirstOrDefaultAsync(x => x.ValueText == id.ToString());
        if (viewModel is null)
        {
            retResp.AddError($"The {typeof(TLookupItemViewModel)} with id {id} was not found.");
            return retResp;
        }

        var recordToDelete = viewModel.ToEntityModel();

        context.Attach(recordToDelete);
        recordToDelete.Deleted = true;
        recordToDelete.LastUpdated = DateTime.Now;

        retResp.RecordCount = await context.SaveChangesAsync(ct);
        retResp.RecordId = id;
        
        return retResp;
    }

    #region Helpers
    private static IQueryable<TLookupItemViewModel>? GetQueryableLookupList<TLookupItemViewModel>(FinanManContext context)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var typeInst = new TLookupItemViewModel();
        IQueryable<TLookupItemViewModel>? queryable = null;
        switch (typeInst.ListType)
        {
            case LookupListType.Accounts:
                queryable = context.Accounts
                    .Include(x => x.AccountType)
                    .Select(x =>  new AccountLookupViewModel()
                    {
                        Id = x.Id,
                        DisplayText = x.Name,
                        AccountType = x.AccountType.Name,
                        AccountTypeId = x.AccountTypeId,
                        SortOrder = x.SortOrder,
                        ValueText = x.Id.ToString(),
                        LastUpdated = x.LastUpdated,
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
                queryable = context.Payees.Select(x => new PayeeLookupViewModel()
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
        }

        return queryable;
    }
    #endregion Helpers
}


public static class ILookupItemExtensions
{
    // TODO: Finish writing the ToEntity extensions
    public static ILookupItem ToEntityModel(this ILookupItemViewModel viewModel) => viewModel switch
    {
        AccountLookupViewModel accountViewModel => accountViewModel.Item,
        LookupItemViewModel<LuAccountType> luAccountTypeViewModel => luAccountTypeViewModel.Item,
        LookupItemViewModel<LuCategory> luCategoryViewModel => luCategoryViewModel.Item,
        LookupItemViewModel<LuDepositReason> luDepositReasonViewModel => luDepositReasonViewModel.Item,
        LookupItemViewModel<LuLineItemType> luLineItemTypeViewModel => luLineItemTypeViewModel.Item,
        PayeeLookupViewModel payeeViewModel => payeeViewModel.Item,
        _ => throw new NotImplementedException(),
        // NOTE: Recurrence types are not maintainable from the application
    };

    public static void Patch(this ILookupItem model, ILookupItem updatedModel)
    {
        model.Name = updatedModel.Name;
        model.SortOrder = updatedModel.SortOrder;
        model.LastUpdated = DateTime.Now;
        model.Deleted = updatedModel.Deleted;

        switch(model)
        {
            case Account account:
                var updatedAccount = (Account)updatedModel;
                account.AccountTypeId = updatedAccount.AccountTypeId;
                account.AccountType = default!;
                break;
            case Payee payee:
                // TODO: Update the categories list associated with the Payee
                break;
        }
    }

    public static int GetId(this ILookupItem model)
    {
        var type = model.GetType();
        var idProp = type.GetProperty(nameof(ILookupItem<int>.Id));

        var idVal = idProp?.GetValue(model)?.ToString();
        var id = 0;
        if (idProp is null || (idProp.PropertyType != typeof(Enum) && !int.TryParse(idVal, out id)))
        {
            return 0;
        }

        if (idProp.PropertyType == typeof(Enum)) { return (int?)idProp.GetValue(model) ?? 0; }

        return id;
    }
}
