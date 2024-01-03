using System.Collections.Concurrent;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankProject.Data.Repositories.Concretes.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _dbContext;
    private readonly ConcurrentDictionary<string, object> _repositories;
    private IDbContextTransaction _transaction;

    public UnitOfWork(BankDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public TRepository GetRepository<TRepository, TEntity, TKey>() 
        where TRepository : GenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        var typeKey = typeof(TRepository).FullName ?? throw new InvalidOperationException("Repository type key cannot be null.");

        if (_repositories.TryGetValue(typeKey, out var repository))
        {
            return (TRepository)repository;
        }

        var newRepository = Activator.CreateInstance(typeof(TRepository), _dbContext) as TRepository 
                            ?? throw new InvalidOperationException($"Unable to create instance of type {typeof(TRepository).Name}.");

        _repositories[typeKey] = newRepository;

        return newRepository;
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