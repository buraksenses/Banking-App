﻿using BankProject.Business.DTOs.Account;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

public class AccountsController : CustomControllerBase
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAccountAsync(CreateAccountRequestDto requestDto)
    {
        await _service.CreateAccountAsync(requestDto);
    
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut]
    [Route("{id:guid}/balance")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBalanceByAccountIdAsync(Guid id, decimal balance)
    {
        await _service.UpdateBalanceByAccountIdAsync(id, balance);

        return Ok(balance);
    }

    [HttpPut]
    [Route("{accountId:guid}/loan-payment")]
    public async Task<IActionResult> MakeLoanPaymentAsync(Guid accountId, Guid loanId, decimal paymentAmount)
    {
        await _service.MakeLoanPaymentAsync(accountId, loanId, paymentAmount);

        return Ok();
    }
}