using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task AddAsync(TEntity entity);
    
    Task<TEntity?> GetByIdAsync(TKey id);
}