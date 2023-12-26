using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task DepositAsync(Transaction transaction);

    Task WithdrawAsync(Transaction transaction);

    Task<Transaction> GetTransactionDetailByAccountIdAsync(Guid accountId);

    Task InternalTransferAsync(Transaction transaction);

    Task ExternalTransfer(Transaction transaction);

    Task<Transaction> GetTransferDetailByAccountId(Guid accountId);
}