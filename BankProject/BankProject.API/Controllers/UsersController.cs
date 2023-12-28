﻿using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto requestDto)
    {
        var result = await _userService.CreateUserAsync(requestDto);

        if (result.Succeeded)
            return Ok();
        
        return BadRequest(result.Errors);
    }
}