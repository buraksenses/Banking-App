using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository : IGenericRepository<Account,Guid>
{
    public Task<Account?> UpdateBalanceByAccountIdAsync(Account account, decimal balance);
}