using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class TransactionApplication : IEntity<Guid>
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }
    
    public DateTime CreatedDate { get; set; }
    
    public TransactionType TransactionType { get; set; }
    
    public TransactionApplicationStatus Status { get; set; }
    
    public Guid AccountId { get; set; }
    //Navigation Properties
    public Account Account { get; set; }

    public TransactionApplication()
    {
        CreatedDate = DateTime.UtcNow;
    }
}