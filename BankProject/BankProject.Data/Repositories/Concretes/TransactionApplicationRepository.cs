using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionApplicationRepository : GenericRepository<TransactionApplication,Guid>,ITransactionApplicationRepository
{
    public TransactionApplicationRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}