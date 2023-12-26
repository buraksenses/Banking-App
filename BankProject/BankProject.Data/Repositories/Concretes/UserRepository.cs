using BankProject.Core.Exceptions;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class UserRepository : IUserRepository
{
    private readonly BankDbContext _dbContext;

    public UserRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, User user)
    {
        var updatingUser = await GetByIdAsync(id);

        if (updatingUser != null)
        {
            updatingUser.Email = user.Email;
            updatingUser.Password = user.Password;

            _dbContext.Users.Update(updatingUser);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteByIdAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }


}