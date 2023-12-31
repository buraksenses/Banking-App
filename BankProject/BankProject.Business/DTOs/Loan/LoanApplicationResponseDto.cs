namespace BankProject.Business.DTOs.Loan;

public class LoanApplicationResponseDto
{
    public float UserCreditScore { get; set; }

    public int MinimumRequiredCreditScoreForApplication { get; set; }

    public string Recommendation { get; set; }
}