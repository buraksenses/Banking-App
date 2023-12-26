using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionRepository : ITransactionRepository
{
    public Task DepositAsync(Transaction transaction)
    {
        return null;
    }

    public Task WithdrawAsync(Transaction transaction)
    {
        return null;
    }

    public Task<Transaction> GetTransactionDetailByAccountIdAsync(Guid accountId)
    {
        return null;
    }

    public Task InternalTransferAsync(Transaction transaction)
    {
        return null;
    }

    public Task ExternalTransfer(Transaction transaction)
    {
        return null;
    }

    public Task<Transaction> GetTransferDetailByAccountId(Guid accountId)
    {
        return null;
    }
}