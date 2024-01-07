using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Data.Entities;

namespace BankProject.Business.Services.Concretes;

public class CreditScoreService : ICreditScoreService
{
    public decimal CalculateCreditScore(User user)
    {
        decimal score;

        //This gives 20 points for every $1000 of annual income.
        var incomeScore = user.AnnualIncome / 1000 * 20;

        //Assigns 15 points for every $1000 in total assets.
        var totalAssetsScore = user.TotalAssets / 1000 * 15;
        
        decimal loanTypeScore = 0;
        decimal remainingDebtBurdenScore = 0;
        decimal paymentPerformanceScore = 0;

        if (user.Loans.Any())
        {
            foreach (var userLoan in user.Loans)
            {
                switch (userLoan.LoanType)
                {
                    case LoanType.Mortgage:
                        loanTypeScore += 30;
                        break;
                    case LoanType.Education:
                        loanTypeScore += 25;
                        break;
                    case LoanType.Personal:
                        loanTypeScore += 15;
                        break;
                    case LoanType.Vehicle:
                        loanTypeScore += 20;
                        break;
                    case LoanType.SmallBusiness:
                        loanTypeScore += 35;
                        break;
                }

                remainingDebtBurdenScore = (1 - userLoan.RemainingDebt / userLoan.LoanAmount) * 25;
            }
        }

        var loansStartedToBePaid = user.Loans.Where(loan => loan.NumberOfTotalPayments > 0).ToList();
        
        if (loansStartedToBePaid.Any())
        {
            paymentPerformanceScore = loansStartedToBePaid
                .Sum(loan => loan.NumberOfTimelyPayments / loan.NumberOfTotalPayments * 30);
        }

        score = incomeScore + totalAssetsScore + paymentPerformanceScore + remainingDebtBurdenScore + loanTypeScore;

        return score;
    }

    public decimal CalculateMinimumRequiredCreditScoreForLoanApplication(LoanApplication loanApplication)
    {
        decimal score = 0;

        switch (loanApplication.LoanType)
        {
            case LoanType.Mortgage:
                score += 30;
                break;
            case LoanType.Education:
                score += 25;
                break;
            case LoanType.Personal:
                score += 15;
                break;
            case LoanType.Vehicle:
                score += 20;
                break;
            case LoanType.SmallBusiness:
                score += 35;
                break;
        }

        //5 points added for each $10.000
        score += loanApplication.LoanAmount / 10000 * 5;

        //10 points added for each 12 month term
        score += loanApplication.LoanTerm / 12 * 10;

        return score;
    }

}