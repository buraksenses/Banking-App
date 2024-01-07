using BankProject.Core.Enums;
using BankProject.Data.Entities.Base;

namespace BankProject.Data.Entities;

public class Account : IEntity<Guid>
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public AccountType AccountType { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }

    public Account()
    {
        CreatedDate = DateTime.UtcNow;
    }
}