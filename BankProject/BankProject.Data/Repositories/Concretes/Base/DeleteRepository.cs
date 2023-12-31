using BankProject.Core.Exceptions;
using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;
using Microsoft.EntityFrameworkCore;

namespace BankProject.Data.Repositories.Concretes.Base;

public class DeleteRepository<TEntity,TKey> : GenericRepository<TEntity,TKey> ,IDeleteRepository<TEntity,TKey> where TEntity : class,IEntity<TKey>
{
    public DeleteRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task DeleteAsync(TKey id)
    {
        var entity = await _entities.FindAsync(id);

        if(entity is null)  throw new NotFoundException("Entity not found");

        _entities.Remove(entity);
    }
}