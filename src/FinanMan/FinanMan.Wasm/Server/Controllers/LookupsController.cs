using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanMan.Server.Controllers;

[ApiController]
public abstract class LookupsControllerBase<TLookupItemViewModel> : ControllerBase
    where TLookupItemViewModel : class, ILookupItemViewModel
{
    protected abstract ILookupListService ListService { get; }

    [HttpGet]
    public async Task<IActionResult> GetLookupItems(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default) =>
        Ok(await ListService.GetLookupItemsAsync<TLookupItemViewModel>(startRecord, pageSize, asOfDate, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLookupItem(int id, CancellationToken ct = default) =>
        Ok(await ListService.GetLookupItem<TLookupItemViewModel>(id, ct));

    [HttpPost]
    public async Task<IActionResult> AddLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default) =>
        Ok(await ListService.AddLookupItem<TLookupItemViewModel>(dataEntryViewModel, ct));

    [HttpPut]
    public async Task<IActionResult> UpdateLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default) =>
        Ok(await ListService.UpdateLookupItem<TLookupItemViewModel>(dataEntryViewModel, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLookupItem(int id, CancellationToken ct = default) =>
        Ok(await ListService.DeleteLookupItem<TLookupItemViewModel>(id, ct));
}

[Route("api/Lookups/Accounts")]
[ApiController]
public class AccountsLookupController : LookupsControllerBase<AccountViewModel>
{
    protected override ILookupListService ListService { get; }

    public AccountsLookupController(ILookupListService listService)
    {
        ListService = listService;
    }
}

[Route("api/Lookups/AccountTypes")]
[ApiController]
public class AccountTypesController : LookupsControllerBase<LookupItemViewModel<LuAccountType>>
{
    protected override ILookupListService ListService { get; }

    public AccountTypesController(ILookupListService listService)
    {
        ListService = listService;
    }
}
