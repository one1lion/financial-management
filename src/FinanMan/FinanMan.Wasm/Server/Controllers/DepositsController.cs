using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedClient.Services;
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

    [HttpPost]
    public async Task<IActionResult> AddTransactionData(DepositEntryViewModel deposit) =>
        Ok(await _transactionEntryService.AddTransactionData(deposit));
}
