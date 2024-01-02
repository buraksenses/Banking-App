using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class PaymentRepository : GenericRepository<Payment,Guid>,IPaymentRepository
{
    public PaymentRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}