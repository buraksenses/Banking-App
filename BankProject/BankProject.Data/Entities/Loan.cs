using System.ComponentModel.DataAnnotations.Schema;
using BankProject.Core.Enums;

namespace BankProject.Data.Entities;

public class Loan
{
    public Guid Id { get; set; }
    
    public DateTime LoanDate { get; set; }
    public LoanType LoanType { get; set; }
    
    public float LoanAmount { get; set; }

    public int NumberOfTotalPayments { get; set; }

    public float MonthlyPayment => RemainingDebt / LoanTerm;
    public int LoanTerm { get; set; }
    public float RemainingDebt { get; set; }

    [NotMapped]
    public bool IsPaid => RemainingDebt == 0;
    
    public int NumberOfTimelyPayments { get; set; }
    
    public string UserId { get; set; }
    
    //Navigation Properties
    public User User { get; set; }
}