using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes.Base;

public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    protected readonly BankDbContext _dbContext;

    protected readonly DbSet<TEntity> _entities;

    protected GenericRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<TEntity>();
    }
    
}