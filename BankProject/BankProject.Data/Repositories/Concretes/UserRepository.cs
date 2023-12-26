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
    
    public async Task<User> GetByIdAsync(Guid id)
    {
        return await GetUserOrThrowAsync(id);
    }

    public async Task CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, User user)
    {
        var updatingUser = await GetUserOrThrowAsync(id);
        
        updatingUser.Email = user.Email;
        updatingUser.Password = user.Password;

        _dbContext.Users.Update(updatingUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(Guid id)
    {
        var deletingUser = await GetUserOrThrowAsync(id);

        _dbContext.Users.Remove(deletingUser);
        await _dbContext.SaveChangesAsync();
    }
    
    private async Task<User> GetUserOrThrowAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
        {
            throw new NotFoundException("User not found!");
        }

        return user;
    }

    
}