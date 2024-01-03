using AutoMapper;
using BankProject.Business.DTOs.User;
using BankProject.Business.Services.Interfaces;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Business.Services.Concretes;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(UserManager<User> userManager, IMapper mapper,IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IdentityResult> CreateUserAsync(CreateUserRequestDto requestDto)
    {
        var user = _mapper.Map<User>(requestDto);
        var result = await _userManager.CreateAsync(user, requestDto.Password);

        if (result.Succeeded)
            await _userManager.AddToRolesAsync(user, requestDto.Roles);
        
        return result;
    }

    public async Task ResetDailyLimits()
    {
        var users = await _userManager.Users.Where(user => user.DailyTransferAmount > 0).ToListAsync();

        foreach (var user in users)
        {
            user.DailyTransferAmount = 0;
        }

        await _unitOfWork.SaveChangesAsync();
    }
}