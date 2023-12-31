using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class LoanApplication : IEntity<Guid>
{
    public Guid Id { get; set; }
    public LoanType LoanType { get; set; }
    
    public float LoanAmount { get; set; }
    
    public int LoanTerm { get; set; }

    public LoanApplicationStatus LoanApplicationStatus { get; set; }
    
    public string UserId { get; set; }
    
    //Navigation Properties
    public User User { get; set; }
    
}