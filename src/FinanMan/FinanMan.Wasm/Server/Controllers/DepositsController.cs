using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanMan.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepositsController : ControllerBase
{
    private readonly ITransactionEntryService<DepositEntryViewModel> _transactionEntryService;

    public DepositsController(ITransactionEntryService<DepositEntryViewModel> transactionEntryService)
    {
        _transactionEntryService = transactionEntryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactionsAsync(ushort sr = 0, ushort ps = 100, DateTime? aod = null, CancellationToken ct = default)
    {
        return Ok(await _transactionEntryService.GetTransactionsAsync(sr, ps, aod, ct));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransactionsAsync([FromRoute] int id, CancellationToken ct = default)
    {
        return Ok(await _transactionEntryService.GetTransactionAsync(id, ct));
    }

    [HttpPost]
    public async Task<IActionResult> AddTransactionData(DepositEntryViewModel deposit) =>
        Ok(await _transactionEntryService.AddTransactionAsync(deposit));

    [HttpPut]
    public async Task<IActionResult> UpdateTransactionData(DepositEntryViewModel deposit) =>
        Ok(await _transactionEntryService.UpdateTransactionAsync(deposit));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransactionData([FromRoute] int id) =>
        Ok(await _transactionEntryService.DeleteTransactionAsync(id));
}
