using BankProject.Core.Enums;
using Microsoft.AspNetCore.Identity;

namespace BankProject.Data.Entities;

public class Account
{
    public Guid Id { get; set; }

    public float Balance { get; set; }

    public AccountType AccountType { get; set; }
    public DateTime CreatedDate { get; set; }

    public Guid UserId { get; set; }
    
    //Navigation Properties
    public IdentityUser User { get; set; }

    public Account()
    {
        CreatedDate = DateTime.UtcNow;
    }
}