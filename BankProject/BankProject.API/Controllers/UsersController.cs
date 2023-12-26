using BankProject.API.DTOs.User;
using BankProject.Business.DTOs.Role;
using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await _service.GetAllAsync();

        return Ok(users);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var requestDto = await _service.GetByIdAsync(id);

        return Ok(requestDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateUserRequestDto requestDto)
    { 
        await _service.CreateAsync(requestDto);

        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut]
    [Route("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);

        return Ok(requestDto);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        await _service.DeleteByIdAsync(id);

        return Ok();
    }

    [HttpPut]
    [Route("{id:guid}/roles")]
    public async Task<IActionResult> AssignRoleByIdAsync(Guid id, RoleDto roleDto)
    {
        await _service.AssignRoleByIdAsync(id, roleDto);

        return Ok();
    }
}