using BankProject.Business.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Business.Services.Interfaces;

public interface IUserService
{
    Task<IdentityResult> CreateUserAsync(CreateUserRequestDto requestDto);

    Task UpdateUserAsync(string id, UpdateUserRequestDto requestDto);

    Task ResetDailyLimits();
}