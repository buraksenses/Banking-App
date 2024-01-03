using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class AccountRepository : GenericRepository<Account,Guid>, IAccountRepository
{
    public AccountRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<Account?> UpdateBalanceByAccountIdAsync(Account account, float balance)
    {
        account.Balance = balance;
        await UpdateAsync(account.Id, account);

        return account;
    }
}