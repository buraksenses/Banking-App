using BankProject.Business.DTOs.Loan;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    [Route("{id:guid}/recommendation")]
    public async Task<IActionResult> ProcessApplicationAsync(Guid id)
    { 
        var response = await _applicationService.GetRecommendationForApplicationByIdAsync(id);

        return Ok(response);
    }

    [HttpPut]
    [Route("{id:guid}/reject")]
    public async Task<IActionResult> RejectApplicationAsync(Guid id)
    {
        var response = await _applicationService.RejectLoanApplicationByIdAsync(id);

        return Ok(response);
    }
    
    [HttpPost]
    [Route("{id:guid}/approve")]
    public async Task<IActionResult> ApproveApplicationAsync(Guid id)
    {
        var response = await _applicationService.ApproveLoanApplicationByIdAndCreateLoanAsync(id);

        return Ok(response);
    }
}