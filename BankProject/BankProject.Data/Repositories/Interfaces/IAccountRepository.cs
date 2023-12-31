using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository : IGenericRepository<Account,Guid>
{
    public Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance);
}