using BankProject.Core.Enums;
using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Concretes.Base;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class LoanApplicationRepository : GenericRepository<LoanApplication,Guid>,ILoanApplicationRepository
{
    public LoanApplicationRepository(BankDbContext dbContext) : base(dbContext)
    {
    }

    public async Task UpdateLoanApplicationStatusAsync(LoanApplication application, LoanApplicationStatus status)
    {
        application.LoanApplicationStatus = status;
    }
}