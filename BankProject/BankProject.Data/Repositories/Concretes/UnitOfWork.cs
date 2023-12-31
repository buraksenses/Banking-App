using System.Collections;
using System.Collections.Concurrent;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankProject.Data.Repositories.Concretes;

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
    
    public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class, IEntity<TKey>
    {
        var typeKey = $"{typeof(TEntity).FullName}-{typeof(TKey).FullName}";

        if (_repositories.TryGetValue(typeKey, out var repository))
            return (IGenericRepository<TEntity, TKey>)repository;
        try
        {
            var repositoryType = typeof(GenericRepository<,>);
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TKey)), _dbContext);

            if (repositoryInstance is IGenericRepository<TEntity, TKey> typedRepository)
            {
                _repositories[typeKey] = typedRepository;
                return typedRepository;
            }

            throw new InvalidOperationException($"Created repository type does not match the IGenericRepository<{typeof(TEntity).Name}, {typeof(TKey).Name}> interface.");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unable to create repository instance for {typeof(TEntity).Name}: {ex.Message}", ex);
        }
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
    }
    
    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task TransactionCommitAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("Transaction has not been started.");
        
        try
        {
            await _transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await _transaction.RollbackAsync();
            throw new Exception(e.Message);
        }
        finally
        {
            _transaction.Dispose();
        }
    }
}