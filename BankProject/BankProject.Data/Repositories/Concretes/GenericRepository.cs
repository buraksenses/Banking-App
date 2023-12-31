using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly BankDbContext _dbContext;

    private readonly DbSet<TEntity> _entities;

    public GenericRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<TEntity>();
    }
    
    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        return await _entities.FindAsync(id);
    }
}