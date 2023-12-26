using BankProject.API.DTOs.User;
using BankProject.Business.DTOs.Role;
using BankProject.Business.DTOs.User;

namespace BankProject.Business.Services.Interfaces;

public interface IUserService
{
    public Task<GetUserRequestDto> GetByIdAsync(Guid id);
    public Task CreateAsync(CreateUserRequestDto requestDto);
    public Task UpdateAsync(Guid id, UpdateUserRequestDto requestDto);
    public Task DeleteByIdAsync(Guid id);
    public Task AssignRoleByIdAsync(Guid id, RoleDto dto);
    public Task<List<GetUserRequestDto>> GetAllAsync();
}