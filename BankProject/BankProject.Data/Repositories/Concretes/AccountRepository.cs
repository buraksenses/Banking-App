using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class AccountRepository : IAccountRepository
{
    private readonly BankDbContext _dbContext;

    public AccountRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Account?> GetByIdAsync(Guid id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        return account ?? null;
    }

    public async Task CreateAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Account?> UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account == null)
            return null;
        
        account.Balance = balance;
        
        await _dbContext.SaveChangesAsync();

        return account;
    }
}