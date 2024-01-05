using BankProject.Business.DTOs.Auth;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(UserManager<User> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }
   

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new User
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        switch (identityResult.Succeeded)
        {
            case false:
                return BadRequest("Something went wrong!");
            case true:
                return Ok("User was registered! Please login!");
        }
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user == null) return BadRequest("Username or password incorrect!");
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!checkPasswordResult) return BadRequest("Username or password incorrect!");
        
        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Any()) return BadRequest("Username or password incorrect!");
        
        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
        var response = new LoginResponseDto
        {
            JwtToken = jwtToken
        };
        return Ok(response);

    }
}