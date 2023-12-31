using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IGenericRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
{
    
}