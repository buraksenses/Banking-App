using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface ICreateRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task CreateAsync(TEntity entity);
}