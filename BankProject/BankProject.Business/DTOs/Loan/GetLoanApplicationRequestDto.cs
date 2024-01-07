namespace BankProject.Business.DTOs.Loan;

public class GetLoanApplicationRequestDto
{
    public string LoanType { get; set; }
    
    public decimal LoanAmount { get; set; }
    
    public int LoanTerm { get; set; }

    public string LoanApplicationStatus { get; set; }

    public string UserId { get; set; }
}