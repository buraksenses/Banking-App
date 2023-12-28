using BankProject.Core.Enums;

namespace BankProject.Data.Entities;

public class Account
{
    public Guid Id { get; set; }
    public float Balance { get; set; }
    public AccountType AccountType { get; set; }
    public DateTime CreatedDate { get; set; }
    public string UserId { get; set; }

    public Account()
    {
        CreatedDate = DateTime.UtcNow;
    }
}