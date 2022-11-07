using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanMan.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransfersController : ControllerBase
{
    private readonly ITransactionEntryService<TransferEntryViewModel> _transactionEntryService;

    public TransfersController(ITransactionEntryService<TransferEntryViewModel> transactionEntryService)
    {
        _transactionEntryService = transactionEntryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactionsAsync(ushort sr = 0, ushort ps = 100, DateTime? aod = null, CancellationToken ct = default)
    {
        return Ok(await _transactionEntryService.GetTransactionsAsync(sr, ps, aod, ct));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTransactionsAsync([FromRoute] int id, DateTime? aod = null, CancellationToken ct = default)
    {
        return Ok(await _transactionEntryService.GetTransactionAsync(id, ct));
    }

    [HttpPost]
    public async Task<IActionResult> AddTransactionData(TransferEntryViewModel deposit) =>
        Ok(await _transactionEntryService.AddTransactionAsync(deposit));

    [HttpPut]
    public async Task<IActionResult> UpdateTransactionData(TransferEntryViewModel deposit) =>
        Ok(await _transactionEntryService.UpdateTransactionAsync(deposit));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTransactionData([FromRoute] int id) =>
        Ok(await _transactionEntryService.DeleteTransactionAsync(id));
}
