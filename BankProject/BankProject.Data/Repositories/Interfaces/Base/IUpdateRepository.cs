using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IUpdateRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task UpdateAsync(TKey id, TEntity entity);
}