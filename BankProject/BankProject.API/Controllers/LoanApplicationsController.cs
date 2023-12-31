using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/loanapplications")]
public class LoanApplicationsController : ControllerBase
{
    private readonly ILoanApplicationService _applicationService;

    public LoanApplicationsController(ILoanApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateLoanApplicationAsync(CreateLoanApplicationRequestDto requestDto)
    {
        await _applicationService.CreateLoanApplicationAsync(requestDto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    [Route("{id:guid}/status")]
    public async Task<IActionResult> GetLoanApplicationStatusByIdAsync(Guid id)
    {
        var loanApplication = await _applicationService.GetLoanApplicationByIdAsync(id);

        return Ok(loanApplication);
    }

    [HttpPut]
    [Route("{id:guid}/process")]
    public async Task<IActionResult> ProcessApplicationAsync(Guid id)
    {
        await _applicationService.ProcessLoanApplicationStatusAsync(id);

        return Ok();
    }
}