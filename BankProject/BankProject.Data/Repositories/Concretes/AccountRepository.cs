using BankProject.Core.Exceptions;
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
    
    public async Task<float> GetBalanceByAccountIdAsync(Guid id)
    {
        var account = await GetAccountOrThrow(id);

        return account.Balance;
    }

    public async Task CreateAccountAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateBalanceByAccountIdAsync(Guid id, float balance)
    {
        var account = await GetAccountOrThrow(id);

        account.Balance = balance;
        
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<Account> GetAccountOrThrow(Guid id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account == null)
            throw new NotFoundException("Account not found!");

        return account;
    }
}