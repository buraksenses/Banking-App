using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class Payment : IEntity<Guid>
{
    public Guid Id { get; set; }

    public float Amount { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public string Description { get; set; }

    public TimePeriod TimePeriod { get; set; }

    public int PaymentFrequency { get; set; }

    public DateTime LastPaymentDate { get; set; }

    public DateTime NextPaymentDate { get; set; }
    
    public Guid AccountId { get; set; }
    
    //Navigation Properties
    public Account Account { get; set; }

    public Payment()
    {
        PaymentDate = DateTime.UtcNow;
        LastPaymentDate = DateTime.UtcNow;
        NextPaymentDate = DateTime.UtcNow;
    }
}