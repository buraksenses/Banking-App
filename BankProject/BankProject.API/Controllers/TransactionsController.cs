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
    public async Task<IActionResult> DepositAsync(DepositAndWithdrawDto depositAndWithdrawDto)
    {
        await _accountService.DepositAsync(depositAndWithdrawDto.AccountId, depositAndWithdrawDto.Amount);
        return Ok();
    }

    [HttpPost]
    [Route("withdraw")]
    public async Task<IActionResult> WithdrawAsync(DepositAndWithdrawDto depositAndWithdrawDto)
    {
        await _accountService.WithdrawAsync(depositAndWithdrawDto.AccountId, depositAndWithdrawDto.Amount);
        return Ok();
    }
}