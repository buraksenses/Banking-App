using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/loans")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _service;

    public LoansController(ILoanService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("apply")]
    public async Task<IActionResult> CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto)
    {
        await _service.CreateLoanApplicationAsync(requestDto);

        return StatusCode(StatusCodes.Status201Created);
    }
}