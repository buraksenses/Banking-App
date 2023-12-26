using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionRepository : ITransactionRepository
{
    private readonly BankDbContext _dbContext;

    public TransactionRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task CreateAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Transaction>> GetByAccountIdAsync(Guid accountId)
    {
        return await _dbContext.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
    }
}