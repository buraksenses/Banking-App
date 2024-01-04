using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class TransactionRecordRecordRepository : GenericRepository<TransactionRecord,Guid>, ITransactionRecordRepository
{
    public TransactionRecordRecordRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}