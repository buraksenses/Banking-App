using System.Linq.Expressions;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes.Base;

public class ReadRepository<TEntity,TKey> : GenericRepository<TEntity,TKey> ,IReadRepository<TEntity,TKey> where TEntity : class,IEntity<TKey>
{
    public ReadRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<TEntity?> GetByIdAsync(TKey id) => await _entities.FindAsync(id);
    public async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _entities.Where(predicate).SingleOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate) => await _entities.Where(predicate).ToListAsync();
    public async Task<List<TEntity>> GetAllAsync() => await _entities.ToListAsync();
}