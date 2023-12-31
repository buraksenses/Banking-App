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
    
    public async Task CreateAsync(Loan loan)
    {
        await _dbContext.AddAsync(loan);
        await _dbContext.SaveChangesAsync();
    }
}