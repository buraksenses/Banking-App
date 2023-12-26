using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository
{
    public Task<Account?> GetByIdAsync(Guid id);
    public Task CreateAsync(Account account);
    public Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance);
}