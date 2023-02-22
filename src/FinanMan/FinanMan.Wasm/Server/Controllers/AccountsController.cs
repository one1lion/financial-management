using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanMan.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccounts()
        => Ok(await _accountService.GetAccountsAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAccount([FromRoute] int id)
        => Ok(await _accountService.GetAccountAsync(id));

    [HttpGet("summaries")]
    public async Task<IActionResult> GetAccountSummaries(CancellationToken ct = default)
        => Ok(await _accountService.GetAccountSummariesAsync(ct));

    [HttpGet("summaries/{id:int}")]
    public async Task<IActionResult> GetAccountSummary([FromRoute] int id)
        => Ok(await _accountService.GetAccountSummaryAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] AccountLookupViewModel accountModel)
        => Ok(await _accountService.CreateAccountAsync(accountModel));

    [HttpPut]
    public async Task<IActionResult> UpdateAccount([FromBody] AccountLookupViewModel accountModel)
        => Ok(await _accountService.UpdateAccountAsync(accountModel));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] int id)
        => Ok(await _accountService.DeleteAccountAsync(id));
}
