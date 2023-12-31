using BankProject.Data.Context;
using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes.Base;

public class CreateRepository<TEntity,TKey> : GenericRepository<TEntity,TKey> ,ICreateRepository<TEntity,TKey> where TEntity : class,IEntity<TKey>
{
    public CreateRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
    
    public async Task CreateAsync(TEntity entity) => await _entities.AddAsync(entity);
}