using BankProject.Data.Context;
using BankProject.Data.Entities;
using BankProject.Data.Repositories.Interfaces;

namespace BankProject.Data.Repositories.Concretes;

public class LoanApplicationRepository : ILoanApplicationRepository
{
    private readonly BankDbContext _dbContext;

    public LoanApplicationRepository(BankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateLoanApplicationAsync(LoanApplication loanApplication)
    {
        await _dbContext.LoanApplications.AddAsync(loanApplication);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<LoanApplication?> GetLoanApplicationByIdAsync(Guid id)
    {
        return await _dbContext.LoanApplications.FindAsync(id);
    }
}