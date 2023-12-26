using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task CreateAsync(Transaction transaction);

    Task<List<Transaction>> GetByAccountIdAsync(Guid accountId);
}