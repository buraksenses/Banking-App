using BankProject.Business.DTOs.Auth;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankProject.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }
   
    //POST: /api/Auth/Register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };

        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (!identityResult.Succeeded) return BadRequest("Something went wrong!");
        
        //Add roles to this user
        if (!registerRequestDto.Roles.Any()) return BadRequest("Something went wrong!");
        identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

        if (identityResult.Succeeded)
            return Ok("User was registered! Please login!");
        

        return BadRequest("Something went wrong!");
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);

        if (user == null) return BadRequest("Username or password incorrect!");
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!checkPasswordResult) return BadRequest("Username or password incorrect!");
        //Get Roles for this user
        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Any()) return BadRequest("Username or password incorrect!");
        //Create Token
        var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());
        var response = new LoginResponseDto
        {
            JwtToken = jwtToken
        };
        return Ok(response);

    }
}