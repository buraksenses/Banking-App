namespace BankProject.Business.DTOs.Loan;

public class LoanApplicationResponseDto
{
    public decimal UserCreditScore { get; set; }

    public decimal MinimumRequiredCreditScoreForApplication { get; set; }

    public string Recommendation { get; set; }
}