using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class LoanRepository : ILoanRepository
{
    private readonly BankDbContext _dbContext;

    public LoanRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateLoanApplication(LoanApplication loanApplication)
    {
        await _dbContext.LoanApplications.AddAsync(loanApplication);
        await _dbContext.SaveChangesAsync();
    }
}