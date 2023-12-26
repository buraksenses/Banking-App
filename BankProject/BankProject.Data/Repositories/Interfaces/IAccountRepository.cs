using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository
{
    public Task<float> GetBalanceByAccountIdAsync(Guid id);

    public Task CreateAccountAsync(Account account);

    public Task UpdateBalanceByAccountIdAsync(Guid id, float balance);
}