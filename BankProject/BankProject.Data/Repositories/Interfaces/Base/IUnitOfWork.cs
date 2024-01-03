using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Concretes.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IUnitOfWork
{
    TRepository GetRepository<TRepository, TEntity, TKey>() 
        where TRepository : GenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>;

    Task CommitAsync();
}