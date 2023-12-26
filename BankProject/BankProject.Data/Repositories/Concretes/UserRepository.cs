using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public async Task UpdateAsync(User updatingUser, User newUser)
    {
        updatingUser.Email = newUser.Email;
        updatingUser.Password = newUser.Password;

        _dbContext.Users.Update(updatingUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AssignRoleAsync(User user, Role role)
    {
        user.Role = role;
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }
}