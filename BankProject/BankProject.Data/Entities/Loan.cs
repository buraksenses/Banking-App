using System.ComponentModel.DataAnnotations.Schema;
using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class Loan : IEntity<Guid>
{
    public Guid Id { get; set; }
    
    public DateTime LoanDate { get; set; }
    public LoanType LoanType { get; set; }
    
    public decimal LoanAmount { get; set; }

    public int NumberOfTotalPayments { get; set; }

    public decimal MonthlyPayment => RemainingDebt / LoanTerm;
    public int LoanTerm { get; set; }
    public decimal RemainingDebt { get; set; }

    [NotMapped]
    public bool IsPaid => RemainingDebt == 0;
    
    public int NumberOfTimelyPayments { get; set; }

    public DateTime LastPaymentDate { get; set; }

    public DateTime NextPaymentDueDate { get; set; }
    
    public string UserId { get; set; }
    
    //Navigation Properties
    public User User { get; set; }

    public Loan()
    {
        LoanDate = DateTime.UtcNow;
        NextPaymentDueDate = LoanDate.AddMonths(1);
        LastPaymentDate = new DateTime();
    }
}