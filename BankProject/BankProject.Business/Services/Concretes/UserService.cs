using AutoMapper;
using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Concretes;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<IdentityResult> CreateUserAsync(CreateUserRequestDto requestDto)
    {
        var user = _mapper.Map<IdentityUser>(requestDto);
        var result = await _userManager.CreateAsync(user, requestDto.Password);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, requestDto.Roles);
        
        return result;
    }

}