using BankProject.Data.Entities;

namespace BankProject.Data.Repositories.Interfaces;

public interface ILoanRepository
{
    Task CreateLoanApplicationAsync(LoanApplication loanApplication);
}