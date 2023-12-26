using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id);
    public Task CreateAsync(User user);
    public Task UpdateAsync(User updatingUser, User newUser);
    public Task DeleteAsync(User user);
    public Task AssignRoleAsync(User user, Role role);
    public Task<List<User>> GetAllAsync();
}