using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository
{
    public Task<Account?> GetAccountByIdAsync(Guid id);
    public Task CreateAccountAsync(Account account);
    public Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance);
}