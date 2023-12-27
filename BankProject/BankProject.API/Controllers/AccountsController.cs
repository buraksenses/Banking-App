using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountsController(IAccountService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id:guid}/balance")]
    public async Task<IActionResult> GetBalanceByAccountIdAsync(Guid id)
    {
        var balance = await _service.GetBalanceByAccountIdAsync(id);

        return Ok(balance);
    }

    // [HttpPost]
    // public async Task<IActionResult> CreateAccountAsync(CreateAccountRequestDto requestDto)
    // {
    //     await _service.CreateAccountAsync(requestDto);
    //
    //     return StatusCode(StatusCodes.Status201Created);
    // }

    [HttpPut]
    public async Task<IActionResult> UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        await _service.UpdateBalanceByAccountIdAsync(id, balance);

        return Ok(balance);
    }
}