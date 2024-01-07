using BankProject.Business.Services.Interfaces;
using BankProject.Core.Enums;
using BankProject.Data.Entities;

namespace BankProject.Business.Services.Concretes;

public class CreditScoreService : ICreditScoreService
{
    private const decimal IncomeScoreMultiplier = 20m;
    private const decimal AssetScoreMultiplier = 15m;
    private const decimal DebtBurdenScoreMultiplier = 25m;
    private const decimal PaymentPerformanceScoreMultiplier = 30m;
    private const decimal LoanAmountScoreMultiplier = 5m;
    private const decimal LoanTermScoreMultiplier = 10m;
    private const int ThousandDollars = 1000;
    private const int TenThousandDollars = 10000;
    private const int TwelveMonths = 12;

    private const int MortgageScore = 30;
    private const int EducationLoanScore = 25;
    private const int PersonalLoanScore = 15;
    private const int VehicleLoanScore = 20;
    private const int SmallBusinessLoanScore = 35;

    public decimal CalculateCreditScore(User user)
    {
        var incomeScore = user.AnnualIncome / ThousandDollars * IncomeScoreMultiplier;
        var totalAssetsScore = user.TotalAssets / ThousandDollars * AssetScoreMultiplier;

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
                        loanTypeScore += MortgageScore;
                        break;
                    case LoanType.Education:
                        loanTypeScore += EducationLoanScore;
                        break;
                    case LoanType.Personal:
                        loanTypeScore += PersonalLoanScore;
                        break;
                    case LoanType.Vehicle:
                        loanTypeScore += VehicleLoanScore;
                        break;
                    case LoanType.SmallBusiness:
                        loanTypeScore += SmallBusinessLoanScore;
                        break;
                }

                remainingDebtBurdenScore = (1 - userLoan.RemainingDebt / userLoan.LoanAmount) * DebtBurdenScoreMultiplier;
            }
        }

        var loansStartedToBePaid = user.Loans.Where(loan => loan.NumberOfTotalPayments > 0).ToList();

        if (loansStartedToBePaid.Any())
        {
            paymentPerformanceScore = loansStartedToBePaid
                .Sum(loan => loan.NumberOfTimelyPayments / loan.NumberOfTotalPayments * PaymentPerformanceScoreMultiplier);
        }

        return incomeScore + totalAssetsScore + paymentPerformanceScore + remainingDebtBurdenScore + loanTypeScore;
    }

    public decimal CalculateMinimumRequiredCreditScoreForLoanApplication(LoanApplication loanApplication)
    {
        decimal score = 0;

        switch (loanApplication.LoanType)
        {
            case LoanType.Mortgage:
                score += MortgageScore;
                break;
            case LoanType.Education:
                score += EducationLoanScore;
                break;
            case LoanType.Personal:
                score += PersonalLoanScore;
                break;
            case LoanType.Vehicle:
                score += VehicleLoanScore;
                break;
            case LoanType.SmallBusiness:
                score += SmallBusinessLoanScore;
                break;
        }

        score += loanApplication.LoanAmount / TenThousandDollars * LoanAmountScoreMultiplier;
        score += loanApplication.LoanTerm / TwelveMonths * LoanTermScoreMultiplier;

        return score;
    }
}
