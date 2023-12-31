using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface IAccountRepository : ICreateRepository<Account,Guid>,IReadRepository<Account,Guid>
{
    public Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance);
}