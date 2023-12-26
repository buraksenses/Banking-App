using BankProject.Business.DTOs.Role;

namespace BankProject.Business.Services.Interfaces;

public interface IRoleService
{
    Task<RoleDto> GetByIdAsync(Guid id);

    Task DeleteByIdAsync(Guid id);
}