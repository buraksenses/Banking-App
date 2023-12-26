using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id);
    
    Task DeleteAsync(Role role);
}