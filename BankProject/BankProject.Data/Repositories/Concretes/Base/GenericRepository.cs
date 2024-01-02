using System.Linq.Expressions;
using BankProject.Core.Exceptions;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes.Base;

public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    private readonly BankDbContext _dbContext;
    private readonly DbSet<TEntity> _entities;

    protected GenericRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
        _entities = _dbContext.Set<TEntity>();
    }

    protected GenericRepository()
    {
        
    }

    public async Task CreateAsync(TEntity entity) => await _entities.AddAsync(entity);
    
    public async Task DeleteAsync(TKey id)
    {
        var entity = await _entities.FindAsync(id);

        if(entity is null)  throw new NotFoundException("Entity not found");

        _entities.Remove(entity);
    }
    
    public async Task<TEntity?> GetByIdAsync(TKey id) => await _entities.FindAsync(id);
    public async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.Where(predicate).SingleOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) => await _entities.Where(predicate).ToListAsync();
    public async Task<List<TEntity>> GetAllAsync() => await _entities.ToListAsync();
    
    public async Task UpdateAsync(TKey id, TEntity entity)
    {
        var current = await _entities.FindAsync(id);

        if(current is null)  throw new NotFoundException("Entity not found");

        _dbContext.Entry(current).CurrentValues.SetValues(entity);
    }
    
}