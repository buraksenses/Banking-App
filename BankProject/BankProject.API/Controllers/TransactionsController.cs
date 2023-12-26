using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public TransactionsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Route("deposit")]
    public async Task<IActionResult> DepositAsync(DepositDto depositDto)
    {
        await _accountService.DepositAsync(depositDto.AccountId, depositDto.Amount);
        return Ok();
    }
}