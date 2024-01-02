using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;
using BankProject.Data.Repositories.Interfaces.Base;

namespace BankProject.Data.Repositories.Concretes;

public class LoanRepository : GenericRepository<Loan,Guid>,ILoanRepository
{
    public LoanRepository(BankDbContext dbContext) : base(dbContext)
    {
    }
}