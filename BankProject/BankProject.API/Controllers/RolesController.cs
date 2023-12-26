using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var roleDto = await _service.GetByIdAsync(id);

        return Ok(roleDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        await _service.DeleteByIdAsync(id);

        return Ok();
    }
}