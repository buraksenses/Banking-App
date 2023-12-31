using BankProject.Data.Entities.Base;

namespace BankProject.Data.Repositories.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class, IEntity<TKey>;

    Task CommitAsync();

    Task TransactionCommitAsync();

    Task BeginTransactionAsync();
}