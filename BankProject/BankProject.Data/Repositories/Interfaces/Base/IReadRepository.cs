using System.Linq.Expressions;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IReadRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity,bool>> predicate);
}