using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id);

    public Task CreateAsync(User user);

    public Task UpdateAsync(Guid id, User user);

    public Task DeleteAsync(Guid id);
}