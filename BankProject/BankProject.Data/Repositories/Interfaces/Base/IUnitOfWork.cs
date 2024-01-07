using BankProject.Data.Entities.Base;
using BankProject.Data.Repositories.Concretes.Base;

namespace BankProject.Data.Repositories.Interfaces.Base;

public interface IUnitOfWork
{
    Task SaveChangesAsync();

    Task BeginTransactionAsync();

    Task TransactionCommitAsync();
}