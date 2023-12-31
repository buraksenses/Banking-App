using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IDeleteRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task DeleteAsync(TKey id);
}