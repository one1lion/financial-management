using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanMan.Server.Controllers;

[ApiController]
public abstract class LookupsControllerBase<TLookupItemViewModel> : ControllerBase
    where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
{
    private readonly ILookupListService _listService;

    public LookupsControllerBase(ILookupListService listService)
    {
        _listService = listService;
    }

    [HttpGet]
    public async Task<IActionResult> GetLookupItems(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default) =>
        Ok(await _listService.GetLookupItemsAsync<TLookupItemViewModel>(startRecord, pageSize, asOfDate, ct));

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLookupItem(int id, CancellationToken ct = default) =>
        Ok(await _listService.GetLookupItemAsync<TLookupItemViewModel>(id, ct));

    [HttpPost]
    public async Task<IActionResult> AddLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default) =>
        Ok(await _listService.CreateLookupItemAsync<TLookupItemViewModel>(dataEntryViewModel, ct));

    [HttpPut]
    public async Task<IActionResult> UpdateLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default) =>
        Ok(await _listService.UpdateLookupItemAsync<TLookupItemViewModel>(dataEntryViewModel, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLookupItem(int id, CancellationToken ct = default) =>
        Ok(await _listService.DeleteLookupItemAsync<TLookupItemViewModel>(id, ct));
}

[Route("api/Lookups/Accounts")]
[ApiController]
public class AccountLookupsController : LookupsControllerBase<AccountLookupViewModel>
{
    public AccountLookupsController(ILookupListService listService) : base(listService) { }
}

[Route("api/Lookups/AccountTypes")]
[ApiController]
public class AccountTypesController : LookupsControllerBase<LookupItemViewModel<LuAccountType>>
{
    public AccountTypesController(ILookupListService listService) : base(listService) {}
}

[Route("api/Lookups/Categories")]
[ApiController]
public class CategoriesController : LookupsControllerBase<LookupItemViewModel<LuAccountType>>
{
    public CategoriesController(ILookupListService listService) : base(listService) {}
}

[Route("api/Lookups/DepositReasons")]
[ApiController]
public class DepositReasonsController : LookupsControllerBase<LookupItemViewModel<LuDepositReason>>
{
    public DepositReasonsController(ILookupListService listService) : base(listService) {}
}

[Route("api/Lookups/LineItemTypes")]
[ApiController]
public class LineItemTypesController : LookupsControllerBase<LookupItemViewModel<LuLineItemType>>
{
    public LineItemTypesController(ILookupListService listService) : base(listService) {}
}

[Route("api/Lookups/Payees")]
[ApiController]
public class PayeeLookupsController : LookupsControllerBase<PayeeLookupViewModel>
{
    public PayeeLookupsController(ILookupListService listService) : base(listService) {}
}

[Route("api/Lookups/RecurrenceTypes")]
[ApiController]
public class RecurrenceTypesController : LookupsControllerBase<LookupItemViewModel<RecurrenceType, LuRecurrenceType>>
{
    public RecurrenceTypesController(ILookupListService listService) : base(listService) {}
}
