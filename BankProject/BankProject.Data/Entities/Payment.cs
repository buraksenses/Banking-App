using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class Payment : IEntity<Guid>
{
    public Guid Id { get; set; }

    public float Amount { get; set; }
    
    public DateTime PaymentDate { get; set; }

    public string Description { get; set; }
    
    public Guid AccountId { get; set; }
    
    //Navigation Properties
    public Account Account { get; set; }

    public Payment()
    {
        PaymentDate = DateTime.UtcNow;
    }
}