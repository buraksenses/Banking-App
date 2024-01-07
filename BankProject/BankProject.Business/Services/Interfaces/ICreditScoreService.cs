using BankProject.Data.Entities;

namespace BankProject.Business.Services.Interfaces;

public interface ICreditScoreService
{
    decimal CalculateCreditScore(User user);

    decimal CalculateMinimumRequiredCreditScoreForLoanApplication(LoanApplication loanApplication);
}