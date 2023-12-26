using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IRolesRepository
{
    Task<Role?> GetByIdAsync(Guid id);

    Task CreateAsync(Role role);

    Task DeleteAsync(Role role);
}