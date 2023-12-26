using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class RoleRepository : IRoleRepository
{
    private readonly BankDbContext _dbContext;

    public RoleRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Roles.FindAsync(id);
    }

    public async Task DeleteAsync(Role role)
    {
        _dbContext.Remove(role);
        await _dbContext.SaveChangesAsync();
    }
}