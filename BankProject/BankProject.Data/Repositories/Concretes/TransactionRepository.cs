using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionRepository : GenericRepository<Transaction,Guid>, ITransactionRepository
{
    public TransactionRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}