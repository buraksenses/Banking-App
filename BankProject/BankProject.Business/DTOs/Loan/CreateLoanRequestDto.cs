namespace BankProject.Business.DTOs.Loan;

public class CreateLoanRequestDto
{
    public string LoanType { get; set; }
    
    public float LoanAmount { get; set; }
    
    public int LoanTerm { get; set; }
    
    public string UserId { get; set; }
}