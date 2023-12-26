using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class RolesRepository : IRolesRepository
{
    private readonly BankDbContext _dbContext;

    public RolesRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Role?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Roles.FindAsync(id);
    }

    public async Task CreateAsync(Role role)
    {
        await _dbContext.Roles.AddAsync(role);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Role role)
    {
        _dbContext.Remove(role);
        await _dbContext.SaveChangesAsync();
    }
}