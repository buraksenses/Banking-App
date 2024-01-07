using System.Collections.Concurrent;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankProject.Data.Repositories.Concretes.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _dbContext;
    private IDbContextTransaction _transaction;

    public UnitOfWork(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }

    public async Task TransactionCommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("transaction has not been started!");

        try
        {
            await SaveChangesAsync();

            await _transaction.CommitAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await _transaction.DisposeAsync();
        }
        
    }
}