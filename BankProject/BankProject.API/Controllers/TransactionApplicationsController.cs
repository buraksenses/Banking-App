using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

public class TransactionApplicationsController : CustomControllerBase
{
    private readonly ITransactionApplicationService _service;

    public TransactionApplicationsController(ITransactionApplicationService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("{id:guid}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveApplication(Guid id)
    {
        await _service.ApproveApplicationAsync(id);
        return Ok();
    }

    [HttpPost]
    [Route("{id:guid}/reject")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RejectApplication(Guid id)
    {
        await _service.RejectApplicationAsync(id);
        return Ok();
    }
}