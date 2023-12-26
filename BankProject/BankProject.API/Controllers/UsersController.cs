using BankProject.API.DTOs.User;
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
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateUserRequestDto requestDto)
    {
        await _service.UpdateAsync(id, requestDto);

        return Ok(requestDto);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        await _service.DeleteByIdAsync(id);

        return Ok();
    }
}