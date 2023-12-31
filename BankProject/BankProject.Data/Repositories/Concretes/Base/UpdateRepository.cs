using BankProject.Core.Exceptions;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes.Base;

public class UpdateRepository<TEntity, TKey> : GenericRepository<TEntity,TKey> ,IUpdateRepository<TEntity,TKey> where TEntity : class,IEntity<TKey>
{
    public UpdateRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task UpdateAsync(TKey id, TEntity entity)
    {
        var current = await _entities.FindAsync(id);

        if(entity is null)  throw new NotFoundException("Entity not found");

        _dbContext.Entry(current).CurrentValues.SetValues(entity);
    }
}